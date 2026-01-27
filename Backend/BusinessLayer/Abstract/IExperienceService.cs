using CV.EntityLayer.Entities;
using DtoLayer.ExperienceDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IExperienceService:IGenericService<Experience,ExperienceDto,CreateExperienceDto,UpdateExperienceDto>
    {
    }
}
