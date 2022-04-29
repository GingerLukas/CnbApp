using System.Runtime.InteropServices;
using CnbApp.Data;

namespace CnbApp.Services;

public class CurrencyApiService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly HttpClient _httpClient;

    public const string CNB_API_ENDPOINT =
        "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

    private const int HEADER_LEN = 2;

    private const int COUNTRY_INDEX = 0;
    private const int NAME_INDEX = 1;
    private const int COUNT_INDEX = 2;
    private const int ID_INDEX = 3;
    private const int PRICE_INDEX = 4;
    private const int FIELDS_COUNT = 5;

    private const int VALIDITY_TIME_IN_MINUTES = 1;


    public CurrencyApiService(ApplicationDbContext dbContext, HttpClient httpClient)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
    }

    public IEnumerable<Currency> GetAll()
    {
        var data = _dbContext.Currencies.First();
        
        return _dbContext.Currencies.ToList();
    }

    public Currency? GetCurrency(string currencyCode)
    {
        Currency? currency = _dbContext.Currencies.Find(currencyCode);
        if (currency == null)
        {
            UpdateCurrencies();
            //TODO: request new currency data
        }
        else
        {
            CurrencyData? lastData = currency.GetLatest();

            if (lastData == null ||
                (DateTimeOffset.Now - lastData.Time) > TimeSpan.FromMinutes(VALIDITY_TIME_IN_MINUTES))
            {
                UpdateCurrencies();
                //TODO: request new currency data
            }
        }

        return currency ?? _dbContext.Find<Currency>(currencyCode);
    }

    public async void UpdateCurrencies()
    {
        string[] data = ( _httpClient.GetStringAsync(CNB_API_ENDPOINT).GetAwaiter().GetResult()).Split('\n');
        DateTimeOffset time = DateTime.Now;
        string header0 = data[0];
        string header1 = data[1];
        foreach (string s in data[HEADER_LEN..])
        {
            string[] values;
            if (string.IsNullOrWhiteSpace(s) || (values = s.Split('|')).Length != FIELDS_COUNT)
            {
                continue;
            }

            Currency currency = GetOrCreateCurrency(values[ID_INDEX], values[NAME_INDEX], values[COUNTRY_INDEX]);

            CurrencyData? latest = currency.GetLatest();

            if (latest == null || (time - latest.Time) > TimeSpan.FromMinutes(VALIDITY_TIME_IN_MINUTES))
            {
                if (!decimal.TryParse(values[COUNT_INDEX], out decimal count))
                {
                    throw new Exception("Invalid decimal format for count");
                }

                if (!decimal.TryParse(values[PRICE_INDEX], out decimal price))
                {
                    throw new Exception("Invalid decimal format for price");
                }

                latest = new CurrencyData()
                {
                    Time = time,
                    Count = count,
                    Price = price
                };
                
                currency.Datas.Add(latest);
                _dbContext.Add(latest);
            }
        }

        _dbContext.SaveChanges();
    }

    public Currency GetOrCreateCurrency(string id, string name, string country)
    {
        Currency? currency = _dbContext.Find<Currency>(id);
        if (currency != null)
        {
            return currency;
        }

        currency = new Currency(id, name, country);
        _dbContext.Add(currency);
        _dbContext.SaveChanges();
        return currency;
    }
}