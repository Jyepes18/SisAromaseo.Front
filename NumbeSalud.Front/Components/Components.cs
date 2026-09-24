using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Login;

namespace NumbeSalud.Front.Components;

public class Components : ComponentBase, IAsyncDisposable
{
    [Inject]
    protected HttpClient _client { get; set; } = default!;

    [Inject]
    protected NavigationManager _navigation { get; set; } = default!;

    [Inject]
    protected IJSRuntime _js { get; set; } = default!;

    [Inject]
    protected TokenRequest _session { get; set; } = default!;


    private PeriodicTimer? _timer;
    private CancellationTokenSource? _cts;


    protected void IniciarValidacionToken()
    {
        if (_timer is not null)
            return;

        _cts = new CancellationTokenSource();

        _timer = new PeriodicTimer(
            TimeSpan.FromMinutes(5)
        );

        _ = EjecutarTimerAsync();
    }


    private async Task EjecutarTimerAsync()
    {
        if (_timer is null || _cts is null)
            return;

        try
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                await ValidarToken();
            }
        }
        catch (OperationCanceledException)
        {
        }
    }


    protected async Task ValidarToken()
    {
        try
        {
            AplicarToken();

            var response = await _client.GetAsync(
                "Login/status"
            );

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                await CerrarSesion();
            }
        }
        catch (HttpRequestException)
        {
            _navigation.NavigateTo("/Error");
        }
    }


    protected async Task CerrarSesion()
    {
        await _js.InvokeVoidAsync(
            "auth.removeCookie"
        );

        _session.Token = string.Empty;

        _client.DefaultRequestHeaders.Authorization = null;

        DetenerValidacionToken();

        _navigation.NavigateTo(
            "/",
            forceLoad: true
        );
    }


    protected void AplicarToken()
    {
        if (string.IsNullOrWhiteSpace(_session.Token))
            return;

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _session.Token
            );
    }


    protected void DetenerValidacionToken()
    {
        if (_cts is not null &&
            !_cts.IsCancellationRequested)
        {
            _cts.Cancel();
        }

        _timer?.Dispose();

        _timer = null;
    }


    public ValueTask DisposeAsync()
    {
        DetenerValidacionToken();

        _cts?.Dispose();

        return ValueTask.CompletedTask;
    }
    
}