using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using DtoLayer.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IProjectService:IGenericService<Project,ProjectListDto,CreateProjectDto,UpdateProjectDto>
    {
        Task<ProjectDto?> GetDetailsByIdAsync(Guid guid);

        Task<ProjectDto?> GetBySlugAsync(string slug);

        Task<ProjectListDto?> RestoreAsync(Guid guid);
        Task<List<ProjectListDto>> GetAllAdminAsync();
    }
}
