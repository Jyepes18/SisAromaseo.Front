using NumbeSalud.Front.DTOs.Invoice;

namespace NumbeSalud.Front.Models.Invoice;

public class InvoicePageResponse
{
    public IEnumerable<InvoiceResponseDto> Data { get; set; } = [];
    public int Total { get; set; }
}