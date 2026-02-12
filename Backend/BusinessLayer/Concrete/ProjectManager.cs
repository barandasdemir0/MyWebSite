using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DtoLayer.GuestBookDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.Shared;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ProjectDto> AddAsync(CreateProjectDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<Project>(dto);
            entity.Slug = await UniqueSlugAsync(dto.Name, cancellationToken);
            if (entity.IsPublished && entity.PublishedAt == null)
            {
                entity.PublishedAt = DateTime.UtcNow;
            }
            if (dto.TopicIds != null)
            {
                foreach (var topic in dto.TopicIds)
                {
                    entity.ProjectTopics.Add(new ProjectTopic { TopicId = topic });
                }
            }
            await _projectDal.AddAsync(entity, cancellationToken);
            await _projectDal.SaveAsync(cancellationToken);
            return _mapper.Map<ProjectDto>(entity);
        }

        public async Task DeleteAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.GetByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity != null)
            {
                await _projectDal.DeleteAsync(entity, cancellationToken);
                await _projectDal.SaveAsync(cancellationToken);
            }
        }

        public async Task<List<ProjectDto>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.GetAllAsync(tracking: false, includes: source => source.Include(x => x.ProjectTopics).ThenInclude(y => y.Topic), cancellationToken: cancellationToken);
            return _mapper.Map<List<ProjectDto>>(entity);
        }

        public async Task<ProjectDto?> GetByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.GetByIdAsync(guid, tracking: false,includes:source=>source.Include(x=>x.ProjectTopics).ThenInclude(y=>y.Topic), cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProjectDto>(entity);
        }

        public async Task<ProjectDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.GetAsync(x => x.Slug == slug, tracking: false, includes: source => source.Include(x => x.ProjectTopics).ThenInclude(y => y.Topic), cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProjectDto>(entity);

        }

        public async Task<ProjectDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.GetByIdAsync(guid, tracking: false,includes:source=>source.Include(x=>x.ProjectTopics).ThenInclude(y=>y.Topic), cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<ProjectDto>(entity);
        }

        public async Task<ProjectDto?> UpdateAsync(Guid guid, UpdateProjectDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.GetAsync(x => x.Id == guid,
            tracking: true,
            includes: source => source.Include(x => x.ProjectTopics).ThenInclude(y => y.Topic), cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            if (entity.IsPublished && entity.PublishedAt == null)
            {
                entity.PublishedAt = DateTime.UtcNow;
            }
            entity.ProjectTopics.Clear();
            if (dto.TopicIds != null)
            {
                foreach (var topic in dto.TopicIds)
                {
                    entity.ProjectTopics.Add(new ProjectTopic { TopicId = topic });

                }
            }
            await _projectDal.UpdateAsync(entity, cancellationToken: cancellationToken);
            await _projectDal.SaveAsync(cancellationToken);
            return _mapper.Map<ProjectDto>(entity);
        }

        public async Task<string> UniqueSlugAsync(string name, CancellationToken cancellationToken = default)
        {
            string slug = name.AutoSlug();

            string originalslug = slug;
            int counter = 1;
            while (await _projectDal.GetAsync(x => x.Slug == slug, cancellationToken: cancellationToken) != null)
            {
                counter++;
                slug = $"{originalslug}-{counter}";
            }
            return slug;

        }

        public async Task<ProjectListDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            var entity = await _projectDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return null;
            }
            entity.IsDeleted = false;
            entity.DeletedAt = null;

            await _projectDal.UpdateAsync(entity, cancellationToken);
            await _projectDal.SaveAsync(cancellationToken);

            return _mapper.Map<ProjectListDto>(entity);
        }

        public async Task<PagedResult<ProjectListDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default)
        {
            //var entity = await _projectDal.GetAllAdminAsync(tracking: false);
            //return _mapper.Map<PagedResult<ProjectListDto>>(entity);
            var (items, totalCount) = await _projectDal.GetAdminListPagesAsync(
                query.PageNumber,
                query.PageSize,
                query.TopicId
                , cancellationToken);

            return _mapper.Map<List<ProjectListDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
        }
    }
}
