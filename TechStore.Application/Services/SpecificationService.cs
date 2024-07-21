using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IspecificationsRepository _specificationsRepository;
        private readonly ICategorySpecificationsRepository _categorySpecificationsRepository;
        private readonly IMapper _mapper;

        public SpecificationService(IspecificationsRepository specificationsRepository,ICategorySpecificationsRepository categorySpecificationsRepository, IMapper mapper) {
            _specificationsRepository = specificationsRepository;
            _categorySpecificationsRepository = categorySpecificationsRepository;
            _mapper = mapper;
        }
        public async Task<ResultView<SpecificationsDto>> Create(SpecificationsDto specificationsDto)
        {
            var ExistingSpecification = (await _specificationsRepository.GetAllAsync()).FirstOrDefault(s => s.Name == specificationsDto.Name);
            if (ExistingSpecification == null)
            {
                var SpecificationModel = _mapper.Map<Specification>(specificationsDto);
                var createdSpecification = await _specificationsRepository.CreateAsync(SpecificationModel);
                await _specificationsRepository.SaveChangesAsync();
                var createdSpecificationDto = _mapper.Map<SpecificationsDto>(createdSpecification);
                return new ResultView<SpecificationsDto>()
                {
                    Entity = createdSpecificationDto,
                    IsSuccess = true,
                    Message = "Specification Added Successfully"
                };
            }
            return new ResultView<SpecificationsDto>()
            {
                Entity = null,
                IsSuccess = false,
                Message = "Specification Already Existed"
            };
        }

        public async Task<ResultView<SpecificationsDto>> SoftDelete(int id)
        {
            var ExistingSpecification = await _specificationsRepository.GetByIdAsync(id);
            if(ExistingSpecification != null)
            {
                var ExistingSpecInCategory = ((await _categorySpecificationsRepository.GetAllAsync()).Where(cs=>cs.SpecificationId == id)).ToList();
                if(ExistingSpecInCategory.Count() == 0)
                {
                    ExistingSpecification.IsDeleted = true;
                    await _specificationsRepository.SaveChangesAsync();
                    var DeletedSpecificationDto = _mapper.Map<SpecificationsDto>(ExistingSpecification);
                    return new ResultView<SpecificationsDto>()
                    {
                        Entity = DeletedSpecificationDto,
                        IsSuccess = true,
                        Message = "Specification Deleted Successfully"
                    };
                }  
            }

            return new ResultView<SpecificationsDto>()
            {
                Entity = null,
                IsSuccess = false,
                Message = "Faild To Delete Specification ,It's Related To Category"
            };
        }

        public async Task<ResultDataList<SpecificationsDto>> GetAllPagination(int ItemsPerPage, int PageNumber)
        {
            var FilteredSpecification = (await _specificationsRepository.GetAllAsync()).Where(s => s.IsDeleted == false);
            var PaginatedSpecifications = FilteredSpecification
                                         .Skip(ItemsPerPage * (PageNumber - 1))
                                         .Take(ItemsPerPage)
                                         .Select(p => new SpecificationsDto
                                         {
                                            Id = p.Id,
                                            Name = p.Name,
                                         }).ToList();
            return new ResultDataList<SpecificationsDto>()
            {
                Entities = PaginatedSpecifications,
                Count = FilteredSpecification.Count()
            };
        }

        public async Task<ResultView<SpecificationsDto>> Update(SpecificationsDto specificationsDto)
        {
            var UpdatedSpecification = _mapper.Map<Specification>(specificationsDto);
            await _specificationsRepository.UpdateAsync(UpdatedSpecification);
            await _specificationsRepository.SaveChangesAsync();

            return new ResultView<SpecificationsDto>()
            {
                Entity = specificationsDto,
                IsSuccess = true,
                Message = "Specification Updated Successfully"
            };

        }

    }
}
