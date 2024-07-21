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
    public interface IProductService
    {
        Task<ResultView<ProductCategorySpecificationsListDto>> Create(ProductWithSpecificationsDto productDto);

        Task<ResultView<GetProductSpecificationNameValueDtos>> GetOne(int id);

        Task<ResultView<ProductWithSpecificationsDto>> Update(ProductWithSpecificationsDto productDto);

        Task<ResultView<ProductCategorySpecificationsListDto>> SoftDelete(int productId);

        Task<ResultView<ProductCategorySpecificationsListDto>> HardDelete(int productId);

        Task<ResultDataList<GetAllProductsDtos>> GetAllPagination(int ItemsPerPage, int PageNumber);


        //sort

        Task<ResultDataList<GetAllProductsDtos>> SortProductsByDesending(int categoryId, int ItemsPerPage, int PageNumber);
        Task<ResultDataList<GetAllProductsDtos>> SortProductsByAscending(int categoryId, int ItemsPerPage, int PageNumber);
        

        //search
        Task<ResultDataList<GetAllProductsDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber);


        //filter
        Task<ResultDataList<GetAllProductsDtos>> FilterProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FiltertRelatedProducts(int productId, int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice, int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FilterNewlyAddedProducts(int count, int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FilterDiscountedProducts(int ItemsPerPage, int PageNumber);

        //Task<ResultDataList<CreateOrUpdateProductDtos>> FilterProductsByWarranty(string Warranty, int ItemsPerPage, int PageNumber);

        Task<ResultDataList<GetAllProductsDtos>> FilterProducts(FillterProductsDtos fillterProductsDto, int categoryId, int ItemsPerPage, int PageNumber);
        Task<List<string>> GetBrands( int categoryid);
        Task<List<string>> GetAllBrands();
        Task<ResultDataList<GetAllProductsDtos>> FilterNewlyAddedProducts(int count);
        Task<ResultDataList<GetAllProductsDtos>> FilterDiscountedProducts();

    }
}
