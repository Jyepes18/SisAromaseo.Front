using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using NumbeSalud.Front.Models.Users;
using System.Net.Http.Json;

namespace NumbeSalud.Front.Components.Pages;

public partial class Usuario : Components
{
    private EditarUsuario editarUsuario = default!;
    
    private Grid<UserResponseDto> userGrid = default!;

    private async Task<GridDataProviderResult<UserResponseDto>> UserData(GridDataProviderRequest<UserResponseDto> request)
    {
        var url = 
            $"User/gets" +
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

                if (filter.PropertyName == "Company")
                {
                    var company = filter.Value?.ToString();

                    if (!string.IsNullOrEmpty(company))
                    {
                        url += $"&Company={Uri.EscapeDataString(company)}";
                    }
                }
            }
        }
        
        
        var result = await _client.GetFromJsonAsync<UserPageResponse>(url);

        return new GridDataProviderResult<UserResponseDto>
        {
            Data = result?.Data ?? [],
            TotalCount = result?.Total ?? 0
        };
    }
    
    private async Task RecargarUsuarios()
    {
        await userGrid.RefreshDataAsync();
    }
    
    private async Task EditarUsuario(int id)
    {
        await editarUsuario.Abrir(id);
    }
    
}