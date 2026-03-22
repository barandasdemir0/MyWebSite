namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IRolePermissionApiService
{
    Task<List<string>> GetRolePermissionsAsync(string roleName);
    Task<bool> SaveRolePermissions(string roleName, List<string> permissions);
}
