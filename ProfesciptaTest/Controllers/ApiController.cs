using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfesciptaTest.Repositories.Interfaces;
using ProfesciptaTest.ViewModels;

namespace ProfesciptaTest.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ISoOrderRepository _orderRepository;

    public ApiController(ICustomerRepository customerRepository, ISoOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
    }

    [HttpPut("EditOrder/{id}")]
    public IActionResult EditOrder([FromRoute]long id, [FromBody] SoOrderRequestEditVM soOrderRequestEditVm)
    {
        _orderRepository.EditOrder(id, soOrderRequestEditVm);
        return Ok();
    }
}