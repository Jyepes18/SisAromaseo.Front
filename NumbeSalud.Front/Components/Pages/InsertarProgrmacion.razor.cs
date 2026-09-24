using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NumbeSalud.Front.DTOs.DetailsOrder;
using NumbeSalud.Front.DTOs.Orders;
using NumbeSalud.Front.DTOs.Product;
using NumbeSalud.Front.DTOs.User;
using NumbeSalud.Front.Models;

namespace NumbeSalud.Front.Components.Pages;

public partial class InsertarProgrmacion : Components
{
    [Parameter] public EventCallback OrderCreado { get; set; }
    
    private string? clienteName;
    private int? clienteId;
    
    private async Task<AutoCompleteDataProviderResult<UserList>> UserDataProvider(
        AutoCompleteDataProviderRequest<UserList> request)
    {
        var search = request.Filter?.Value?.ToString() ?? string.Empty;
        
        var users = await _client.GetFromJsonAsync<Result<IEnumerable<UserList>>>(
            $"User/search?search={Uri.EscapeDataString(search)}");

        users.Value ??= [];

        return new AutoCompleteDataProviderResult<UserList>
        {
            Data = users.Value,
            TotalCount = users.Value.Count()
        };
    }
    private void OnAutoCompleteChanged(UserList userList)
    {
        if (userList == null)
        {
            clienteId = null;
            return;
        }
        clienteId = userList.Id;
    }
    
    private string? productoName;

    private ProductList? selectedProduct;

    private int selectedQuantity = 1;
    

    private async Task<AutoCompleteDataProviderResult<ProductList>>
        ProductDataProvider(
            AutoCompleteDataProviderRequest<ProductList> request)
    {
        var search = request.Filter?.Value?.ToString() ?? "";

        var products = await _client.GetFromJsonAsync<Result<IEnumerable<ProductList>>>(
            $"Product/search?search={Uri.EscapeDataString(search)}",
            request.CancellationToken);

        products?.Value ??= [];

        return new AutoCompleteDataProviderResult<ProductList>
        {
            Data = products?.Value,
            TotalCount = products?.Value.Count()
        };
    }

    private void OnProductChanged(ProductList? product)
    {
        if (product is null)
            return;

        selectedProduct = product;

        var existing = orderDetails.FirstOrDefault(
            x => x.ProductId == product.Id);

        if (existing is not null)
        {
            existing.Quantity++;
        }
        else
        {
            orderDetails.Add(new OrderDetailViewDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = 1
            });
        }

        productoName = null;
    }
    
    private List<OrderDetailViewDto> orderDetails = [];

    private void AddProduct()
    {
        if (selectedProduct is null)
            return;

        if (selectedQuantity < 1)
            selectedQuantity = 1;

        var existing = orderDetails.FirstOrDefault(
            x => x.ProductId == selectedProduct.Id);

        if (existing is not null)
        {
            existing.Quantity += selectedQuantity;
        }
        else
        {
            orderDetails.Add(new OrderDetailViewDto
            {
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.Name,
                UnitPrice = selectedProduct.Price,
                Quantity = selectedQuantity
            });
        }

        selectedProduct = null;
        productoName = null;
        selectedQuantity = 1;
    }

    private decimal TotalOrder =>
        orderDetails.Sum(x => x.Subtotal);

    private void RemoveProduct(OrderDetailViewDto detail)
    {
        orderDetails.Remove(detail);
    }

    private OrderDto newOrder = new()
    {
        Date = DateTime.Today
    };

    public async Task InsertOrder()
    {
        if (clienteId is null)
        {
            await _js.InvokeVoidAsync("Swal.fire", "Error", "Debe seleccionar un cliente.", "error");
            return;
        }

        if (orderDetails.Count == 0)
        {
            await _js.InvokeVoidAsync("Swal.fire", "Error", "Debe agregar al menos un producto.", "error");
            return;
        }

        var request = new
        {
            userId = clienteId.Value,
            date = newOrder.Date,
            detailOrders = orderDetails.Select(x => new
            {
                productId = x.ProductId,
                quantity = x.Quantity
            }).ToList()
        };

        var response = await _client.PostAsJsonAsync(
            "Order/create",
            request);

        var result = await response.Content
            .ReadFromJsonAsync<Result<string>>();

        if (result is null || !result.IsSuccess)
        {
            await _js.InvokeVoidAsync(
                "Swal.fire",
                "Error",
                result?.Error ?? "No se pudo crear la orden.",
                "error");

            return;
        }

        await _js.InvokeVoidAsync(
            "Swal.fire",
            ":)",
            $"{result.Value.Trim()}",
            "success");
        
        clienteId = null;
        clienteName = null;

        selectedProduct = null;
        productoName = null;
        selectedQuantity = 1;

        orderDetails.Clear();

        newOrder = new OrderDto
        {
            Date = DateTime.Today
        };
        
        await _js.InvokeVoidAsync("cerrarModal", "nuevaProgramacionModal");
        await OrderCreado.InvokeAsync();
    }
}