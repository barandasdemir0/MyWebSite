using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;

namespace BusinessLayer.Concrete;

public class RolePermissionManager : IRolePermissionService
{

    private readonly IRolePermissionDal _rolePermissionDal;

    public RolePermissionManager(IRolePermissionDal rolePermissionDal)
    {
        _rolePermissionDal = rolePermissionDal;
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken)
    {
        return await _rolePermissionDal.GetRolePermissionsAsync(roleName, cancellationToken);
    }

    public async Task<bool> SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken)
    {
        await _rolePermissionDal.SaveRolePermissionsAsync(roleName, permissions, cancellationToken);
        return true;
    }
}
