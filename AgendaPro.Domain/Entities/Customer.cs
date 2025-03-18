namespace AgendaPro.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cnpj { get; set; }
    public string Phone { get; set; }
}