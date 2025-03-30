using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AgendaPro.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AgendaPro.Web.Models.Customer;

public class CustomerScheduleRequest
{
    public Guid Id { get; set; }
    [Required]
    public DateTime Date { get; set; } = DateTime.Now;
    [Required]
    public TimeSpan StartTime { get; set; } = TimeSpan.FromMinutes(480);
    [Required]
    public TimeSpan EndTime { get; set; } = TimeSpan.FromMinutes(510);
}