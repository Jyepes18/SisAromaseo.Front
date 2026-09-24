using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace NumbeSalud.Front.Components.Pages;

public partial class AuthGuard : Components
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool _validado;
    private bool _autorizado;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        var token = await _js.InvokeAsync<string?>(
            "auth.getCookie"
        );

        if (string.IsNullOrWhiteSpace(token))
        {
            _autorizado = false;
            _validado = true;

            _navigation.NavigateTo(
                "/");

            return;
        }

        _session.Token = token;

        AplicarToken();

        var response = await _client.GetAsync(
            "Login/status"
        );

        if (response.StatusCode == HttpStatusCode.Unauthorized ||
            response.StatusCode == HttpStatusCode.Forbidden)
        {
            await CerrarSesion();
            return;
        }

        _autorizado = response.IsSuccessStatusCode;
        _validado = true;

        await InvokeAsync(StateHasChanged);
    }
}