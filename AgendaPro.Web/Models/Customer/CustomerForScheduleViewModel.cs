using AgendaPro.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AgendaPro.Web.Models.Customer;

public class CustomerForScheduleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; private set; }

    public CustomerForScheduleViewModel FromEntity(Domain.Entities.Customer entity)
    {
        var model = new CustomerForScheduleViewModel()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        return model;
    }
}