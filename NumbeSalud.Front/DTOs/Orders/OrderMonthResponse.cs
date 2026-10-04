namespace NumbeSalud.Front.DTOs.Orders;

public class OrderMonthResponse
{
    public int Year { get; set; }

    public int Month { get; set; }

    public string MonthName { get; set; } = string.Empty;

    public decimal Total { get; set; }
}