using System.Diagnostics;
using CnbApp.Data;
using Microsoft.AspNetCore.Mvc;
using CnbApp.Models;
using CnbApp.Services;

namespace CnbApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CurrencyApiService _currencyApiService;

    public HomeController(ILogger<HomeController> logger, CurrencyApiService currencyApiService)
    {
        _logger = logger;
        _currencyApiService = currencyApiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("test")]
    public IEnumerable<Currency> GetCurrencies()
    {
        _currencyApiService.UpdateCurrencies();
        return _currencyApiService.GetAll();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}