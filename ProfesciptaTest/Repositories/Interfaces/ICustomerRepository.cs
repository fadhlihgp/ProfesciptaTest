using ProfesciptaTest.Entities;

namespace ProfesciptaTest.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<ComCustomer>> GetAllCustomers();
}