using DtoLayer.AuthDtos.Items;
using Mapster;

namespace DtoLayer.Mapping;

public class AuthItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // PermissionItem -> başka bir DTO mapping gerekirse ekle
        // Örnek: Listeleme için map

        //PermissionItem
        //AssignRoleDto(opsiyonel, request gibi de düşünülebilir)
    }
}
