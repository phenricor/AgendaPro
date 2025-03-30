using System.Globalization;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.ValueObjects;

namespace AgendaPro.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public Phone Phone { get; private set; }
    public virtual ICollection<AvailableBlock?> AvailableBlocks { get; set; } = new List<AvailableBlock?>();
    protected Customer(){}
    private Customer(string name, string email, Cnpj cnpj, Phone phone)
    {
        Name = name;
        Email = email;
        Cnpj = cnpj;
        Phone = phone;
    }
    public static Result<Customer> Create(string name, string email, string? cnpj, string? phone)
    {
        var validation = Validate(cnpj, phone);
        if (validation.IsFailure)
        {
            return Result<Customer>.Failure(validation.Error);
        }

        var cnpjObj = Cnpj.Create(cnpj);
        var phoneObj = Phone.Create(phone);
        if (cnpjObj.IsFailure || phoneObj.IsFailure)
        {
            return Result<Customer>.Failure(CustomerErrors.CnpjInvalid);
        }
        var customer = new Customer()
        {
            Name = FormatName(name),
            Email = email,
            Cnpj = cnpjObj.Value,
            Phone = phoneObj.Value
        };
        return Result<Customer>.Success(customer);
    }

    private static Result<bool> Validate(string? cnpj, string? phone)
    {
        var cnpjResult = Cnpj.Validate(cnpj);
        var phoneResult = Phone.Validate(phone);
        if (cnpjResult.IsFailure)
        {
            return Result<bool>.Failure(cnpjResult.Error);
        }
        if (phoneResult.IsFailure)
        {
            return Result<bool>.Failure(phoneResult.Error);
        }
        return Result<bool>.Success(true);
    }

    private static string FormatName(string name)
    {
        var textInfo = new CultureInfo("pt-BR", false).TextInfo;
        name = textInfo
            .ToTitleCase(name)
            .Trim();
        return name;
    }

    public Result<Customer> UpdateProperties(string name, string email, string cnpj, string phone)
    {
        var validation = Validate(cnpj, phone);
        if (validation.IsFailure)
        {
            return Result<Customer>.Failure(validation.Error);
        }
        Name = FormatName(name);
        Email = email;
        Cnpj = Cnpj.Create(cnpj).Value;
        Phone = Phone.Create(phone).Value;
        return Result<Customer>.Success(this);
    }

    #region Availability

    public Result<AvailableBlock> AddAvailableBlock(DateTime startDate, DateTime endDate)
    {
        if (startDate == endDate)
        {
            return Result<AvailableBlock>.Failure(AvailableBlockErrors.LessThanOneMinute);
        }
        if (startDate < DateTime.Now)
        {
            return Result<AvailableBlock>.Failure(AvailableBlockErrors.StartDateInThePast);
        }
        if (!IsAvailableAt(startDate, endDate))
        {
            return Result<AvailableBlock>.Failure(AvailableBlockErrors.CustomerNotAvailable);
        }
        var availability = AvailableBlock.Create(startDate, endDate, Id);
        if (availability.IsFailure)
        {
            return Result<AvailableBlock>.Failure(availability.Error);
        }
        AvailableBlocks.Add(availability.Value);
        return Result<AvailableBlock>.Success(availability.Value);
    }

    private bool IsAvailableAt(DateTime startDate, DateTime endDate)
    {
        if (!AvailableBlocks.Any())
        {
            return true;
        }
        var isBlocked = AvailableBlocks
            .Any(x => x != null && startDate < x.EndDate && endDate > x.StartDate);
        return !isBlocked;
    }

    #endregion
}