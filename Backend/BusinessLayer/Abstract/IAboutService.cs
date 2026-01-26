using CV.EntityLayer.Entities;
using DtoLayer.AboutDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IAboutService: IGenericService<About, AboutDto, CreateAboutDto, UpdateAboutDto>                 /*IGenericService<AboutDto>*/
    {
    }
}
