namespace AgendaPro.Domain.Shared;

public static partial class AvailableBlockErrors
{
    public static Error StartDateBeforeEndDate => new("STARTDATE_BEFORE_ENDDATE", "Start date can't be before end date.");
    public static Error StartDateInThePast => new("STARTDATE_IN_PAST", "Start date can't be in the past.");
    public static Error LessThanOneMinute => new("LESS_THAN_MINUTE", "Schedule can't have less than a minute.");
    public static Error InvalidCustomer => new("CUSTOMER_INVALID", "Customer is invalid.");
    public static Error CustomerNotAvailable => new("CUSTOMER_NOT_AVAILABLE", "Customer is not available.");
}