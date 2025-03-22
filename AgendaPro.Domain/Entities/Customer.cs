using System.Data;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.ValueObjects;

namespace AgendaPro.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Cnpj { get; set; }
    public string Phone { get; set; }
    protected Customer(){}
    private Customer(string name, string email, string cnpj, string phone)
    {
        Name = name;
        Email = email;
        Cnpj = cnpj;
        Phone = phone;
    }
    public static Result<Customer> Create(string name, string email, string cnpj, string phone)
    {
        var cnpjResult = ValueObjects.Cnpj.Create(cnpj);
        var phoneResult = ValueObjects.Phone.Create(phone);
        if (cnpjResult.IsFailure)
        {
            return Result<Customer>.Failure(cnpjResult.Error);
        }
        if (phoneResult.IsFailure)
        {
            return Result<Customer>.Failure(phoneResult.Error);
        }
        return Result<Customer>.Success(new Customer(name, email, cnpj, phone));
    }
}