using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using TechStore.Dtos.ProductDtos;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class specificationController : Controller
    {
        private readonly ISpecificationService _specificationService;

        public specificationController(ISpecificationService specificationService)
        {
            _specificationService = specificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecification(SpecificationsDto specificationsDto)
        {
            var CreatedSpecification = await _specificationService.Create(specificationsDto);
            if(ModelState.IsValid)
            {
                return Ok(CreatedSpecification);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSpecification(SpecificationsDto specificationsDto)
        {
            var UpdatedSpecification = await _specificationService.Update(specificationsDto);
            if (ModelState.IsValid)
            {
                return Ok(UpdatedSpecification);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSpecification(int id)
        {
            var DeletedSpecification = await _specificationService.SoftDelete(id);
            if (ModelState.IsValid)
            {
                return Ok(DeletedSpecification);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecifications(int ItemsPerPage, int PageNumber)
        {
            var Specifications = await _specificationService.GetAllPagination(ItemsPerPage, PageNumber);
            if (ModelState.IsValid)
            {
                return Ok(Specifications);
            }
            return BadRequest(ModelState);
        }
    }
}
