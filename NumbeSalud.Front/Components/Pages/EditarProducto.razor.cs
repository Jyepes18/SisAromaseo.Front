using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Product;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class EditarProducto : Components
{
    [Parameter]
    public EventCallback ProductoEditado { get; set; }
    private ProductoDto productoDto = new();
    private int productoId;
    
    public async Task Abrir(int id)
    {
        productoId = id;

        var response = await _client.GetAsync($"Product/{id}");
        var result = await response.Content .ReadFromJsonAsync<Result<ProductoDto>>();
        
        productoDto = result.Value;

        await _js.InvokeVoidAsync("abrirModal", "editarProductoModal");
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task ActualizarProducto(EditContext context)
    {
        try
        {
            var url = $"Product/update/{productoId}";

            var response = await _client.PutAsJsonAsync(url, productoDto);
            var result = await response.Content.ReadFromJsonAsync<Result<string>>();
            
            await _js.InvokeVoidAsync("cerrarModal", "editarProductoModal");
            await _js.InvokeVoidAsync("Swal.fire", ":)", result.Value, "success" );
            await ProductoEditado.InvokeAsync();
        }
        catch (Exception ex)
        {
            _navigation.NavigateTo("/Error");
        }
    }
}