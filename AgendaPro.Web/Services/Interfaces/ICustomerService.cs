using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Shared;
using AgendaPro.Web.Controllers;
using AgendaPro.Web.Models;
using AgendaPro.Web.Models.Customer;
using X.PagedList;

namespace AgendaPro.Web.Services.Interfaces;

public interface ICustomerService
{
    public Task<Result<Customer>> FindCustomer(Guid customerId);
    public Task AddCustomer(Customer customer);
    public Task UpdateCustomer(Customer? customer);
    public IPagedList<CustomerModel> ReturnPaginatedViewModel(int pageIndex, int pageSize);
    public Task<Result<bool>> RemoveCustomer(Guid customerId);
    public Task<Result<bool>> ValidateFields(CreateCustomerRequest request);
    public Task<Result<bool>> ValidateFields(EditCustomerRequest request, Customer? customer);
    public Task<Result<EditCustomerRequest>> ReturnEditCustomerRequest(Guid customerId);
}