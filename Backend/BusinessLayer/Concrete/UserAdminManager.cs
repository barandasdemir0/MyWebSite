using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Concrete;

public class UserAdminManager : IUserAdminService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IUserDal _userDal;
    private readonly IMapper _mapper;

    public UserAdminManager(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUserDal userDal, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userDal = userDal;
        _mapper = mapper;
    }

    public async Task<bool> ApproveUserAsync(string userId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }
        user.IsApproved = true;
        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);

        return true;
    }

    public async Task<bool> AssignRoleAsync(string UserId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            return false;
        }
        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = role
            });

        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role);
        return true;
    }

    public async Task<List<ApprovedUserDto>> GetAllUserAsync(CancellationToken cancellationToken)
    {
        var users = await _userDal.GetApprovedUserAsync(cancellationToken);
        var userIds = users.Select(u => u.Id).ToList();
        var rolesMap = await _userDal.GetUserRolesBatchAsync(userIds, cancellationToken);

        return users.Select(item => 
        {

            var dto = _mapper.Map<ApprovedUserDto>(item);
            dto.Role = rolesMap.TryGetValue(item.Id, out var roles) ? roles.FirstOrDefault() ?? RoleConsts.User : RoleConsts.User;
            return dto;
               
        }).ToList();



    }

    public async Task<List<PendingUserDto>> GetPendingUsersAsync(CancellationToken cancellationToken)
    {
        var user = await _userDal.GetPendingUserAsync(cancellationToken);
        return _mapper.Map<List<PendingUserDto>>(user);
    }

    public async Task<bool> RejectUserAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}
