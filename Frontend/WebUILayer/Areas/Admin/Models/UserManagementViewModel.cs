// DtoLayer/AuthDtos/UserManagementViewModel.cs (YENİ)
namespace DtoLayer.AuthDtos;

public class UserManagementViewModel
{
    public List<PendingUserDto> PendingUsers { get; set; } = new();
    public List<ApprovedUserDto> ApprovedUsers { get; set; } = new();
    public string SelectedRole { get; set; } = "Editor";
    public List<string> ActivePermissions { get; set; } = new();
    public List<PermissionItem> AllPermissions { get; set; } = new();
}

public class PermissionItem
{
    public string Key { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}
