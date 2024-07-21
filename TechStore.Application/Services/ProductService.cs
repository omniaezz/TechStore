using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechStore.Application.Contract;
using TechStore.Dtos;
using TechStore.Dtos.ProductDtos;
using TechStore.Dtos.ViewResult;
using TechStore.Models;

namespace TechStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IproductCategorySpecifications _productCategorySpecifications;
        private readonly IspecificationsRepository _specificationsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(IProductRepository productRepository,IproductCategorySpecifications productCategorySpecifications,IspecificationsRepository specificationsRepository,ICategoryRepository categoryRepository,IMapper mapper,IWebHostEnvironment webHostEnvironment) {

            _productRepository = productRepository;
            _productCategorySpecifications = productCategorySpecifications;
            _specificationsRepository = specificationsRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }


        //images
        private async Task<List<string>> SaveProductImages(List<IFormFile> images)
        {
            var imagePaths = new List<string>();

            foreach (var image in images)
            {
                using var datastream = new MemoryStream();
                await image.CopyToAsync(datastream);
                var Img1Byts = datastream.ToArray();
                string img1Base64String = Convert.ToBase64String(Img1Byts);
                imagePaths.Add(img1Base64String);


            }

            return imagePaths;
        }


        //admin

        public async Task<ResultView<ProductCategorySpecificationsListDto>> Create(ProductWithSpecificationsDto productDto)
        {
            var OldProduct = (await _productRepository.GetAllAsync())
                             .Where(p => p.Description == productDto.Description).FirstOrDefault();
            if (OldProduct != null)
            {
                return new ResultView<ProductCategorySpecificationsListDto>
                {
                    Entity = null,
                    IsSuccess = false,
                    Message = "Product Already Exists !"
                };
            }

            var createorupdateproduct = new CreateOrUpdateProductDtos
            {
                CategoryId = productDto.CategoryId,
                Description = productDto.Description,
                ModelName = productDto.ModelName,
                Brand = productDto.Brand,
                DateAdded = productDto.DateAdded,
                DiscountValue = productDto.DiscountValue,
                Price = productDto.Price,
                Images = productDto.Images,
                UserId = productDto.UserId,
                Quantity = productDto.Quantity,
                Warranty = productDto.Warranty
            };

            var product = _mapper.Map<Product>(createorupdateproduct);


            var imagePaths = await SaveProductImages(productDto.Images);
            product.Images = new List<Image>(); 
            foreach (var imagePath in imagePaths)
            {
                product.Images.Add(new Image { Name = imagePath });
            }

            //add product
            var AddedProduct = await _productRepository.CreateAsync(product);
            await _productRepository.SaveChangesAsync();


            var list = new List<ProductCategorySpecifications>();

            foreach (var item in productDto.ProductCategorySpecifications)
            {
                var itemModel = _mapper.Map<ProductCategorySpecifications>(item);
                itemModel.ProductId = AddedProduct.Id;
                itemModel.CategoryId = AddedProduct.CategoryId;
                var res = await _productCategorySpecifications.CreateAsync(itemModel);
                //add specifications
                list.Add(res);
            }

            await _productCategorySpecifications.SaveChangesAsync();
            
            //map product & specifications
            var listDto = _mapper.Map<List<ProductCategorySpecificationsDto>>(list);
            var ProductCategorySpecificationsList = new ProductCategorySpecificationsListDto
            {
                CreateOrUpdateProductDtos = _mapper.Map<CreateOrUpdateProductDtos>(AddedProduct),
                ProductCategorySpecifications = listDto
            };

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = ProductCategorySpecificationsList,
                IsSuccess = true,
                Message = "Product Added Successfully !"
            };
        }
        
        public async Task<ResultView<GetProductSpecificationNameValueDtos>> GetOne(int id) //values only ?? new func for big table??
        {
            var ProductModel = await _productRepository.GetProductWithImages(id);
            if (ProductModel != null)
            {
                var productDto = _mapper.Map<GetAllProductsDtos>(ProductModel);
                productDto.Images = ProductModel.Images.Select(image => image.Name).ToList();


                var productCategorySpecificationsList = (await _productCategorySpecifications.GetProductCategorySpecifications(id)).ToList();

                var SpecList = new List<GetSpecificationsNameValueDtos>();

                foreach(var productCatSpec in productCategorySpecificationsList)
                {
                    var SpecName = await _specificationsRepository.GetSpecificationNameById((int)productCatSpec.SpecificationId);
                    SpecList.Add(new GetSpecificationsNameValueDtos { Name = SpecName, Value = productCatSpec.Value });
                }

                var getAll = new GetProductSpecificationNameValueDtos { productsDtos = productDto, specificationsNameValueDtos = SpecList };

                return new ResultView<GetProductSpecificationNameValueDtos>
                {
                    Entity = getAll,
                    IsSuccess = true,
                    Message = "Product Retrived Successfully !"
                };
            }

            return new ResultView<GetProductSpecificationNameValueDtos>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist !"
            };
        }

        public async Task<ResultView<ProductWithSpecificationsDto>> Update(ProductWithSpecificationsDto productDto)
        {

            var OldProduct = await _productRepository.GetByIdWithSpecificationsAsync(productDto.Id);

            await _productRepository.DetachEntityAsync(OldProduct);

            if (OldProduct != null)
            {
                var updatedProduct = _mapper.Map<Product>(productDto);
                foreach (var oldSpec in OldProduct.ProductCategorySpecifications)
                {
                   await _productCategorySpecifications.DeleteAsync(oldSpec);
                }

                foreach (var newSpec in updatedProduct.ProductCategorySpecifications)
                {
                    await _productCategorySpecifications.CreateAsync(newSpec);
                }
                await _productCategorySpecifications.SaveChangesAsync();

                var NewUpdatedProduct = await _productRepository.UpdateAsync(updatedProduct);
                await _productRepository.SaveChangesAsync();
                var NewUpdatedProductDto = _mapper.Map<ProductWithSpecificationsDto>(NewUpdatedProduct);

                return new ResultView<ProductWithSpecificationsDto>
                {
                    Entity =  NewUpdatedProductDto,
                    IsSuccess = true,
                    Message = "Product Updated Successfully !"
                };
            }

            return new ResultView<ProductWithSpecificationsDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product doesn't Exist & failed To Update !"
            };
        }

        public async Task<ResultView<ProductCategorySpecificationsListDto>> SoftDelete(int productId)
        {
            var OldProduct = (await _productRepository.GetByIdAsync(productId));
            if (OldProduct != null)
            {
                OldProduct.IsDeleted = true;
                await _productRepository.SaveChangesAsync();

                var ProductCatSpec = (await _productCategorySpecifications.GetProductCategorySpecifications(productId)).ToList();

                foreach(var productCategorySpec in ProductCatSpec)
                {
                    productCategorySpec.IsDeleted = true;
                }
                await _productCategorySpecifications.SaveChangesAsync();

                var DeletedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(OldProduct);
                var ProductCatSpecDtos = _mapper.Map<List<ProductCategorySpecificationsDto>>(ProductCatSpec);

                var ProductCatSpecListDtos = new ProductCategorySpecificationsListDto { CreateOrUpdateProductDtos = DeletedProductDto, ProductCategorySpecifications = ProductCatSpecDtos };

                return new ResultView<ProductCategorySpecificationsListDto>
                {
                    Entity = ProductCatSpecListDtos,
                    IsSuccess = true,
                    Message = "Product Deleted Successfully !"

                };
            }

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "failed to delete this product !"

            };

        }
        
        public async Task<ResultView<ProductCategorySpecificationsListDto>> HardDelete(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {

                var ProductCatSpec = (await _productCategorySpecifications.GetProductCategorySpecifications(productId)).ToList();

                var productCategorySpecificationsList = new List<ProductCategorySpecifications>();
                foreach (var productCatSpec in ProductCatSpec)
                {
                    var DeletedproductCategorySpecifications = await _productCategorySpecifications.DeleteAsync(productCatSpec);
                    productCategorySpecificationsList.Add(DeletedproductCategorySpecifications);
                }

                await _productCategorySpecifications.SaveChangesAsync();

                var DeletedProductModel = await _productRepository.DeleteAsync(product);
                await _productRepository.SaveChangesAsync();

                var DeletedProductDto = _mapper.Map<CreateOrUpdateProductDtos>(DeletedProductModel);
                var productCategorySpecificationsListtDtos = _mapper.Map<List<ProductCategorySpecificationsDto>>(productCategorySpecificationsList);

                var productCategorySpecificationsListDto = new ProductCategorySpecificationsListDto
                {
                    CreateOrUpdateProductDtos = DeletedProductDto,
                    ProductCategorySpecifications = productCategorySpecificationsListtDtos
                };

                return new ResultView<ProductCategorySpecificationsListDto>
                {
                        Entity = productCategorySpecificationsListDto,
                        IsSuccess = true,
                        Message = "Product Deleted Sucessfully"
                };
            }

            return new ResultView<ProductCategorySpecificationsListDto>
            {
                Entity = null,
                IsSuccess = false,
                Message = "Product Not Found"
            };
        }
                                                                        
        public async Task<ResultDataList<GetAllProductsDtos>> GetAllPagination(int ItemsPerPage, int PageNumber)
        {

            if (PageNumber > 0)
            {

                var products = (await _productRepository.GetAllAsync())
                               .Where(p => p.IsDeleted == false&&p.Quantity>0)
                               .Skip(ItemsPerPage * (PageNumber - 1))
                               .Take(ItemsPerPage)
                               .Select(p => new GetAllProductsDtos
                               {
                                   Id = p.Id,
                                   ModelName = p.ModelName,
                                   Description = p.Description,
                                   Brand = p.Brand,
                                   CategoryId = p.CategoryId,
                                   DateAdded = p.DateAdded,
                                   Price = p.Price,
                                   DiscountValue = p.DiscountValue,
                                   DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                   IsDeleted = p.IsDeleted,
                                   Ar_Description = p.Ar_Description,
                                   Ar_ModelName = p.Ar_ModelName,
                                   Images = p.Images.Select(i => i.Name).ToList()
                               }).ToList();

                var productscount = (await _productRepository.GetAllAsync())
                               .Where(p => p.IsDeleted == false).Count();
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = products,
                    Count = productscount
                };
                return resultDataList;
            }
            else
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }


        }




        //user
        public async Task<ResultDataList<GetAllProductsDtos>> FilterProductsByCategory(int categoryId, int ItemsPerPage, int PageNumber)
        {
            try
            {
                if (categoryId <= 0)
                {
                    throw new ArgumentException("Invalid CategoryId");
                }

                if (PageNumber <= 0)
                {
                    throw new ArgumentException("Page number must be greater than zero");
                }

                var products = (await _productRepository.GetProductsByCategory(categoryId))
                               .Where(p => p.IsDeleted == false && p.Quantity > 0)
                               .Skip(ItemsPerPage * (PageNumber - 1))
                               .Take(ItemsPerPage)
                               .Select(p => new GetAllProductsDtos
                               {
                                    Id = p.Id,
                                    ModelName = p.ModelName,
                                    Description = p.Description,
                                    Brand = p.Brand,
                                    CategoryId = p.CategoryId,
                                    DateAdded = p.DateAdded,
                                    Price = p.Price,
                                    DiscountValue = p.DiscountValue,
                                    DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                    IsDeleted = p.IsDeleted,
                                   Ar_Description = p.Ar_Description,
                                   Ar_ModelName = p.Ar_ModelName,
                                   Images = p.Images.Select(i => i.Name).ToList()

                               }).ToList();
                var totalCount = (await _productRepository.GetProductsByCategory(categoryId))
                                .Where(p => !p.IsDeleted)
                                .Count();

                var ProductsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
             
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = ProductsDto,
                    Count = totalCount
                };
                return resultDataList;
            }
            catch (Exception ex)
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }

        }


        //sort 
        public async Task<ResultDataList<GetAllProductsDtos>> SortProductsByDesending(int categoryId, int ItemsPerPage, int PageNumber)
        {
            var products = (await _productRepository.GetProductsByDescending(categoryId))
                            .Where(p => p.IsDeleted == false && p.Quantity > 0)
                            .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                            .Select(p => new GetAllProductsDtos
                            {
                                Id = p.Id,
                                ModelName = p.ModelName,
                                Description = p.Description,
                                Brand = p.Brand,
                                CategoryId = p.CategoryId,
                                DateAdded = p.DateAdded,
                                Price = p.Price,
                                DiscountValue = p.DiscountValue,
                                DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                IsDeleted = p.IsDeleted,
                                Ar_Description = p.Ar_Description,
                                Ar_ModelName = p.Ar_ModelName,
                                Images = p.Images.Select(i => i.Name).ToList()
                            }).ToList();
            var totalcount = (await _productRepository.GetProductsByDescending(categoryId))
                          .Where(p => p.IsDeleted == false).Count();
            var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
            ResultDataList<GetAllProductsDtos> res;
            if(products != null)
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = totalcount
                };
            }
            else
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
            }
            return res;
        }

        public async Task<ResultDataList<GetAllProductsDtos>> SortProductsByAscending(int categoryId,int ItemsPerPage, int PageNumber)
        {
            var products = (await _productRepository.GetProductsByAscending(categoryId))
                            .Where(p => p.IsDeleted == false && p.Quantity > 0)
                            .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                            .Select(p => new GetAllProductsDtos
                            {
                                Id = p.Id,
                                ModelName = p.ModelName,
                                Description = p.Description,
                                Brand = p.Brand,
                                CategoryId = p.CategoryId,
                                DateAdded = p.DateAdded,
                                Price = p.Price,
                                DiscountValue = p.DiscountValue,
                                DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                IsDeleted = p.IsDeleted,
                                Ar_Description = p.Ar_Description,
                                Ar_ModelName = p.Ar_ModelName,
                                Images = p.Images.Select(i => i.Name).ToList()
                            }).ToList();
            var totalcount = (await _productRepository.GetProductsByAscending(categoryId))
                            .Where(p => p.IsDeleted == false).Count();
            var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
            ResultDataList<GetAllProductsDtos> res;
            if(products != null)
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = totalcount
                };
            }
            else
            {
                res = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
            }

            
            return res;
        }


        //search
        public async Task<ResultDataList<GetAllProductsDtos>> SearchProduct(string Name, int ItemsPerPage, int PageNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    throw new ArgumentException("Name cannot be empty or whitespace.");
                }

                if (PageNumber <= 0)
                {
                    throw new ArgumentException("Page number must be greater than zero");
                }

                var products = (await _productRepository.SearchProduct(Name))
                               .Where(p => p.IsDeleted == false && p.Quantity > 0)
                               .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                               .Select(p => new GetAllProductsDtos
                               {
                                    Id = p.Id,
                                    ModelName = p.ModelName,
                                    Description = p.Description,
                                    Brand = p.Brand,
                                    CategoryId = p.CategoryId,
                                    DateAdded = p.DateAdded,
                                    Price = p.Price,
                                    DiscountValue = p.DiscountValue,
                                    DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                    IsDeleted = p.IsDeleted,
                                   Ar_Description = p.Ar_Description,
                                   Ar_ModelName = p.Ar_ModelName,
                                   Images = p.Images.Select(i => i.Name).ToList()
                               }).ToList();
                var totalCount = (await _productRepository.SearchProduct(Name))
                               .Where(p => !p.IsDeleted)
                               .Count();

                var ProductsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = ProductsDto,
                    Count = totalCount
                };
                return resultDataList;
            }
            catch(Exception ex)
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }
            
        }


        //filter
        public async Task<ResultDataList<GetAllProductsDtos>> FilterProducts(FillterProductsDtos fillterProductsDto, int categoryId, int ItemsPerPage, int PageNumber)
        {
            var products = (await _productRepository.FilterProducts(fillterProductsDto,categoryId))
                            .Where(p => p.IsDeleted == false && p.Quantity > 0)
                            .Skip(ItemsPerPage * (PageNumber - 1)).Take(ItemsPerPage)
                            .Select(p => new GetAllProductsDtos
                            {
                                Id = p.Id,
                                ModelName = p.ModelName,
                                Description = p.Description,
                                Brand = p.Brand,
                                CategoryId = p.CategoryId,
                                DateAdded = p.DateAdded,
                                Price = p.Price,
                                DiscountValue = p.DiscountValue,
                                DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                IsDeleted = p.IsDeleted,
                                Ar_Description = p.Ar_Description,
                                Ar_ModelName = p.Ar_ModelName,
                                Images = p.Images.Select(i => i.Name).ToList()
                            }).ToList();
            var totalcount = (await _productRepository.FilterProducts(fillterProductsDto, categoryId))
                          .Where(p => p.IsDeleted == false).Count();
            var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
            ResultDataList<GetAllProductsDtos> resultDataList;
            if(products != null)
            {
                resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = totalcount
                };
            }
            else
            {
                resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
            }
            
            return resultDataList;
        }
       
        public async Task<List<string>> GetBrands(int categoryid)
        {
            var brands = await _productRepository.GetBrands(categoryid);
            return brands;
        }

        public async Task<List<string>> GetAllBrands()
        {
            var brands = await _productRepository.GetAllBrands();
            return brands.ToList();
        }

        public async Task<ResultDataList<GetAllProductsDtos>> FilterNewlyAddedProducts(int count)
        {
            try
            {
                if (count <= 0)
                {
                    throw new ArgumentException("The count must be greater than zero");
                }


                var products = (await _productRepository.GetNewlyAddedProducts(count))
                               .Where(p => p.IsDeleted == false && p.Quantity > 0)
                               .Select(p => new GetAllProductsDtos
                               {
                                   Id = p.Id,
                                   ModelName = p.ModelName,
                                   Description = p.Description,
                                   Brand = p.Brand,
                                   CategoryId = p.CategoryId,
                                   DateAdded = p.DateAdded,
                                   Price = p.Price,
                                   Quantity =p.Quantity,
                                   DiscountValue = p.DiscountValue,
                                   DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                   IsDeleted = p.IsDeleted,
                                   Ar_Description = p.Ar_Description,
                                   Ar_ModelName = p.Ar_ModelName,
                                   Images = p.Images.Select(i => i.Name).ToList()

                               }).ToList();

                var ProductsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
                var resultDataLists = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = ProductsDto,
                    Count = ProductsDto.Count()
                };
                return resultDataLists;
            }
            catch (Exception ex)
            {
                var resultDataLists = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataLists;
            }


        }


        public async Task<ResultDataList<GetAllProductsDtos>> FilterDiscountedProducts()
        {
            try
            {
              
                var products = (await _productRepository.GetDiscountedProducts())
                               .Where(p => p.IsDeleted == false && p.Quantity > 0)
                                .Select(p => new GetAllProductsDtos
                                {
                                    Id = p.Id,
                                    ModelName = p.ModelName,
                                    Description = p.Description,
                                    Brand = p.Brand,
                                    CategoryId = p.CategoryId,
                                    DateAdded = p.DateAdded,
                                    Price = p.Price,
                                    Quantity = p.Quantity,
                                    DiscountValue = p.DiscountValue,
                                    DiscountedPrice = p.Price - (p.Price * p.DiscountValue / 100),
                                    IsDeleted = p.IsDeleted,
                                    Ar_Description = p.Ar_Description,
                                    Ar_ModelName = p.Ar_ModelName,
                                    Images = p.Images.Select(i => i.Name).ToList()

                                }).ToList();

                if (products is null)
                {
                    throw new ArgumentException("No discounted products found");
                }

                var productsDto = _mapper.Map<List<GetAllProductsDtos>>(products);
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = productsDto,
                    Count = productsDto.Count()
                };
                return resultDataList;
            }
            catch (Exception ex)
            {
                var resultDataList = new ResultDataList<GetAllProductsDtos>()
                {
                    Entities = null,
                    Count = 0
                };
                return resultDataList;
            }

        }

    }
}
