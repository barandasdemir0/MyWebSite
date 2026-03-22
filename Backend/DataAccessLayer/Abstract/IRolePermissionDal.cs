namespace DataAccessLayer.Abstract;

public interface IRolePermissionDal
{
    Task<List<string>> GetRolePermissionsAsync(string roleName, CancellationToken cancellationToken);
    Task SaveRolePermissionsAsync(string roleName, List<string> permissions, CancellationToken cancellationToken);
}
