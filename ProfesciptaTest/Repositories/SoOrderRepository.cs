using Microsoft.EntityFrameworkCore;
using ProfesciptaTest.Context;
using ProfesciptaTest.Entities;
using ProfesciptaTest.Repositories.Interfaces;
using ProfesciptaTest.ViewModels;

namespace ProfesciptaTest.Repositories;

public class SoOrderRepository : ISoOrderRepository
{
    private readonly AppDbContext _appDbContext;

    public SoOrderRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IEnumerable<SoOrder> GetAllSoOrder(string? keyword, DateTime? orderDate)
    {
        var orders = _appDbContext.SoOrders.AsQueryable();
        orders = orders.Include(o => o.ComCustomer).Include(i => i.SoItems);
        if (!string.IsNullOrEmpty(keyword))
        {
            orders = orders.Where(o => o.OrderNo.Contains(keyword) | o.ComCustomer.CustomerName.Contains(keyword));
        }

        if (orderDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate.Date == orderDate.Value.Date);
        }

        return orders.ToList();
    }

    public SoOrderResponseVM? GetSoOrderDetail(long id)
    {
        var order = _appDbContext.SoOrders
            .Include(s => s.SoItems)
            .Include(s => s.ComCustomer)
            .FirstOrDefault(s => s.SoOrderId == id);

        return new SoOrderResponseVM
        {
            SoOrderId = order.SoOrderId,
            OrderNo = order.OrderNo,
            OrderDate = order.OrderDate,
            ComCustomerId = order.ComCustomerId,
            CustomerName = order.ComCustomer.CustomerName,
            Address = order.Address,
            Items = order.SoItems.Select(i => new SoItemResponseVM
            {
                SoItemId = i.SoItemId,
                SoOrderId = i.SoOrderId,
                ItemName = i.ItemName,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
    }

    public void CreateOrder(SoOrderRequestVM soOrderRequestVm)
    {
        var soOrder = new SoOrder
        {
            OrderNo = soOrderRequestVm.OrderNo,
            OrderDate = soOrderRequestVm.OrderDate,
            ComCustomerId = soOrderRequestVm.ComCustomerId,
            Address = soOrderRequestVm.Address,
            SoItems = soOrderRequestVm.Items.Select(o => new SoItem
            {
                ItemName = o.ItemName,
                Quantity = o.Quantity,
                Price = o.Price,
            }).ToList() 
        };
        _appDbContext.SoOrders.Add(soOrder);
        _appDbContext.SaveChanges();
    }

    public void EditOrder(long id, SoOrderRequestEditVM soOrderRequestEditVm)
    {
        try
        {
            var soOrder = _appDbContext.SoOrders
                .Include(o => o.SoItems)
                .FirstOrDefault(o => o.SoOrderId == id);
        
            // Update SoOrder
            soOrder.OrderNo = soOrderRequestEditVm.OrderNo;
            soOrder.OrderDate = soOrderRequestEditVm.OrderDate;
            soOrder.ComCustomerId = soOrderRequestEditVm.ComCustomerId;
            soOrder.Address = soOrderRequestEditVm.Address;
        
            // Hapus item yang ada di DeletedItemIds
            if (soOrderRequestEditVm.DeleteItemIds != null && soOrderRequestEditVm.DeleteItemIds.Count() > 0)
            {
                var itemsToRemove = soOrder.SoItems.Where(i => soOrderRequestEditVm.DeleteItemIds.Contains(i.SoItemId)).ToList();
                _appDbContext.SoItems.RemoveRange(itemsToRemove);
            }
        
            // Tambah atau update item
            // var updatedItems = JsonConvert.DeserializeObject<List<SoItemViewModel>>(soOrderVm.Items);
            foreach (var itemVm in soOrderRequestEditVm.Items)
            {
                var existingItem = soOrder.SoItems.FirstOrDefault(i => i.SoItemId == itemVm.SoItemId);
                if (existingItem != null)
                {
                    // Update item yang ada
                    existingItem.ItemName = itemVm.ItemName;
                    existingItem.Quantity = itemVm.Quantity;
                    existingItem.Price = itemVm.Price;
                }
                else
                {
                    // Tambah item baru
                    soOrder.SoItems.Add(new SoItem
                    {
                        ItemName = itemVm.ItemName,
                        Quantity = itemVm.Quantity,
                        Price = itemVm.Price
                    });
                }
            }

            _appDbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    public void DeleteOrder(long id)
    {
        var order = _appDbContext.SoOrders.Find(id);
        if (order != null)
        {
            _appDbContext.Remove(order);
            _appDbContext.SaveChanges();
        }
    }
}