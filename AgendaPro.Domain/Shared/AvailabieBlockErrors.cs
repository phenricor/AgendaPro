namespace AgendaPro.Domain.Shared;

public static partial class AvailableBlockErrors
{
    public static Error TimeSpanIsInvalid => new("TIMESPAN_INVALID", "Start date can't be before end date.");
    public static Error InvalidCustomer => new("CUSTOMER_INVALID", "Customer is invalid.");
    public static Error GenericError => new("GENERIC_ERROR", "Something went wrong.");
}