using System.Text.RegularExpressions;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.ValueObjects;

public class Phone
{
    private string _phoneNumber;

    private static string Pattern =>
        @"^\(?(?:[14689][1-9]|2[12478]|3[1234578]|5[1345]|7[134579])\)? ?(?:[2-8]|9[0-9])[0-9]{3}\-?[0-9]{4}$";

    private Phone(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public static Result<Phone> Create(string? phoneNumber)
    {
        if (phoneNumber == null || phoneNumber.Length < 10 || !IsValid(phoneNumber))
        {
            return Result<Phone>.Failure(CustomerErrors.PhoneInvalid);
        }
        return Result<Phone>.Success(new Phone(phoneNumber));
    }

    private static bool IsValid(string phoneNumber)
    {
        var formattedNumber = phoneNumber.Trim();
        var result = Regex.Match(formattedNumber, Pattern).Success;
        return result;
    }
}