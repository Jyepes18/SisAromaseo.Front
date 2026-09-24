namespace NumbeSalud.Front.Models.Orders;

public class OrderPageResponse
{
    public IEnumerable<OrderResponse> Data { get; set; } = [];
    public int Total { get; set; }
}