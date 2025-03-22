namespace AgendaPro.Domain.Shared;

public record Error
{
    public string Code { get; init; }
    public string Description { get; init; }
    public string? FieldValidation { get; init; }

    public Error(string code, string description, string? fieldValidation = null)
    {
        Code = code;
        Description = description;
        // Helps ajax find the field in the view that will be validated
        if (fieldValidation != null)
        {
            FieldValidation = fieldValidation;
        }
    }
    public static Error None => new(string.Empty, string.Empty, string.Empty);
}

public static class CustomerErrors
{
    public static Error CnpjInvalid => new("CNPJ_INVALID", "Cnpj is invalid.", "cnpj");
    public static Error PhoneInvalid => new("PHONE_INVALID", "Phone number is invalid.", "phone");
}