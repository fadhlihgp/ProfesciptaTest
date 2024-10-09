using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProfesciptaTest.ViewModels;

public class SoOrderResponseVM
{
    public long SoOrderId { get; set; }
    public string OrderNo { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public int ComCustomerId { get; set; }
    public string CustomerName { get; set; }
    public string? Address { get; set; } = null!;
    public ICollection<SoItemResponseVM> Items { get; set; }
}

public class SoOrderRequestVM
{
    [Required]
    public string OrderNo { get; set; }

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public int ComCustomerId { get; set; }

    public string? Address { get; set; } = null!;
    
    public IEnumerable<SoItemRequestVM> Items { get; set; }
}

public class SoOrderRequestEditVM
{
    public string OrderNo { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public int ComCustomerId { get; set; }
    public string? Address { get; set; } = null!;
    public List<long> DeleteItemIds { get; set; }
    public ICollection<SoItemRequestEditVM> Items { get; set; }
}