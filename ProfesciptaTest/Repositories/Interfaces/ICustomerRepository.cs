using ProfesciptaTest.Entities;

namespace ProfesciptaTest.Repositories.Interfaces;

public interface ICustomerRepository
{
    IEnumerable<ComCustomer> GetAllCustomers();
}