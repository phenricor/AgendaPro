using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Shared;
using AgendaPro.Web.Models.Customer;

namespace AgendaPro.Web.Services.Interfaces;

public interface IScheduler
{
    public List<CustomerForScheduleViewModel> ReturnViewModelFromEntityList(List<Customer> customers);
    public Task<Result<AvailableBlock>> ScheduleIfAvailable(Customer customer, DateTime date, TimeSpan startTime, TimeSpan endTime);
}