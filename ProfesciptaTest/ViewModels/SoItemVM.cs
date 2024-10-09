namespace ProfesciptaTest.ViewModels;

public class SoItemResponseVM
{
    public long SoItemId { get; set; }

    public long SoOrderId { get; set; }

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public double Price { get; set; }

    public decimal Total => (decimal)(Quantity * Price);
}

public class SoItemRequestVM
{
    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public double Price { get; set; }
}

public class SoItemRequestEditVM
{
    public long? SoItemId { get; set; }
    
    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public double Price { get; set; }
}