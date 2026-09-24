namespace NumbeSalud.Front.Models;

public class Result<TValue>
{
    public TValue Value { get; set; }
    public string Error { get; set; }
    public bool IsSuccess { get; set; }
    public int Status { get; set; }
}