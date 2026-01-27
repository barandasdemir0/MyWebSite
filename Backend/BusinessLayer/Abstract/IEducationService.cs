using CV.EntityLayer.Entities;
using DtoLayer.EducationDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IEducationService:IGenericService<Education,EducationDto,CreateEducationDto,UpdateEducationDto>
    {
    }
}
