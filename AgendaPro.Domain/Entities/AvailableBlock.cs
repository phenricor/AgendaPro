using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Domain.Entities;

public class AvailableBlock
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid CustomerId { get; set; }
    [JsonIgnore]
    public Customer Customer { get; set; }
    protected AvailableBlock() { }

    private AvailableBlock(DateTime startDate, DateTime endDate, Guid customerId)
    {
        StartDate = startDate;
        EndDate = endDate;
        CustomerId = customerId;
    }
    public static Result<AvailableBlock> Create(DateTime startDate, DateTime endDate, Guid customerId)
    {
        if (startDate > endDate)
        {
            return Result<AvailableBlock>.Failure(AvailableBlockErrors.StartDateBeforeEndDate);
        }
        if (customerId == Guid.Empty)
        {
            return Result<AvailableBlock>.Failure(AvailableBlockErrors.InvalidCustomer);
        }
        var block = new AvailableBlock(startDate, endDate, customerId);
        return Result<AvailableBlock>.Success(block);
    }
}