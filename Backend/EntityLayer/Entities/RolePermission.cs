namespace EntityLayer.Entities;

public class RolePermission
{
    public RolePermission()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; protected set; }
    public string RoleName { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;
}
