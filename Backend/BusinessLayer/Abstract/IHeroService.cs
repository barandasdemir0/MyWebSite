using CV.EntityLayer.Entities;
using DtoLayer.ContactDtos;
using DtoLayer.HeroDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IHeroService:IGenericService<Hero,HeroDto,CreateHeroDto,UpdateHeroDto>
    {
        Task<HeroDto?> GetSingleAsync(CancellationToken cancellationToken = default);

        Task<HeroDto> SaveAsync(UpdateHeroDto updateHeroDto, CancellationToken cancellationToken = default);
    }
}
