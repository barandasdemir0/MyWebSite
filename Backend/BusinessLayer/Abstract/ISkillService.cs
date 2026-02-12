using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using DtoLayer.SkillDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ISkillService:IGenericService<Skill,SkillDto,CreateSkillDto,UpdateSkillDto>
    {
        Task<SkillDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);

    }
}
