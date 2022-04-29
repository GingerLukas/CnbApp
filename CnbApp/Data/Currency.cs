namespace CnbApp.Data;

public class Currency
{
    public string CurrencyId { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public ICollection<CurrencyData> Datas { get; set; } = new HashSet<CurrencyData>();

    public Currency(string country)
    {
        Country = country;
    }

    public Currency(string currencyId, string name, string country)
    {
        CurrencyId = currencyId;
        Name = name;
        Country = country;
    }

    public CurrencyData? GetLatest() => Datas.OrderByDescending(x => x.Time).FirstOrDefault();
}

public class CurrencyData
{
    public string CurrencyDataId { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset Time { get; set; }
    public decimal Price { get; set; }
    public decimal Count { get; set; }
}