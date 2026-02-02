using CV.EntityLayer.Entities;
using DtoLayer.CertificateDto;
using DtoLayer.ExperienceDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IExperienceService:IGenericService<Experience,ExperienceDto,CreateExperienceDto,UpdateExperienceDto>
    {
        Task<ExperienceDto?> RestoreAsync(Guid guid);
        Task<List<ExperienceDto>> GetAllAdminAsync();
    }
}
