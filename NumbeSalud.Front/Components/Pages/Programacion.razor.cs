using System.Globalization;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumbeSalud.Front.Models;
using NumbeSalud.Front.Models.Orders;
using NumbeSalud.Front.Models.Users;

namespace NumbeSalud.Front.Components.Pages;

public partial class Programacion : Components
{
    private Grid<OrderResponse> OrderGrid = default!;
    private VerProgramacion abrirVerProgramacion = default!;


    private async Task<GridDataProviderResult<OrderResponse>> OrderData(
        GridDataProviderRequest<OrderResponse> request)
    {
        var url =
            $"Order/gets" +
            $"?Page={request.PageNumber}" +
            $"&PageSize={request.PageSize}";

        if (request.Filters is not null)
        {
            foreach (var filter in request.Filters)
            {
                if (filter.PropertyName == "FullName")
                {
                    var fullName = filter.Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(fullName))
                    {
                        url += $"&UserName={Uri.EscapeDataString(fullName)}";
                    }
                }

                if (filter.PropertyName == "Status")
                {
                    var status = filter.Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        status = status.Trim();

                        if (status.Equals("Programado", StringComparison.OrdinalIgnoreCase))
                        {
                            status = "PROGRAMMING";
                        }
                        else if (status.Equals("Cancelado", StringComparison.OrdinalIgnoreCase))
                        {
                            status = "CANCELLED";
                        }

                        url += $"&Status={Uri.EscapeDataString(status)}";
                    }
                }
            }
        }

        var result = await _client.GetFromJsonAsync<OrderPageResponse>(url);

        return new GridDataProviderResult<OrderResponse>
        {
            Data = result?.Data ?? [],
            TotalCount = result?.Total ?? 0
        };
    }

    private async Task RecargarProgramacion()
    {
        await OrderGrid.RefreshDataAsync();
    }

    private async Task VerProgrmacion(int id)
    {
        await abrirVerProgramacion.Abrir(id);
    }

    private async Task EliminarProgramacion(int id)
    {
        try
        {
            var confirmar = await _js.InvokeAsync<bool>(
                "confirmarEliminacion"
            );

            if (!confirmar)
                return;

            var response = await _client.DeleteAsync($"Order/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                await _js.InvokeVoidAsync(
                    "Swal.fire",
                    "¡Eliminado!",
                    "La programación fue eliminada correctamente.",
                    "success"
                );

                await RecargarProgramacion();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                await _js.InvokeVoidAsync(
                    "Swal.fire",
                    "Error",
                    string.IsNullOrWhiteSpace(error)
                        ? "No se pudo eliminar la programación."
                        : error,
                    "error"
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            await _js.InvokeVoidAsync(
                "Swal.fire",
                "Error",
                "Ocurrió un error al eliminar la programación.",
                "error"
            );
        }
    }

    private async Task Faturar(int id)
    {
        try
        {
            string url = $"Invoice/create/{id}";

            var response = await _client.PostAsJsonAsync(url, new { });
            var result = await response.Content.ReadFromJsonAsync<Result<string>>();

            if (result.IsSuccess)
            {
                await _js.InvokeVoidAsync(
                    "Swal.fire",
                    "¡Factura generada!",
                    result.Value ?? "La factura fue generada correctamente.",
                    "success"
                );

                await RecargarProgramacion();
            }
            else
            {
                await _js.InvokeVoidAsync(
                    "Swal.fire",
                    "Error",
                    result?.Error ?? "No se pudo generar la factura.",
                    "error"
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            await _js.InvokeVoidAsync(
                "Swal.fire",
                "Error",
                "Ocurrió un error al generar la factura.",
                "error"
            );
        }
    }

    private async Task DescargarFactura(int id)
    {
        try
        {
            var result = await _client.GetFromJsonAsync<Result<string>>(
                $"Invoice/get/{id}");

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