using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Domain.ValueObjects;
using AgendaPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AgendaPro.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context) {}
    public List<Customer> GetCustomersList()
    {
        return DbSet.ToList();
    }
    public IQueryable<Customer> GetCustomersQueryable()
    {
        return DbSet.AsNoTracking();
    }
    public async Task<Customer?> FindByCnpj(string? cnpj)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Cnpj.Value == cnpj);
    }
    
    public async Task<Customer?> FindByPhone(string? phone)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Phone.Value == phone);
    }

    public async Task<Customer?> FindByEmail(string email)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}