using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Product;
using NumbeSalud.Front.DTOs.User;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class InsertarProducto : Components
{
    [Parameter]  public EventCallback ProductoCreado { get; set; }
    
    
    private ProductoDto newProducto = new();
    private async Task InsertProduct()
    {
        try
        {
            const string url = "Product/Create";

            var response = await _client.PostAsJsonAsync(url, newProducto);
            var content = await response.Content.ReadFromJsonAsync<Result<string>>();

            if (content.IsSuccess)
            {
                await _js.InvokeVoidAsync("Swal.fire", ":)", content.Value, "success");
                newProducto = new ProductoDto();
                await _js.InvokeVoidAsync("cerrarModal", "nuevoUsuarioModal");
                await ProductoCreado.InvokeAsync();
            }
        }
        catch (Exception e)
        {
            _navigation.NavigateTo("/Error");
        }
    }
    
}