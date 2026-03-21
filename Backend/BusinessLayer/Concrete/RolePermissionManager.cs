using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;

namespace BusinessLayer.Concrete;

public class RolePermissionManager : IRolePermissionService
{

    private readonly IUserDal _userDal;

    public RolePermissionManager(IUserDal userDal)
    {
        _userDal = userDal;
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _userDal.GetRolePermissionsAsync(roleName, cancellationToken);
    }

    public async Task<bool> SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken)
    {
        await _userDal.SaveRolePermissionsAsync(roleName, permissions, cancellationToken);
        return true;
    }
}
