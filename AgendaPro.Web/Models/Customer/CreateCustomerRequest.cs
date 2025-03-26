using System.ComponentModel.DataAnnotations;
using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.ValueObjects;

namespace AgendaPro.Web.Models;

public class CreateCustomerRequest
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(10)]
    public string? Cnpj { get; set; }
    [Required]
    [MinLength(8)]
    public string? Phone { get; set; }

    public Result<Domain.Entities.Customer> ToModel()
    {
        var result = Domain.Entities.Customer.Create(Name, Email, Cnpj, Phone);
        return result;
    }
}