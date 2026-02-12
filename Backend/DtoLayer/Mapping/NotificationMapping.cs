using DtoLayer.NotificationDtos;
using EntityLayer.Concrete;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public class NotificationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Notification, NotificationDtos.NotificationDto>();
        config.NewConfig<CreateNotificationDto, Notification>().Ignore(x => x.Id);
        config.NewConfig<UpdateNotificationDto, Notification>().Ignore(x => x.Id);
    }
}
