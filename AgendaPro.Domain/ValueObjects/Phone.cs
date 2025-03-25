using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.ValueObjects;

[ComplexType]
public record Phone
{
    public string Value { get; private set; }
    private static string Pattern =>
        @"^\(?(?:[14689][1-9]|2[12478]|3[1234578]|5[1345]|7[134579])\)? ?(?:[2-8]|9[0-9])[0-9]{3}\-?[0-9]{4}$";
    protected Phone(){}
    private Phone(string value)
    {
        Value = value;
    }

    public static Result<Phone> Create(string? phoneNumber)
    {
        if (phoneNumber == null || phoneNumber.Length < 10 || !Validate(phoneNumber))
        {
            return Result<Phone>.Failure(CustomerErrors.PhoneInvalid);
        }
        return Result<Phone>.Success(new (phoneNumber));
    }
    private static bool Validate(string phoneNumber)
    {
        var formattedNumber = phoneNumber.Trim();
        var result = Regex.Match(formattedNumber, Pattern).Success;
        return result;
    }
    public override string ToString() => Value;
}