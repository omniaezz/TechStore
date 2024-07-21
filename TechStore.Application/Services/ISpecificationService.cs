using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;

namespace TechStore.Application.Services
{
    public interface ISpecificationService
    {
        Task<ResultView<SpecificationsDto>> Create(SpecificationsDto specificationsDto);
        Task<ResultView<SpecificationsDto>> SoftDelete(int id);
        Task<ResultView<SpecificationsDto>> Update(SpecificationsDto specificationsDto);
        Task<ResultDataList<SpecificationsDto>> GetAllPagination(int ItemsPerPage, int PageNumber);
    }
}
