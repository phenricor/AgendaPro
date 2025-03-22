using System.Data;
using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.ValueObjects;
using AgendaPro.Infrastructure.Repositories;
using AgendaPro.Web.Models;
using AgendaPro.Web.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace AgendaPro.Web.Controllers;

public class CustomersController : Controller
{
    private readonly CustomerRepository _customerRepository;

    public CustomersController(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return PartialView("_CreateCustomerModal", new CreateCustomerRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            var error = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
            return Json(new { success = false, errorMessage = error });
        }
        if (request.ToModel().IsFailure)
        {
            return Json(new { 
                success = false, 
                error = request.ToModel().Error.Description, 
                field = request.ToModel().Error.FieldValidation 
            });
        }
        if (await _customerRepository.FindByCnpj(request.Cnpj) != null)
        {
            return Json(new { success = false, error = "CNPJ already registed.", field = "cnpj" });
        }
        if (await _customerRepository.FindByPhone(request.Phone) != null)
        {
            return Json(new { success = false, error = "Phone number already registed.", field = "phone" });
        }
        if (await _customerRepository.FindByEmail(request.Email) != null)
        {
            return Json(new { success = false, error = "Email already used.", field = "email" });
        }
        var customer = request.ToModel().Value;
        if (customer == null)
        {
            return Json(new { success = false, error = "Something went wrong." });
        }
        await _customerRepository.AddAsync(customer);
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<JsonResult> Delete(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id.ToString());
        await _customerRepository.DeleteAsync(id.ToString());
        return Json(new { success = true });
    }

    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        var result = customers.Select(c => new CustomerModel().FromModel(c)).ToList();
        return PartialView("_CustomersTable", result);
    }
}