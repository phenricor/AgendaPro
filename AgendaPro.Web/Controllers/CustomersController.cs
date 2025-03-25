using System.Data;
using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Domain.Shared;
using AgendaPro.Domain.ValueObjects;
using AgendaPro.Infrastructure.Repositories;
using AgendaPro.Web.Models;
using AgendaPro.Web.Models.Customer;
using AgendaPro.Web.Services;
using AgendaPro.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace AgendaPro.Web.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public IActionResult Index(int pageIndex, int pageSize)
    {
        if (TempData["Message"] != null)
        {
            ViewBag.Message = TempData["Message"];
        }
        ViewBag.PageIndex = pageIndex;
        ViewBag.PageSize = pageSize;
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
        var validation = _customerService.ValidateFields(request);
        if (validation.Result.IsFailure)
        {
            return Json(new
            {
                success = false,
                error = validation.Result.Error.Description,
                field = validation.Result.Error.FieldValidation
            });
        }
        var customer = request.ToModel().Value;
        if (customer == null)
        {
            return Json(new { success = false, error = "Something went wrong." });
        }
        await _customerService.AddCustomer(customer);
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _customerService.RemoveCustomer(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }
        TempData["Message"] = "Customer deleted successfully.";
        return RedirectToAction("Index");
    }

    public IActionResult LoadCustomersTable([FromQuery] int pageSize, [FromQuery] int pageIndex)
    {
        var paginatedViewModels = _customerService.ReturnPaginatedViewModel(pageIndex, pageSize);
        return PartialView("_CustomersTable", paginatedViewModels);
    }
    
    public async Task<IActionResult> Edit(Guid id)
    {
        var customer = await _customerService.FindCustomer(id);
        var viewModel = new EditCustomerRequest().FromModel(customer);
        if (viewModel.IsFailure)
        {
            return BadRequest(viewModel.Error.Description);
        }
        return PartialView("_EditCustomerModal", viewModel.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] EditCustomerRequest request)
    {
        if (!ModelState.IsValid)
        {
            var error = String.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList());
            return Json(new { success = false, errorMessage = error });
        }
        var customer = await _customerService.FindCustomer(request.Id);
        var validation = _customerService.ValidateFields(request, customer);
        if (validation.Result.IsFailure)
        {
            return Json(new
            {
                success = false,
                error = validation.Result.Error.Description,
                field = validation.Result.Error.FieldValidation
            });
        }
        var mappedCustomer = request.MapValues(customer);
        if (mappedCustomer.IsFailure)
        {
            return Json(new
            {
                success = false,
                error = validation.Result.Error.Description,
                field = validation.Result.Error.FieldValidation
            });
        }
        await _customerService.UpdateCustomer(mappedCustomer.Value);
        return Ok(new { success = true });
    }
}