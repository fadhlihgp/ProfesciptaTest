using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfesciptaTest.Models;
using ProfesciptaTest.Repositories.Interfaces;
using ProfesciptaTest.ViewModels;

namespace ProfesciptaTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly ISoOrderRepository _orderRepository;

    public HomeController(ILogger<HomeController> logger, ICustomerRepository customerRepository, ISoOrderRepository orderRepository)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    public IActionResult Index(string keyword = "", DateTime? orderDate = null)
    {
        var allSoOrder = _orderRepository.GetAllSoOrder(keyword, orderDate);
        ViewBag.Keyword = keyword;
        ViewBag.OrderDate = orderDate?.ToString("yyyy-MM-dd");
        return View(allSoOrder);
    }

    [HttpPost]
    public IActionResult DeleteOrder(long id)
    {
        _orderRepository.DeleteOrder(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> AddNew()
    {
        var orderRequest = new SoOrderRequestVM();
        ViewBag.Customers = await _customerRepository.GetAllCustomers();
        return View(orderRequest);
    }

    [HttpPost]
    public IActionResult AddNew([FromBody] SoOrderRequestVM soOrderRequestVm)
    {
        _orderRepository.CreateOrder(soOrderRequestVm);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(long id)
    {
        var soOrderResponseVm = _orderRepository.GetSoOrderDetail(id);
        var orderRequest = new SoOrderRequestVM();
        ViewBag.Customers = await _customerRepository.GetAllCustomers();
        ViewBag.OrderId = id;
        return View(soOrderResponseVm);
    }

    [HttpPut("EditOrder/{id}")]
    public IActionResult EditOrder([FromRoute]long id, [FromBody] SoOrderRequestEditVM soOrderRequestEditVm)
    {
        _orderRepository.EditOrder(id, soOrderRequestEditVm);
        return RedirectToAction("Index");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}