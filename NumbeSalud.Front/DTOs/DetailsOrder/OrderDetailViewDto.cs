namespace NumbeSalud.Front.DTOs.DetailsOrder;

public class OrderDetailViewDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Subtotal => UnitPrice * Quantity;
    
}