// DtoLayer/AuthDtos/UserManagementViewModel.cs (YENİ)
using EntityLayer.Entities;

namespace DtoLayer.AuthDtos;

public class UserManagementViewModel
{
    public List<PendingUserDto> PendingUsers { get; set; } = new();
    public List<ApprovedUserDto> ApprovedUsers { get; set; } = new();
    public string SelectedRole { get; set; } = RoleConsts.Editor;
    public List<string> ActivePermissions { get; set; } = new();
    public List<PermissionItem> AllPermissions { get; set; } = new();
    public List<string> AllRoles { get; set; } = new();
    public List<string> SelectableRoles { get; set; } = new();
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
