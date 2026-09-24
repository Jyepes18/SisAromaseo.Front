using Microsoft.AspNetCore.Components;

namespace NumbeSalud.Front.Components.Pages;

public partial class Home : Components
{
    protected override void OnInitialized()
    {
        IniciarValidacionToken();

        base.OnInitialized();
    }
}