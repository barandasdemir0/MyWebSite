using CV.EntityLayer.Entities;
using DtoLayer.TopicDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ITopicService:IGenericService<Topic,TopicDto,CreateTopicDto,UpdateTopicDto>
    {
    }
}
