using System.Net.Http.Json;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Orders;

namespace NumbeSalud.Front.Components.Pages;

public partial class HomeSalesByMonth : Components
{
    private List<OrderMonthResponse> sales = [];

    private bool loading = true;
    private bool chartRendered;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            sales = await _client
                        .GetFromJsonAsync<List<OrderMonthResponse>>(
                            "Order/sales-by-month")
                    ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Error cargando ventas por mes: {ex.Message}");
        }
        finally
        {
            loading = false;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!loading && sales.Count > 0 && !chartRendered)
        {
            chartRendered = true;

            var labels = sales
                .Select(x => x.MonthName)
                .ToArray();

            var values = sales
                .Select(x => x.Total)
                .ToArray();

            await _js.InvokeVoidAsync(
                "crearGraficaVentasMensuales",
                labels,
                values
            );
        }
    }
}