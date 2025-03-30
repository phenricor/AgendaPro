using System.Text.Json;
using AgendaPro.Web.Models;
using AgendaPro.Web.Models.Customer;
using AgendaPro.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;

namespace AgendaPro.Web.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerFacade _facade;
    private readonly IScheduler _scheduler;

    public CustomersController(
        ICustomerFacade facade,
        IScheduler scheduler)
    {
        _facade = facade;
        _scheduler = scheduler;
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
            var error = String.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList());
            return Json(new { success = false, errorMessage = error });
        }
        var validation = _facade.ValidateFields(request);
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
        await _facade.AddCustomer(customer);
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _facade.RemoveCustomer(id);
        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }
        TempData["Message"] = "Customer deleted successfully.";
        return RedirectToAction("Index");
    }

    public IActionResult LoadCustomersTable([FromQuery] int pageSize, [FromQuery] int pageIndex)
    {
        var paginatedViewModels = _facade.ReturnPaginatedViewModel(pageIndex, pageSize);
        return PartialView("_CustomersTable", paginatedViewModels);
    }
    
    public async Task<IActionResult> Edit(Guid id)
    {
        var viewModel = await _facade.ReturnEditCustomerRequest(id);
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
        var customer = await _facade.FindCustomer(request.Id);
        if (customer.IsFailure)
        {
            return Json(new { success = false, error = customer.Error.Description });
        }
        var validation = _facade.ValidateFields(request, customer.Value);
        if (validation.Result.IsFailure)
        {
            return Json(new
            {
                success = false,
                error = validation.Result.Error.Description,
                field = validation.Result.Error.FieldValidation
            });
        }
        var mappedCustomer = request.MapValues(customer.Value);
        if (mappedCustomer.IsFailure)
        {
            return Json(new
            {
                success = false,
                error = mappedCustomer.Error.Description,
                field = mappedCustomer.Error.FieldValidation
            });
        }
        await _facade.UpdateCustomer(mappedCustomer.Value);
        return Ok(new { success = true });
    }

    public async Task<IActionResult> Scheduler()
    {
        var customers = await _facade.GetAllCustomers();
        if (customers.IsFailure)
        {
            ViewBag.Message = customers.Error.Description;
        }
        if (customers.Value == null)
        {
            return View();
        }
        var models = _scheduler.ReturnViewModelFromEntityList(customers.Value);
        return View(models);
    }

    public IActionResult GetScheduleForm()
    {
        return PartialView("_ScheduleForm", new CustomerScheduleRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Scheduler([FromBody] CustomerScheduleRequest? request)
    {
        if (!ModelState.IsValid || request == null)
        {
            return Json(new { success = false, error = "Something went wrong." });
        }
        var customerResult = await _facade.FindCustomer(request.Id);
        if (customerResult.IsFailure || customerResult.Value == null)
        {
            return Json(new { success = false, error = customerResult.Error.Description });
        }
        var customer = customerResult.Value;
        if (customer == null)
        {
            return Json(new { success = false, error = "Something went wrong." });
        }
        var scheduleResult = await _scheduler.ScheduleIfAvailable(customer, request.Date, request.StartTime, request.EndTime);
        if (scheduleResult.IsFailure)
        {
            return Json(new { success = false, error = scheduleResult.Error.Description });
        }
        return Json(new { success = true });
    }

    public async Task<IActionResult> GetAvailableBlocks([FromQuery] Guid customerId)
    {
        var customer = await _facade.FindCustomer(customerId);
        var blocks = customer.Value.AvailableBlocks
            .Select(b => new
            {
                start = b.StartDate,
                end = b.EndDate
            });
        return Json(blocks);
    }
}