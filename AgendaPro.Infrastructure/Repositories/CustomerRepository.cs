using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Domain.ValueObjects;
using AgendaPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AgendaPro.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>
{
    public CustomerRepository(ApplicationDbContext context) : base(context) {}

    public async Task<Customer?> FindByCnpj(string cnpj)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Cnpj == cnpj);
    }
    
    public async Task<Customer?> FindByPhone(string phone)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Phone == phone);
    }

    public async Task<Customer?> FindByEmail(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
    }
}