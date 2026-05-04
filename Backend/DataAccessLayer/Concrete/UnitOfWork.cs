using DataAccessLayer.Abstract;
using DataAccessLayer.Context;

namespace DataAccessLayer.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Dispose()
    {
         _appDbContext.Dispose();
        
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return await _appDbContext.SaveChangesAsync(cancellationToken);
    }
}
