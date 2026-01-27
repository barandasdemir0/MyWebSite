using CV.EntityLayer.Entities;
using DtoLayer.HeroDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IHeroService:IGenericService<Hero,HeroDto,CreateHeroDto,UpdateHeroDto>
    {
    }
}
