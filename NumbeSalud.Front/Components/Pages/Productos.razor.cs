using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using NumbeSalud.Front.Models.Products;

namespace NumbeSalud.Front.Components.Pages;

public partial class Productos : Components
{
    private EditarProducto editarProducto = default!;
    
    private Grid<ProductResponseDto> productGrid = default!;
    private async Task<GridDataProviderResult<ProductResponseDto>> ProductData(
        GridDataProviderRequest<ProductResponseDto> request)
    {
        var url =  $"Product/Gets" +
                   $"?Page={request.PageNumber}" +
                   $"&PageSize={request.PageSize}";
        
        if (request?.Filters is not null)
        {
            foreach (var filter in request.Filters)
            {
                if (filter.PropertyName == "Name")
                {
                    var name = filter.Value?.ToString();

                    if (!string.IsNullOrEmpty(name))
                    {
                        url += $"&Name={Uri.EscapeDataString(name)}";
                    }
                }

                if (filter.PropertyName == "Presentation")
                {
                    var presentation = filter.Value?.ToString();

                    if (!string.IsNullOrEmpty(presentation))
                    {
                        url += $"&Presentation={Uri.EscapeDataString(presentation)}";
                    }
                }
            }
        }

        var result = await _client.GetFromJsonAsync<ProductPageResponse>(url);
        
        return new GridDataProviderResult<ProductResponseDto>
        {
            Data = result?.Data ?? [],
            TotalCount = result?.Total ?? 0
        };
    }

    public async Task RecargarProductos()
    {
        await productGrid.RefreshDataAsync();
    }
    
    private async Task EditarProducto(int id)
    {
        await editarProducto.Abrir(id);
    }
}