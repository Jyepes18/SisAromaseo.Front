using NumbeSalud.Front.DTOs.DetailsOrder;

namespace NumbeSalud.Front.DTOs.Orders;

public class OrderCompleteDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Status { get; set; }
    public DateTime Date { get; set; }
    public decimal Total { get; set;}
    public List<DetailOrderResponseDto> DetailOrderResponseDtos { get; set; }
}