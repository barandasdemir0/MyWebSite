using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using DtoLayer.TopicDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ITopicService:IGenericService<Topic,TopicDto,CreateTopicDto,UpdateTopicDto>
    {
        Task<TopicDto?> RestoreAsync(Guid guid);
        Task<List<TopicDto>> GetAllAdminAsync();
    }
}
