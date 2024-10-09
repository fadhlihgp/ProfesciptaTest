using Microsoft.EntityFrameworkCore;
using ProfesciptaTest.Context;
using ProfesciptaTest.Entities;
using ProfesciptaTest.Repositories.Interfaces;

namespace ProfesciptaTest.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _appDbContext;

    public CustomerRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<ComCustomer> GetAllCustomers()
    {
        return _appDbContext.ComCustomers.ToList();
    }
}