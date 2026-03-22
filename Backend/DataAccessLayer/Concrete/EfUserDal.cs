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

    public async Task<Dictionary<Guid, List<string>>> GetUserRolesBatchAsync(List<Guid> userIds, CancellationToken cancellationToken)
    {
        return await _appDbContext.UserRoles.
            Where(u => userIds.Contains(u.UserId)).
            Join(_appDbContext.Roles, u => u.RoleId, r => r.Id, (u, r) => new
            {
                u.UserId,
                RoleName = r.Name!
            }).GroupBy(x => x.UserId).ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleName).ToList(), cancellationToken);
    }
}
