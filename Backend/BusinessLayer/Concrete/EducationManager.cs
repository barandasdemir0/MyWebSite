using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.EducationDto;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class EducationManager : IEducationService
    {
        private readonly IEducationDal _educationDal;
        private readonly IMapper _mapper;

        public EducationManager(IEducationDal educationDal, IMapper mapper)
        {
            _educationDal = educationDal;
            _mapper = mapper;
        }

        public async Task<EducationDto> AddAsync(CreateEducationDto dto)
        {
            var entity = _mapper.Map<Education>(dto);
            await _educationDal.AddAsync(entity);
            await _educationDal.SaveAsync();
            return _mapper.Map<EducationDto>(entity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var entity = await _educationDal.GetByIdAsync(guid);
            if (entity != null)
            {
                await _educationDal.DeleteAsync(entity);
                await _educationDal.SaveAsync();
            }

        }

        public async Task<List<EducationDto>> GetAllAsync()
        {
            var entity = await _educationDal.GetAllAsync(tracking: false);
            return _mapper.Map<List<EducationDto>>(entity);
        }

        public async Task<EducationDto?> GetByIdAsync(Guid guid)
        {
            var entity = await _educationDal.GetByIdAsync(guid, tracking: false);
            if (entity == null)
            {
                return null;
            }
            return _mapper.Map<EducationDto>(entity);
        }

        public async Task<EducationDto?> UpdateAsync(UpdateEducationDto dto)
        {
            var entity = await _educationDal.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return null;
            }
            _mapper.Map(dto, entity);
            await _educationDal.UpdateAsync(entity);
            await _educationDal.SaveAsync();
            return _mapper.Map<EducationDto>(entity);
        }
    }
}
