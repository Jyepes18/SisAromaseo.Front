using System.Globalization;
using NumbeSalud.Front.Enums;

public class OrderResponse
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string DateFormatted =>
        Date.ToString("dddd dd/MM/yyyy", new CultureInfo("es-CO"));

    public string Status { get; set; }
    
    public string FullName { get; set; }

    public string DateClass
    {
        get
        {   
            if (Status == StatusOrder.CANCELLED.ToString())
                return "badge text-bg-secondary";
            
            var today = DateTime.Today;
            var date = Date.Date;

            var daysDifference = (date - today).Days;

            if (daysDifference <= 0)
                return "badge text-bg-danger";

            if (daysDifference <= 3)
                return "badge text-bg-warning";

            return "badge text-bg-success";
        }
    }

}