using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IProjectService:IGenericService<Project, ProjectDto, CreateProjectDto,UpdateProjectDto>
    {
        Task<ProjectDto?> GetDetailsByIdAsync(Guid guid);

        Task<ProjectDto?> GetBySlugAsync(string slug);

        Task<ProjectListDto?> RestoreAsync(Guid guid);
        Task<PagedResult<ProjectListDto>> GetAllAdminAsync(PaginationQuery query);
    }
}
