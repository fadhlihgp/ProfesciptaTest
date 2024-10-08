using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfesciptaTest.Models;
using ProfesciptaTest.Repositories.Interfaces;

namespace ProfesciptaTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICustomerRepository _customerRepository;

    public HomeController(ILogger<HomeController> logger, ICustomerRepository customerRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> AddNew()
    {
        ViewBag.Customers = await _customerRepository.GetAllCustomers();
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}