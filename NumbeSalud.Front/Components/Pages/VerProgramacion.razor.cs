using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.Orders;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class VerProgramacion : Components
{
    [Parameter]
    public EventCallback ProgramcionAbrir { get; set; }
    
    private int oderId;
    private OrderCompleteDto viewOrder = new();
    public async Task Abrir(int id)
    {
        oderId = id;

        var response = await _client.GetAsync($"Order/get/{id}");
        var result = await response.Content.ReadFromJsonAsync<Result<OrderCompleteDto>>();
        viewOrder = result.Value;
        
        await _js.InvokeVoidAsync("abrirModal", "verProgramacionModal");
        await InvokeAsync(StateHasChanged);
    }
}