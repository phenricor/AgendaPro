using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Domain.Shared;
using AgendaPro.Web.Models.Customer;
using AgendaPro.Web.Services.Interfaces;

namespace AgendaPro.Web.Services;

public class Scheduler : IScheduler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAvailableBlockRepository _availableBlockRepository;
    public Scheduler(
        ICustomerRepository customerRepository,
        IAvailableBlockRepository availableBlockRepository)
    {
        _customerRepository = customerRepository;
        _availableBlockRepository = availableBlockRepository;
    }

    public List<CustomerForScheduleViewModel> ReturnViewModelFromEntityList(List<Customer> customers)
    {
        List<CustomerForScheduleViewModel> viewModelList = [];
        customers.Select(x=>new CustomerForScheduleViewModel()
            .FromEntity(x))
            .ToList()
            .ForEach(x=>viewModelList.Add(x));
        return viewModelList;
    }

    public async Task<Result<AvailableBlock>> ScheduleIfAvailable(Customer customer, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var startDate = date.Date.Add(startTime);
        var endDate = date.Date.Add(endTime);
        var addBlock = customer.AddAvailableBlock(startDate, endDate);
        if (addBlock.IsFailure)
        {
            return Result<AvailableBlock>.Failure(addBlock.Error);
        }
        var block = addBlock.Value;
        await _availableBlockRepository.AddAsync(block);
        return Result<AvailableBlock>.Success(block);
    }
}