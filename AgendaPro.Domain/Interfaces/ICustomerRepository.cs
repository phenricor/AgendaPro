using AgendaPro.Domain.Entities;

namespace AgendaPro.Domain.Interfaces;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    public List<Customer> GetCustomersList();
    public IQueryable<Customer> GetCustomersQueryable();
    public Task<Customer?> FindByCnpj(string? cnpj);
    public Task<Customer?> FindByPhone(string? phone);
    public Task<Customer?> FindByEmail(string email);
}