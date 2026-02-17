using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.JobSkillsDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concrete;

public class JobSkillManager : GenericManager<JobSkill,JobSkillDto,CreateJobSkillDto,UpdateJobSkillDto>, IJobSkillService
{
    private readonly IJobSkillDal _jobSkillDal;

    public JobSkillManager(IJobSkillDal jobSkillDal, IMapper mapper) : base(jobSkillDal, mapper)
    {
        _jobSkillDal = jobSkillDal;
    }


    public async Task<List<JobSkillDto>> GetAdminAllAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillDal.GetAllAdminAsync(tracking: false,includes:source=>source.Include(x=>x.JobSkillCategory), cancellationToken: cancellationToken);
        return _mapper.Map<List<JobSkillDto>>(entity);
    }

    public override async Task<List<JobSkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillDal.GetAllAsync(tracking: false,includes:source=>source.Include(x=>x.JobSkillCategory), cancellationToken: cancellationToken);
        return _mapper.Map<List<JobSkillDto>>(entity);
    }

    public override async Task<JobSkillDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillDal.GetByIdAsync(guid, tracking: false,includes:source=>source.Include(x=>x.JobSkillCategory), cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<JobSkillDto>(entity);
    }

    public async Task<JobSkillDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var query = await _jobSkillDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
        if (query == null)
        {
            return null;
        }
        query.IsDeleted = false;
        query.DeletedAt = null;

        await _jobSkillDal.UpdateAsync(query, cancellationToken);
        await _jobSkillDal.SaveAsync(cancellationToken);
        return _mapper.Map<JobSkillDto>(query);
    }
}
