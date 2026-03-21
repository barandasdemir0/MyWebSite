namespace BusinessLayer.Abstract;

public interface IRolePermissionService
{
    Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken);
    Task<bool> SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken);
}
