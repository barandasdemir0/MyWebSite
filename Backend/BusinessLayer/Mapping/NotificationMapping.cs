using CV.EntityLayer.Entities;
using Mapster;

namespace DtoLayer.Mapping;

public class NotificationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Notification, NotificationDtos.NotificationDto>();
    }
}
