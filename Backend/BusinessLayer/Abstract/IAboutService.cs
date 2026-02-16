using CV.EntityLayer.Entities;
using DtoLayer.AboutDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IAboutService: IGenericService<About, AboutDto, CreateAboutDto, UpdateAboutDto>                 /*IGenericService<AboutDto>*/
    {

        Task<AboutDto?> GetSingleAsync(CancellationToken cancellationToken = default);

        Task<AboutDto> SaveAsync(UpdateAboutDto update, CancellationToken cancellation = default);
    }
}
