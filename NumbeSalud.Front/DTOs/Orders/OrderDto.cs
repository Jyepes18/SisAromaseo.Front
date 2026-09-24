using System.ComponentModel.DataAnnotations;
using NumbeSalud.Front.DTOs.DetailsOrder;

namespace NumbeSalud.Front.DTOs.Orders;

public class OrderDto
{
    [Required(ErrorMessage = "El campo Usuario es obligatorio")]
    public int UserId { get; set; }
    [Required(ErrorMessage = "El campo Detalle es obligatorio")]
    public DateTime Date { get; set; }
    public List<DetailOrderDto> DetailOrders { get; set; }
}