using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.JobSkillCategoryDtos;
using DtoLayer.JobSkillsDtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concrete;

public class JobSkillCategoryManager : GenericManager<JobSkillCategory,JobSkillCategoryDto,CreateJobSkillCategoryDto,UpdateJobSkillCategoryDto>,IJobSkillCategoryService
{
    private readonly IJobSkillCategoryDal _jobSkillCategoryDal;

    public JobSkillCategoryManager(IJobSkillCategoryDal jobSkillCategoryDal, IMapper mapper) : base(jobSkillCategoryDal, mapper)
    {
        _jobSkillCategoryDal = jobSkillCategoryDal;
    }

    public async Task<List<JobSkillCategoryDto>> GetAdminAllAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetAllAdminAsync(tracking:false,includes:source=>source.Include(x=>x.JobSkills),cancellationToken:cancellationToken); //tracking yani izlemeyi kapat yetenekleride dahil et yani backend klasöründe C# ve yüzdeliği getir
        return _mapper.Map<List<JobSkillCategoryDto>>(entity); //entityi dtoya dönüştür
    }

    public override async Task<List<JobSkillCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetAllAsync(tracking: false,includes:source=>source.Include(x=>x.JobSkills),cancellationToken:cancellationToken);
        return _mapper.Map<List<JobSkillCategoryDto>>(entity);
    }

    public  override async Task<JobSkillCategoryDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _jobSkillCategoryDal.GetByIdAsync(guid,tracking:false,includes:source=>source.Include(x=>x.JobSkills),cancellationToken:cancellationToken);
        if (entity == null)
        {
            return null;
        }
        return _mapper.Map<JobSkillCategoryDto>(entity);
    }

    public async Task<JobSkillCategoryDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var query = await _jobSkillCategoryDal.RestoreDeleteByIdAsync(guid,cancellationToken:cancellationToken); // silinen kaydı bul
        if (query == null)
        {
            return null;
        }
        query.IsDeleted = false;
        query.DeletedAt = null;
        await _jobSkillCategoryDal.UpdateAsync(query,cancellationToken);
        await _jobSkillCategoryDal.SaveAsync(cancellationToken);
        return _mapper.Map<JobSkillCategoryDto>(query);
    }

}
