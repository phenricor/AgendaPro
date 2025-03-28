using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Domain.Shared;
using AgendaPro.Infrastructure.Repositories;
using AgendaPro.Web.Controllers;
using AgendaPro.Web.Models;
using AgendaPro.Web.Models.Customer;
using AgendaPro.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace AgendaPro.Web.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAvailableBlockRepository _availableBlockRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository, 
        IAvailableBlockRepository availableBlockRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _availableBlockRepository = availableBlockRepository;
        _logger = logger;
    }

    public async Task<Result<Customer?>> GetCustomer(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            return Result<Customer?>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            return Result<Customer?>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        return Result<Customer?>.Success(customer);
    }
    public async Task<Result<Customer>> FindCustomer(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            return Result<Customer>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            return Result<Customer>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        return Result<Customer>.Success(customer);
    }

    public async Task<Result<EditCustomerRequest>> ReturnEditCustomerRequest(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            return Result<EditCustomerRequest>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            return Result<EditCustomerRequest>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var viewModel = new EditCustomerRequest().FromModel(customer);
        if (viewModel.IsFailure || viewModel.Value == null)
        {
            return Result<EditCustomerRequest>.Failure(viewModel.Error);
        }
        return Result<EditCustomerRequest>.Success(viewModel.Value);
    }
    public async Task AddCustomer(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateCustomer(Customer? customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }
    public IPagedList<CustomerModel> ReturnPaginatedViewModel(int pageIndex, int pageSize)
    {
        if (pageIndex <= 0) pageIndex = 1;
        if (pageSize < 10) pageSize = 10;
        var viewModelQueryable = _customerRepository.GetCustomersQueryable()
            .Select(x => new CustomerModel().FromModel(x));
        var paginatedViewModel = viewModelQueryable.ToPagedList(pageIndex, pageSize);
        return paginatedViewModel;
    }
    public async Task<Result<bool>> RemoveCustomer(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            return Result<bool>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        await _customerRepository.DeleteAsync(customerId);
        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> ValidateFields(EditCustomerRequest request, Customer? customer)
    {
        // var customer = _customerRepository.GetCustomersQueryable().FirstOrDefault(x => x.Id == request.Id);
        if (customer == null)
        {
            return Result<bool>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        if (await CnpjAlreadyExists(request.Cnpj) && customer.Cnpj.Value != request.Cnpj)
        {
            return Result<bool>.Failure(CustomerErrors.CnpjAlreadyUsed);
        }
        if (await PhoneAlreadyExists(request.Phone) && customer.Phone.Value != request.Phone)
        {
            return Result<bool>.Failure(CustomerErrors.PhoneAlreadyUsed);
        }
        if (await EmailAlreadyExists(request.Email) && customer.Email != request.Email)
        {
            return Result<bool>.Failure(CustomerErrors.EmailAlreadyUsed);
        }
        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> ValidateFields(CreateCustomerRequest request)
    {
        if (request.ToModel().IsFailure)
        {
            return Result<bool>.Failure(request.ToModel().Error);
        }
        if (await CnpjAlreadyExists(request.Cnpj))
        {
            return Result<bool>.Failure(CustomerErrors.CnpjAlreadyUsed);
        }
        if (await PhoneAlreadyExists(request.Phone))
        {
            return Result<bool>.Failure(CustomerErrors.PhoneAlreadyUsed);
        }
        if (await EmailAlreadyExists(request.Email))
        {
            return Result<bool>.Failure(CustomerErrors.EmailAlreadyUsed);
        }
        return Result<bool>.Success(true);
    }

    public async Task<Result<AvailableBlock>> AddAvailableBlock(Guid customerId, DateTime startDate, DateTime endDate)
    {
        var customer = await GetCustomer(customerId);
        if (customer.IsFailure)
        {
            return Result<AvailableBlock>.Failure(CustomerErrors.CustomerDoesNotExist);
        }
        var availabilyBlock = AvailableBlock.Create(startDate, endDate, customerId);
        if (availabilyBlock.IsFailure)
        {
            return Result<AvailableBlock>.Failure(availabilyBlock.Error);
        }
        await _availableBlockRepository.AddAsync(availabilyBlock.Value);
        return Result<AvailableBlock>.Success(availabilyBlock.Value);
    }

    private async Task<bool> CnpjAlreadyExists(string? cnpj)
    {
        return await _customerRepository.FindByCnpj(cnpj) != null;
    }
    private async Task<bool> PhoneAlreadyExists(string? phone)
    {
        return await _customerRepository.FindByPhone(phone) != null;
    }
    private async Task<bool> EmailAlreadyExists(string email)
    {
        return await _customerRepository.FindByEmail(email) != null;
    }
}