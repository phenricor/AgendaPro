namespace AgendaPro.Domain.Entities;

public class Availability
{
    public Guid Id { get; set; }
    public DateTime DateTIme { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}