namespace DtoLayer.AuthDtos.Items;

public record PermissionItem
{
    public string Key { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
}
