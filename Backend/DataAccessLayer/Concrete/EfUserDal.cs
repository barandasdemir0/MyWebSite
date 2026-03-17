using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfUserDal : IUserDal
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _appDbContext;
    public EfUserDal(UserManager<AppUser> userManager, AppDbContext appDbContext)
    {
        _userManager = userManager;
        _appDbContext = appDbContext;
    }

    public async Task<List<AppUser>> GetApprovedUserAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users.Where(u => u.IsApproved).ToListAsync(cancellationToken);
    }

    public async Task<List<AppUser>> GetPendingUserAsync(CancellationToken cancellationToken)
    {
      
        return await _userManager.Users.Where(u => !u.IsApproved).ToListAsync(cancellationToken);
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _appDbContext.RolePermissions.Where(x => x.RoleName == roleName).Select(y => y.Permission).ToListAsync(cancellationToken);
    }

    public async Task SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken)
    {
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
    }
}
