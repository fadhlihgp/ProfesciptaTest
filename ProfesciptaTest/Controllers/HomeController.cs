using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
    public IActionResult DownloadExcel(string keyword = "", DateTime? orderDate = null)
    {
        var allSoOrder = _orderRepository.GetAllSoOrder(keyword, orderDate);
        ViewBag.Keyword = keyword;
        ViewBag.OrderDate = orderDate?.ToString("yyyy-MM-dd");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("SoOrders");

            worksheet.Cells[1, 1].Value = "Order No";
            worksheet.Cells[1, 2].Value = "Order Date";
            worksheet.Cells[1, 3].Value = "Customer";
            worksheet.Cells[1, 4].Value = "Address";
            
            int row = 2;

            foreach (var soOrder in allSoOrder)
            { 
                worksheet.Cells[row, 1].Value = soOrder.OrderNo;
                worksheet.Cells[row, 2].Value = soOrder.OrderDate.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 3].Value = soOrder.ComCustomer?.CustomerName;
                worksheet.Cells[row, 4].Value = soOrder.Address;
                row++;
            }
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            
            // Sheet 2
            var itemWorksheet = package.Workbook.Worksheets.Add("SoItems");

            itemWorksheet.Cells[1, 1].Value = "Order Id";
            itemWorksheet.Cells[1, 2].Value = "Item Name";
            itemWorksheet.Cells[1, 3].Value = "Quantity";
            itemWorksheet.Cells[1, 4].Value = "Price";
            itemWorksheet.Cells[1, 5].Value = "Total";
            
            int rowItem = 2;

            foreach (var soOrder in allSoOrder)
            { 
                foreach (var item in soOrder.SoItems)
                {
                    itemWorksheet.Cells[rowItem, 1].Value = soOrder.SoOrderId;
                    itemWorksheet.Cells[rowItem, 2].Value = item.ItemName;
                    itemWorksheet.Cells[rowItem, 3].Value = item.Quantity;
                    itemWorksheet.Cells[rowItem, 4].Value = item.Price;
                    itemWorksheet.Cells[rowItem, 5].Value = item.Price * item.Quantity;
                    rowItem++;
                }
            }
            
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var excelData = package.GetAsByteArray();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SoOrders.xlsx");
        }
    }

    [HttpPost]
    public IActionResult DeleteOrder(long id)
    {
        _orderRepository.DeleteOrder(id);
        return RedirectToAction("Index");
    }

    public IActionResult AddNew(SoOrderRequestVM? soOrderRequestVm)
    {
        var orderRequest = new SoOrderRequestVM();
        if (soOrderRequestVm != null)
        {
            orderRequest = soOrderRequestVm;
        }
        ViewBag.Customers = _customerRepository.GetAllCustomers();
        return View(orderRequest);
    }

    [HttpPost]
    public IActionResult AddNewOrder([FromBody] SoOrderRequestVM soOrderRequestVm)
    {
        if (ModelState.IsValid)
        {
            _orderRepository.CreateOrder(soOrderRequestVm);
            return RedirectToAction("Index");
        }
        else
        {
            return AddNew(soOrderRequestVm);
        }
    }

    public async Task<IActionResult> Edit(long id)
    {
        var soOrderResponseVm = _orderRepository.GetSoOrderDetail(id);
        var orderRequest = new SoOrderRequestVM();
        ViewBag.Customers = _customerRepository.GetAllCustomers();
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