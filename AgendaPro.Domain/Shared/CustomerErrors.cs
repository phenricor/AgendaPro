namespace AgendaPro.Domain.Shared;

public static partial class CustomerErrors
{
    public static Error CnpjInvalid => new("CNPJ_INVALID", "Cnpj is invalid.", "cnpj");
    public static Error PhoneInvalid => new("PHONE_INVALID", "Phone number is invalid.", "phone");
    public static Error CnpjAlreadyUsed => new Error("CNPJ_ALREADY_USED", "Cnpj is already used.", "cnpj");
    public static Error PhoneAlreadyUsed => new Error("PHONE_ALREADY_USED", "Phone number is already used.", "phone");
    public static Error EmailAlreadyUsed => new Error("EMAIL_ALREADY_USED", "Email is already used.", "email");
    public static Error CustomerDoesNotExist => new Error("CUSTOMER_NOT_FOUND", "Customer does not exist.");
}