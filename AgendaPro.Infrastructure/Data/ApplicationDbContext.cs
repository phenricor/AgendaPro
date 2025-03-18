using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgendaPro.Domain.Entities;

namespace AgendaPro.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    DbSet<Customer> Customers { get; set; }
    DbSet<Availability> Availabilities { get; set; }
}