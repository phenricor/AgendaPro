namespace AgendaPro.Web.Models.Customer;

public class CustomerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cnpj { get; set; }
    public string Phone { get; set; }

    public CustomerModel FromModel(Domain.Entities.Customer customer)
    {
        var customerModel = new CustomerModel()
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Cnpj = customer.Cnpj.Value,
            Phone = customer.Phone.Value
        };
        return customerModel;
    }
}