using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Interfaces;
using AgendaPro.Infrastructure.Data;

namespace AgendaPro.Infrastructure.Repositories;

public class AvailableBlockRepository : BaseRepository<AvailableBlock>, IAvailableBlockRepository
{
    public AvailableBlockRepository(ApplicationDbContext context) : base(context) { }
}