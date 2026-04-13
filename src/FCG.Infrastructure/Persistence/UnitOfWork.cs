using System;
using System.Threading.Tasks;
using FCG.Domain.Interfaces.Respositories;
using FCG.Infrastructure.Data;

namespace FCG.Infrastructure.Persistence;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context = context;

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}