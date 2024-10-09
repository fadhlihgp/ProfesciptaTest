using ProfesciptaTest.Entities;
using ProfesciptaTest.ViewModels;

namespace ProfesciptaTest.Repositories.Interfaces;

public interface ISoOrderRepository
{
    public IEnumerable<SoOrder> GetAllSoOrder(string? keyword, DateTime? orderDate);
    public SoOrderResponseVM? GetSoOrderDetail(long id);
    public void CreateOrder(SoOrderRequestVM soOrderRequestVm);
    public void EditOrder(long id, SoOrderRequestEditVM soOrderRequestEditVm);
    public void DeleteOrder(long id);
}