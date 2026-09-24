using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Login;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class Login : Components
{
    private LoginDto LoginDto = new();
    
    private async Task LoginOauth()
    {
        string url = "Login/oauth";

        var response = await _client.PostAsJsonAsync(url, LoginDto);

        var content = await response.Content
            .ReadFromJsonAsync<Result<string>>();

        if (content is null)
            return;

        if (!content.IsSuccess)
        {
            await _js.InvokeVoidAsync(
                "Swal.fire",
                ":(",
                content.Error,
                "error"
            );

            return;
        }

        string token = content.Value;

        // IMPORTANTE
        _session.Token = token;

        await _js.InvokeVoidAsync(
            "eval",
            $"document.cookie = 'token={token}; path=/; max-age=86400; SameSite=Lax'"
        );

        AplicarToken();

        Console.WriteLine($"TOKEN SESSION: {_session.Token.Length}");
        Console.WriteLine($"AUTH LOGIN: {_client.DefaultRequestHeaders.Authorization}");

        _navigation.NavigateTo("/Home");
    }
    
} 