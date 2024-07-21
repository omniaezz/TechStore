using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {

        public ProductRepository(TechStoreContext techStoreContext) : base(techStoreContext) { }



        public async Task<Product> GetProductWithImages(int id)
        {
            var product = await _entities.Where(p => p.Id == id)
                .Include(prd => prd.Images)
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<IQueryable<Product>> GetDiscountedProducts()
        {
            return await Task.FromResult(_entities.Where(p => p.DiscountValue > 0).OrderByDescending(p=>p.DiscountValue));
        }

        public async Task DetachEntityAsync(Product entity)
        {
            if (_context.Entry(entity).State != EntityState.Detached)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }

        public Task<IQueryable<Product>> GetNewlyAddedProducts(int count)
        {
            return Task.FromResult(_entities.OrderByDescending(p => p.DateAdded).Take(count));
        }

        public Task<IQueryable<Product>> GetProductsByCategory(int categoryId)
        {
            return Task.FromResult(_entities.Where(p => p.CategoryId == categoryId));
        }


        public Task<IQueryable<Product>> GetRelatedProducts(Product product)
        {
            return Task.FromResult(_entities.Where(p => p.CategoryId == product.CategoryId || p.Id != product.Id));
        }



        public Task<IQueryable<Product>> SearchProduct(string Name)
        {
            Name = Name.ToLower();
            return Task.FromResult(_entities.Where(p => p.ModelName.ToLower().Contains(Name) ||
                                                   p.Description.ToLower().Contains(Name) ||
                                                   p.Brand.ToLower().Contains(Name)));
        }

        public Task<IQueryable<Product>> GetProductsByWarranty(string Warranty)
        {
            return Task.FromResult(_entities.Where(p => p.Warranty == Warranty));
        }

        public async Task<IQueryable<Product>> GetProductsByDescending(int categoryId)
        {
            return await Task.FromResult(_entities.OrderByDescending(p => p.Price).Where(p=>p.CategoryId==categoryId));
        }

        public async Task<IQueryable<Product>> GetProductsByAscending(int categoryId)
        {
            return await Task.FromResult(_entities.OrderBy(p => p.Price).Where(p => p.CategoryId == categoryId));
        }

        public async Task<IQueryable<Product>> FilterProducts(FillterProductsDtos criteria,int categoryId)
        {
            IQueryable<Product> query = _entities.Where(p=>p.CategoryId== categoryId);

            if (criteria.Brand != null && criteria.Brand.Any())
            {
                query = query.Where(p => criteria.Brand.Contains(p.Brand));
            }

            // Filter by warranty if specified
            if (criteria.Warranty != null && criteria.Warranty.Any())
            {
                query = query.Where(p => criteria.Warranty.Contains(p.Warranty));
            }


            if (criteria.DiscountValue != null && criteria.DiscountValue.HasValue)
            {
                query = query.Where(p => p.DiscountValue == criteria.DiscountValue);
            }

            if (criteria.MinPrice != null && criteria.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.MinPrice.Value);
            }

            if (criteria.MaxPrice != null && criteria.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
            }


            return await Task.FromResult(query);
        }

        public async Task<List<string>> GetBrands(int categoryid)
        {
            var brands = await _context.Products
                .Where(p => p.Brand != null&&p.CategoryId== categoryid)
                .Select(p => p.Brand)
                .Distinct()
                .ToListAsync();

            return brands;
        }

        public async Task<IQueryable<Image>> GetImagesByProductId(int ProductId){

            var images = _context.Products
                        .Where(p => p.Id == ProductId)
                        .SelectMany(p => p.Images);
            return images;
        }

            
        public async Task<IQueryable<string>> GetAllBrands()
        {
            return _entities.Select(b => b.Brand);
        }

        public async Task<Product> GetByIdWithSpecificationsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductCategorySpecifications)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
