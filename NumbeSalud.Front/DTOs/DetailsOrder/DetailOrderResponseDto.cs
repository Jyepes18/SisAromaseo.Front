namespace NumbeSalud.Front.DTOs.DetailsOrder;

public class DetailOrderResponseDto
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal { get; set; }
}