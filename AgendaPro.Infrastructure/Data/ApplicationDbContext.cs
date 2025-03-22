using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgendaPro.Domain.Entities;

namespace AgendaPro.Infrastructure.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    DbSet<Customer> Customers { get; set; }
    DbSet<Availability> Availabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // modelBuilder.Entity<Customer>()
        //     .OwnsOne(c => c.Cnpj, cb =>
        //     {
        //         cb.Property(p => p.Value)
        //             .HasColumnName("Cnpj")
        //             .IsRequired();
        //     });
    }
}