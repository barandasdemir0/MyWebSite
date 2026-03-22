using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfRolePermissionDal : IRolePermissionDal
{
    private readonly AppDbContext _appDbContext;

    public EfRolePermissionDal(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _appDbContext.RolePermissions.Where(x => x.RoleName == roleName).Select(y => y.Permission).ToListAsync(cancellationToken);
    }

    public async Task SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken)
    {
        await using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

        var existing = _appDbContext.RolePermissions.Where(x => x.RoleName == roleName);
        _appDbContext.RolePermissions.RemoveRange(existing);
        foreach (var item in permissions)
        {
            _appDbContext.RolePermissions.Add(new RolePermission
            {
                RoleName = roleName,
                Permission = item
            });


        }
        await _appDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
