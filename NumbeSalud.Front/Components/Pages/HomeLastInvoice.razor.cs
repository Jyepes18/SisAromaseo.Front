using System.Globalization;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Invoice;
using NumbeSalud.Front.Models;
using NumbeSalud.Front.Models.Invoice;
using NumbeSalud.Front.Models.Users;

namespace NumbeSalud.Front.Components.Pages;

public partial class HomeLastInvoice : Components
{
    private async Task<GridDataProviderResult<InvoiceResponseDto>> LastInvoiceData(GridDataProviderRequest<InvoiceResponseDto> request)
    {
        var url = 
            $"Invoice/get/last-invoices" +
            $"?Page={request.PageNumber}" +
            $"&PageSize={request.PageSize}";

        if (request.Filters is not null)
        {
            foreach (var filter in request.Filters)
            {
                if (filter.PropertyName == nameof(InvoiceResponseDto.UserName))
                {
                    var name = filter.Value?.ToString()?.Trim();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        url += $"&Name={Uri.EscapeDataString(name)}";
                    }
                }
            }
        }
        
        var result = await _client.GetFromJsonAsync<InvoicePageResponse>(url);

        return new GridDataProviderResult<InvoiceResponseDto>
        {
            Data = result?.Data ?? [],
            TotalCount = result?.Total ?? 0
        };
    }
    
    private async Task DescargarFactura(int id)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<Result<string>>(
                $"Invoice/get/lasta-invoice/{id}");

            if (result is null || !result.IsSuccess || string.IsNullOrWhiteSpace(result.Value))
            {
                await _js.InvokeVoidAsync(
                    "Swal.fire",
                    "Error",
                    result?.Error ?? "No se encontró la factura.",
                    "error");

                return;
            }

            await _js.InvokeVoidAsync(
                "descargarPdfBase64",
                result.Value,
                $"Factura-{DateTime.UtcNow.ToString("dddd dd-MM-yyyy", new CultureInfo("es-Co"))}.pdf");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            await _js.InvokeVoidAsync(
                "Swal.fire",
                "Error",
                "Ocurrió un error al obtener la factura.",
                "error");
        }
    }
}