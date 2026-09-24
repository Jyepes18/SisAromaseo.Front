using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.User;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class InsertarUsuario : Components
{
    [Parameter]
    public EventCallback UsuarioCreado { get; set; }
    
    private UserDto newUserDto = new();
    private async Task InsertarDatos(EditContext context)
    {
        try
        {
            var url = "User/Create";
            
            var response = await _client.PostAsJsonAsync(url, newUserDto);
            var content = await response.Content.ReadFromJsonAsync<Result<string>>();

            if (content.IsSuccess)
            {
                await _js.InvokeVoidAsync("Swal.fire", ":)", content.Value, "success");
                newUserDto = new UserDto();
                await _js.InvokeVoidAsync("cerrarModal", "nuevoUsuarioModal");
                await UsuarioCreado.InvokeAsync();
            }
        }
        catch (Exception ex)
        {
            _navigation.NavigateTo("/Error");
        }
    }
}