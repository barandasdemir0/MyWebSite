using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDto;
using DtoLayer.ProjectDto;
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
