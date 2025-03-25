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
    public static Error CnpjAlreadyUsed => new Error("CNPJ_ALREADY_USED", "Cnpj is already used.", "cnpj");
    public static Error PhoneAlreadyUsed => new Error("PHONE_ALREADY_USED", "Phone number is already used.", "phone");
    public static Error EmailAlreadyUsed => new Error("EMAIL_ALREADY_USED", "Email is already used.", "email");
    public static Error CustomerDoesNotExist => new Error("CUSTOMER_NOT_FOUND", "Customer does not exist.");
}