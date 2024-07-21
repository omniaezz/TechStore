using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.CategoryDtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IspecificationsRepository _specificationsRepository;
        private readonly ICategorySpecificationsRepository _categorySpecificationsRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository,IspecificationsRepository specificationsRepository,ICategorySpecificationsRepository categorySpecificationsRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _specificationsRepository = specificationsRepository;
            _categorySpecificationsRepository = categorySpecificationsRepository;
            _mapper = mapper;
        }

        public async Task<ResultView<CategorySpecificationDto>> CreateCategory(CategoryDto category, List<SpecificationsDto> specificationsDtos)
        {
            var allcategories = await _categoryRepository.GetAllAsync();
            var oldcat = allcategories.Where(c => c.Name == category.Name).FirstOrDefault();
            if (oldcat != null)
            {
                return new ResultView<CategorySpecificationDto> { Entity = null, IsSuccess = false, Message = "Category Already Exists" };
            }
            else
            {
                List<Specification> SpecLists;
                var cat = _mapper.Map<Category>(category);
                var newCat = await _categoryRepository.CreateAsync(cat);
                await _categoryRepository.SaveChangesAsync();

                foreach (var specificationn in specificationsDtos)
                {
                    var specificationModel = _mapper.Map<Specification>(specificationn);
                    var ExistingSpec = await _specificationsRepository.SearchByName(specificationModel.Name);
                    if(ExistingSpec == null)
                    {
                        var CreatedSpc =  await _specificationsRepository.CreateAsync(specificationModel);
                        await _specificationsRepository.SaveChangesAsync();
                        await _categorySpecificationsRepository.CreateAsync(new CategorySpecifications { CategoryId = newCat.Id, SpecificationId = CreatedSpc.Id });
                    }
                    else
                    {
                        await _categorySpecificationsRepository.CreateAsync(new CategorySpecifications { CategoryId = newCat.Id, SpecificationId = ExistingSpec.Id });
                    }
                }
                
                await _categorySpecificationsRepository.SaveChangesAsync();
                List<Specification>  SpecList = (await _specificationsRepository.GetSpecificationsByCategory(newCat.Id)).ToList();

                var catDto = _mapper.Map<CategoryDto>(newCat);
                var SpecListDto = _mapper.Map<List<SpecificationsDto>>(SpecList);

                var CategorySpecificationDto = new CategorySpecificationDto { Category = catDto, SpecificationsDtos = SpecListDto };
                return new ResultView<CategorySpecificationDto> { Entity = CategorySpecificationDto, IsSuccess = true, Message = "Created Successfully" };
            }
        }


        public async Task<ResultView<CategorySpecificationDto>> UpdateCategory(CategoryDto updatedcategory, List<SpecificationsDto> specificationsDtos)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(updatedcategory.Id);
            await _categoryRepository.DetachEntityAsync(existingCategory);

            if (existingCategory == null)
            {
                return new ResultView<CategorySpecificationDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Category not found"
                };
            }

            //updateCategory
            var updatedcategoryModel = _mapper.Map<Category>(updatedcategory);
            var UpdatedCategory = await _categoryRepository.UpdateAsync(updatedcategoryModel);
            await _categoryRepository.SaveChangesAsync();

            //GetAllCatSpec
            var categorySpecifications = ((await _categorySpecificationsRepository.GetAllAsync())
                                        .Where(cs => cs.CategoryId == updatedcategory.Id)).ToList();

            //delete old catspec
            foreach (var RemoveCateSpec in categorySpecifications)
            {
                await _categorySpecificationsRepository.DeleteAsync(RemoveCateSpec);
            }

            //add new catspec
            foreach (var specificationDto in specificationsDtos)
            {
                var categorySpec = new CategorySpecifications
                {
                    CategoryId = existingCategory.Id,
                    SpecificationId = specificationDto.Id
                };
                await _categorySpecificationsRepository.CreateAsync(categorySpec);
            }

            await _categorySpecificationsRepository.SaveChangesAsync();

            var catDto = _mapper.Map<CategoryDto>(UpdatedCategory);
            var CategorySpecifications = new CategorySpecificationDto
            {
                Category = catDto,
                SpecificationsDtos = specificationsDtos
            };

            return new ResultView<CategorySpecificationDto>
            {
                Entity = CategorySpecifications,
                IsSuccess = true,
                Message = "Category & Specifications Updated Successfully"
            };
        }



        public async Task<ResultView<CategorySpecificationDto>> DeleteSpecFromCategory(int CategoryId,int SpecID)
        {
            //update spec
            var Specfication = await _categorySpecificationsRepository.GetSpecByCategoryAndSpecId(CategoryId, SpecID);
            var DeletedSpecfication = await _categorySpecificationsRepository.DeleteAsync(Specfication);
            await _categorySpecificationsRepository.SaveChangesAsync();
            var DeletedSpecDto = _mapper.Map<CategorySpecificationDto>(DeletedSpecfication);
            var category = await _categoryRepository.GetByIdAsync(CategoryId);
            var SpecList = await _specificationsRepository.GetSpecificationsByCategory(CategoryId);
            var CategoryDto = _mapper.Map<CategoryDto>(category);
            var SpecListDto = _mapper.Map<List<SpecificationsDto>>(SpecList);

            var CategorySpec = new CategorySpecificationDto { Category = CategoryDto, SpecificationsDtos = SpecListDto };
            return new ResultView<CategorySpecificationDto> { Entity = CategorySpec, IsSuccess = true, Message = "Deleted Successfully" };
        }

        public async Task<ResultView<CategorySpecificationDto>> AddSpecToCategory(int CategoryId, SpecificationsDto specificationsDto)
        {
            var ExistingCategory = await _categoryRepository.GetByIdAsync(CategoryId);
            if(ExistingCategory == null)
            {
                return new ResultView<CategorySpecificationDto> { Entity = null, IsSuccess = false, Message = "Category Doesn't Exist" };
            }

            var ExistingCategoryDto = _mapper.Map<CategoryDto>(ExistingCategory);
            var SpecModel = _mapper.Map<Specification>(specificationsDto);

            var ExistingSpecification = await _specificationsRepository.SearchByName(specificationsDto.Name);

            if(ExistingSpecification == null) 
            {
                var AddedSpec = await _specificationsRepository.CreateAsync(SpecModel);
                await _specificationsRepository.SaveChangesAsync();
                await _categorySpecificationsRepository.CreateAsync(new CategorySpecifications { CategoryId = ExistingCategory.Id , SpecificationId = AddedSpec.Id });
            }
            else
            {
                await _categorySpecificationsRepository.CreateAsync(new CategorySpecifications { CategoryId = ExistingCategory.Id, SpecificationId = ExistingSpecification.Id });
            }

            await _categorySpecificationsRepository.SaveChangesAsync();

            var SpecList = await _specificationsRepository.GetSpecificationsByCategory(CategoryId);
            var SpecListDto = _mapper.Map<List<SpecificationsDto>>(SpecList);

            var Result = new CategorySpecificationDto { Category = ExistingCategoryDto, SpecificationsDtos = SpecListDto };

            return new ResultView<CategorySpecificationDto> { Entity = Result, IsSuccess = true, Message = "CategoryAndSpecifications Retrived Successfully" };

        }

        public async Task<ResultView<CategoryDto>> HardDeleteCategory(CategoryDto category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync((int)category.Id);
                var mCat = _mapper.Map<Category>(existingCategory);
                var oldCat = _categoryRepository.DeleteAsync(mCat);
                await _categoryRepository.SaveChangesAsync();
                var catDto = _mapper.Map<CategoryDto>(oldCat);
                return new ResultView<CategoryDto> { Entity = catDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }
        
        public async Task<ResultView<CategoryDto>> SoftDeleteCategory(CategoryDto category)
        {
            try
            {
                var mCat = _mapper.Map<Category>(category);
                var oldCat = (await _categoryRepository.GetAllAsync()).FirstOrDefault(b => b.Id == category.Id);
                oldCat.IsDeleted = true;
                await _categoryRepository.SaveChangesAsync();

                var catDto = _mapper.Map<CategoryDto>(oldCat);
                return new ResultView<CategoryDto> { Entity = catDto, IsSuccess = true, Message = "Deleted Successfully" };
            }
            catch (Exception ex)
            {
                return new ResultView<CategoryDto> { Entity = null, IsSuccess = false, Message = ex.Message };

            }
        }

        public async Task<ResultDataList<CategoryDto>> GetAllCategory()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var catDtos = categories.Where(c=>c.IsDeleted == false).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            ResultDataList<CategoryDto> result = new ResultDataList<CategoryDto>();
            result.Entities = catDtos;
            result.Count = catDtos.Count();
            return result;
        }

        public async Task<CategorySpecificationDto> GetCategoryById(int id)
        {
            var cat = await _categoryRepository.GetByIdAsync(id);
            var spec = await _specificationsRepository.GetSpecificationsByCategory(id);

            var catDto = _mapper.Map<CategoryDto>(cat);
            var SpecDto = _mapper.Map<List<SpecificationsDto>>(spec);
            var cateSpec = new CategorySpecificationDto { Category = catDto , SpecificationsDtos = SpecDto };
            return cateSpec;
        }

        public async Task<CategoryDto> GetCategoryByName(string name)
        {
            var cat = await _categoryRepository.SearchByName(name);
            var catDto = _mapper.Map<CategoryDto>(cat);
            return catDto;

        }

        public async Task<ResultDataList<SpecificationsDto>> GetSpecificationsByCategoryId(int CategoryId)
        {
            var Specifications = await _specificationsRepository.GetSpecificationsByCategory(CategoryId);
            var SpecificationsDto = _mapper.Map<List<SpecificationsDto>>(Specifications);
            if(Specifications == null)
            {
                return new ResultDataList<SpecificationsDto> { Entities = null , Count = SpecificationsDto.Count };
            }

            return new ResultDataList<SpecificationsDto> { Entities = SpecificationsDto , Count = SpecificationsDto.Count };
        }
    }
}
