using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.User;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class EditarUsuario : Components
{
    [Parameter]
    public EventCallback UsuarioEditado { get; set; }

    private UserDto userDto = new();

    private int userId;
    
    public async Task Abrir(int id)
    {
        userId = id;

        var response = await _client.GetAsync($"User/{id}");
        var result = await response.Content.ReadFromJsonAsync<Result<UserDto>>();
        userDto = result.Value;
        
        await _js.InvokeVoidAsync("abrirModal", "editarUsuarioModal");
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task ActualizarUsuario(EditContext context)
    {
        try
        {
            var url = $"User/update/{userId}";

            var response = await _client.PutAsJsonAsync(url, userDto);
            var result = await response.Content.ReadFromJsonAsync<Result<string>>();
            
            await _js.InvokeVoidAsync("cerrarModal", "editarUsuarioModal");
            await _js.InvokeVoidAsync("Swal.fire", ":)", result.Value, "success" );
            await UsuarioEditado.InvokeAsync();
        }
        catch (Exception ex)
        {
            _navigation.NavigateTo("/Error");
        }
    }
    
}