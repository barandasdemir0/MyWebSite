using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.GuestBookDto;
using DtoLayer.ProjectDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class ProjectManager : IProjectService
    {
        private readonly IProjectDal _projectDal;
        private readonly IMapper _mapper;

        public ProjectManager(IProjectDal projectDal, IMapper mapper)
        {
            _projectDal = projectDal;
            _mapper = mapper;
        }

        public async Task<ProjectListDto> AddAsync(CreateProjectDto dto)
        {
            var entity = _mapper.Map<Project>(dto);
            entity.Slug = await UniqueSlugAsync(dto.Name);
            if (dto.TopicIds!=null)
            {
                foreach (var topic in dto.TopicIds)
                {
                    entity.ProjectTopics.Add(new ProjectTopic { TopicId = topic });
                }
            }
            await _projectDal.AddAsync(entity);
            await _projectDal.SaveAsync();
            return _mapper.Map<ProjectListDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _projectDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _projectDal.DeleteAsync(entity);
                await _projectDal.SaveAsync();
            }
        }

        public async Task<List<ProjectListDto>> GetAllAsync()
        {
            var entity = await _projectDal.GetAllAsync(tracking: false, includes: x => x.ProjectTopics);
            return _mapper.Map<List<ProjectListDto>>(entity);
        }

        public async Task<ProjectListDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _projectDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProjectListDto>(entity);
        }

        public async Task<ProjectDto?> GetBySlugAsync(string slug)
        {
            var entity = await _projectDal.GetAsync(x => x.Slug == slug, tracking: false, includes: x => x.ProjectTopics);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProjectDto>(entity);

        }

        public async Task<ProjectDto?> GetDetailsByIdAsync(Guid guid)
        {
            var entity = await _projectDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProjectDto>(entity);
        }

        public async Task<ProjectListDto?> UpdateAsync(UpdateProjectDto dto)
        {
            var entity = await _projectDal.GetAsync(x => x.Id == dto.Id,
            tracking: true,
            includes: x => x.ProjectTopics);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            entity.ProjectTopics.Clear();
            if (dto.TopicIds!=null)
            {
                foreach (var topic in dto.TopicIds)
                {
                    entity.ProjectTopics.Add(new ProjectTopic { TopicId = topic });

                }
            }
            await _projectDal.UpdateAsync(entity);
            await _projectDal.SaveAsync();
            return _mapper.Map<ProjectListDto>(entity);
        }

        public async Task<string> UniqueSlugAsync(string name)
        {
            string slug = name.AutoSlug();

            string originalslug = slug;
            int counter = 1;
            while (await _projectDal.GetAsync(x => x.Slug == slug) != null)
            {
                counter++;
                slug = $"{originalslug}-{counter}";
            }
            return slug;

        }

        public async Task<ProjectListDto?> RestoreAsync(Guid guid)
        {
            var entity = await _projectDal.RestoreDeleteByIdAsync(guid);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _projectDal.UpdateAsync(entity);
            await _projectDal.SaveAsync();

            return _mapper.Map<ProjectListDto>(entity);
        }
    }
}
