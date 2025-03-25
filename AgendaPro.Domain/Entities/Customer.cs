using System.Globalization;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.ValueObjects;

namespace AgendaPro.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public Phone Phone { get; private set; }
    protected Customer(){}
    private Customer(string name, string email, Cnpj cnpj, Phone phone)
    {
        Name = name;
        Email = email;
        Cnpj = cnpj;
        Phone = phone;
    }
    public static Result<Customer> Create(string name, string email, string cnpj, string phone)
    {
        var validation = Validate(cnpj, phone);
        if (validation.IsFailure)
        {
            return Result<Customer>.Failure(validation.Error);
        }
        if (validation.Value == null)
        {
            return Result<Customer>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var customer = validation.Value;
        customer.Name = FormatName(name);
        customer.Email = email;
        return Result<Customer>.Success(customer);
    }

    public static Result<Customer> Validate(string cnpj, string phone)
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
        var customer = new Customer()
        {
            Cnpj = cnpjResult.Value, 
            Phone = phoneResult.Value
        };
        return Result<Customer>.Success(customer);
    }

    private static string FormatName(string name)
    {
        var textInfo = new CultureInfo("pt-BR", false).TextInfo;
        name = textInfo
            .ToTitleCase(name)
            .Trim();
        return name;
    }

    public Result<Customer> Update(string name, string email, string cnpj, string phone)
    {
        var validation = Validate(cnpj, phone);
        if (validation.IsFailure)
        {
            return Result<Customer>.Failure(validation.Error);
        }
        Name = FormatName(name);
        Email = email;
        Cnpj = validation.Value.Cnpj;
        Phone = validation.Value.Phone;
        return Result<Customer>.Success(this);
    }
}