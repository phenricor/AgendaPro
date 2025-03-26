using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.ValueObjects;

[ComplexType]
public record Phone
{
    public string? Value { get; private set; }
    private static string Pattern =>
        @"^\(?(?:[14689][1-9]|2[12478]|3[1234578]|5[1345]|7[134579])\)? ?(?:[2-8]|9[0-9])[0-9]{3}\-?[0-9]{4}$";
    protected Phone(){}
    private Phone(string? value)
    {
        Value = value;
    }

    public static Result<Phone> Create(string? phoneNumber)
    {
        if (phoneNumber == null)
        {
            return Result<Phone>.Failure(CustomerErrors.PhoneInvalid);
        }
        var formattedNumber = phoneNumber.Trim();
        if (Validate(formattedNumber).IsFailure)
        {
            return Result<Phone>.Failure(CustomerErrors.PhoneInvalid);
        }
        return Result<Phone>.Success(new Phone(phoneNumber));
    }
    public static Result<bool> Validate(string phoneNumber)
    {
        var result = Regex.Match(phoneNumber, Pattern).Success;
        if (phoneNumber == null || phoneNumber.Length < 10 || !result)
        {
            return Result<bool>.Failure(CustomerErrors.PhoneInvalid);
        }
        return Result<bool>.Success(true);
    }
    public override string? ToString() => Value;
}