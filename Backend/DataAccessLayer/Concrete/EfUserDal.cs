using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete;

public class EfUserDal : IUserDal
{
    private readonly UserManager<AppUser> _userManager;
    public EfUserDal (UserManager<AppUser> userManager) 
    {
        _userManager = userManager;
    }

    public async Task<List<AppUser>> GetApprovedUserAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users.Where(u => !u.IsApproved).ToListAsync(cancellationToken);
    }

    public async Task<List<AppUser>> GetPendingUserAsync(CancellationToken cancellationToken)
    {
        return await _userManager.Users.Where(u => u.IsApproved).ToListAsync(cancellationToken);
    }
}
