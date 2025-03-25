using System.ComponentModel.DataAnnotations;
using AgendaPro.Domain.Shared;

namespace AgendaPro.Web.Models.Customer;

public class EditCustomerRequest
{
    public Guid Id { get; set; }
    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(10)]
    public string Cnpj { get; set; }
    [Required]
    [MinLength(8)]
    public string Phone { get; set; }

    public Result<EditCustomerRequest> FromModel(Domain.Entities.Customer? customer)
    {
        if (customer == null)
        {
            return Result<EditCustomerRequest>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var viewModel = new EditCustomerRequest()
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Cnpj = customer.Cnpj.Value,
            Phone = customer.Phone.Value
        };
        return Result<EditCustomerRequest>.Success(viewModel);
    }
    public Result<Domain.Entities.Customer> MapValues(Domain.Entities.Customer? model)
    {
        if (model == null)
        {
            return Result<Domain.Entities.Customer>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        if (Domain.Entities.Customer.Validate(Cnpj, Phone).IsFailure)
        {
            return Result<Domain.Entities.Customer>.Failure(Domain.Entities.Customer.Validate(Cnpj, Phone).Error);
        }
        model.Update(Name, Email, Cnpj, Phone);
        return Result<Domain.Entities.Customer>.Success(model);
    }
}