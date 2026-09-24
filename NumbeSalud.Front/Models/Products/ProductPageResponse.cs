namespace NumbeSalud.Front.Models.Products;

public class ProductPageResponse
{
    public IEnumerable<ProductResponseDto> Data { get; set; } = [];
    public int Total { get; set; }
}