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

    public async Task<IEnumerable<ComCustomer>> GetAllCustomers()
    {
        return await _appDbContext.ComCustomers.ToListAsync();
    }
}