using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductSizes;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json.Serialization.Metadata;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductService(EShopDbContext context, IStorageService storage)
        {
            _context = context;
            _storageService = storage;
        }

        public async Task<ApiResult<int>> AddImage(int productId, ProductImageCreateRequest request)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new ApiErrorResult<int>($"Cannot find a product: {productId}!");

            var productImage = new ProductImage()
            {
                DateCreated = DateTime.Now,
                ProductId = productId,
                Caption = request.Caption,
                IsDefault = request.IsDefault,
                SortOrder = request.SortOrder
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
                return new ApiSuccessResult<int>(productImage.Id);
            return new ApiErrorResult<int>($"Add Image Unsuccessful For Product ID {productId} !");
        }

        public async Task<ApiResult<bool>> AddViewcount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new ApiErrorResult<bool>($"Cannot find a product: {productId}!");
            product.ViewCount += 1;
            int result = await _context.SaveChangesAsync();
            if (result > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Add View Count Successful For Product Id {productId}!");
            return new ApiErrorResult<bool>($"Add View Count Unsuccessful For Product Id {productId}!");
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var product = await _context.Products
    .AsNoTracking() // Loại bỏ tracking để không vô tình thay đổi Product
    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return new ApiErrorResult<bool>($"Cannot find a product: {id}!");
            }

            // Xử lý việc assign category
            foreach (var category in request.Categories)
            {
                var productInCategory = await _context.ProductInCategories
                    .FirstOrDefaultAsync(x => x.CategoryId == int.Parse(category.Id) && x.ProductId == id);

                if (!category.IsSelected)
                {
                    if (productInCategory != null)
                        _context.ProductInCategories.Remove(productInCategory);
                }
                else
                {
                    if (productInCategory == null)
                    {
                        await _context.ProductInCategories.AddAsync(new ProductInCategory()
                        {
                            CategoryId = int.Parse(category.Id),
                            ProductId = id
                        });
                    }
                }

            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>().CreateMessage("Update category Successful!");
        }

        public async Task<ApiResult<int>> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                BrandId = request.BrandId,
                Stock = 0,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
        {
            new ProductTranslation()
            {
                Name =  request.Name,
                Description = request.Description,
                Details = request.Details,
                SeoDescription = request.SeoDescription,
                SeoAlias = request.SeoAlias,
                SeoTitle = request.SeoTitle,
                LanguageId = request.LanguageId
            }
        }
            };

            if (request.ThumbnailImage != null)
            {
                product.productImages = new List<ProductImage>()
        {
            new ProductImage()
            {
                Caption = "Thumbnail image",
                DateCreated = DateTime.Now,
                FileSize = request.ThumbnailImage.Length,
                ImagePath = await this.SaveFile(request.ThumbnailImage),
                IsDefault = true,
                SortOrder = 1
            }
        };
            }

            // Lưu product trước để lấy Id
            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // Lưu trước để product.Id được tạo

            // Thêm các ProductSizes với ProductId
            for (int i = 1; i <= 6; i++)
            {
                _context.ProductSizes.Add(new ProductSize()
                {
                    SizeId = i,
                    ProductId = product.Id, // Sử dụng Id đã được tạo
                    Quantity = 0
                });
            }

            // Lưu các ProductSizes
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<int>(product.Id)
                    .CreateMessage($"Create Successful Product Name {request.Name}");

            return new ApiErrorResult<int>($"Create Unsuccessful Product Name {request.Name}!");
        }

        public async Task<ApiResult<bool>> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                new ApiErrorResult<bool>($"Cannot find a product: {productId}");

            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _context.Products.Remove(product);

            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Delete Successful Product ID {productId}!");
            return new ApiErrorResult<bool>($"Delete Unsuccessful Product ID {productId}");
        }

        public async Task<ApiResult<List<ProductViewModel>>> GetAll(string languageId)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join br in _context.Brands on p.BrandId equals br.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, br };

            var data = await query.Select(x => new ProductViewModel()
            {
                Id = x.p.Id,
                Name = x.pt.Name,
                BrandId = x.br.Id,
                BrandName = x.br.Name,
                DateCreated = x.p.DateCreated,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = languageId,
                OriginalPrice = x.p.OriginalPrice,
                Price = x.p.Price,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount
            }).ToListAsync();


            return new ApiSuccessResult<List<ProductViewModel>>(data);
        }


        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByCatagoryId(string LanguageId, GetPublicProductPagingRequest request)
        {

            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join br in _context.Brands on p.BrandId equals br.Id
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == LanguageId
                        select new { p, pt, br, pic };
            //2. filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    BrandId = x.br.Id,
                    BrandName = x.br.Name,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };

            return new ApiSuccessResult<PagedResult<ProductViewModel>>(pagedResult);
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAllPagingByKeywordAndatagoryId(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join br in _context.Brands on p.BrandId equals br.Id
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId into ppic
                        from pic in ppic.DefaultIfEmpty()
                        join c in _context.Categories on pic.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, br, pic };

            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            if (request.CategoryIds != null && request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    BrandId = x.br.Id,
                    BrandName = x.br.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();


            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<ProductViewModel>>(pagedResult);
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var brand = await _context.Brands.FindAsync(product.BrandId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);

            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId into picct
                                    from pic in picct.DefaultIfEmpty()
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                BrandId = (int)product.BrandId,
                BrandName = brand.Name,
                DateCreated = product.DateCreated,
                Description = productTranslation != null ? productTranslation.Description : "null",
                LanguageId = languageId,
                Details = productTranslation != null ? productTranslation.Details : "null",
                Name = productTranslation != null ? productTranslation.Name : "null",
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : "null",
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : "null",
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : "null",
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                Categories = categories != null ? categories : new List<string>()
            };

            return new ApiSuccessResult<ProductViewModel>(productViewModel);
        }

        public async Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null) new ApiErrorResult<ProductImageViewModel>($"Cannot find a product image: {imageId}");

            var Image = new ProductImageViewModel()
            {
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                SortOrder = productImage.SortOrder,
                FileSize = productImage.FileSize,
                Id = imageId,
                ImagePath = productImage.ImagePath,
                IsDefault = productImage.IsDefault,
                ProductId = productImage.ProductId,
            };
            return new ApiSuccessResult<ProductImageViewModel>(Image);
        }

        public async Task<ApiResult<List<ProductImageViewModel>>> GetListImages(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                new ApiErrorResult<ProductViewModel>($"Cannot find a product: {productId}");
            return new ApiSuccessResult<List<ProductImageViewModel>>(await _context.ProductImages.Where(x => x.ProductId == productId)
            .Select(i => new ProductImageViewModel
            {
                Caption = i.Caption,
                DateCreated = DateTime.Now,
                FileSize = i.FileSize,
                Id = i.Id,
                IsDefault = i.IsDefault,
                ImagePath = i.ImagePath,
                ProductId = i.ProductId,
                SortOrder = i.SortOrder

            }).ToListAsync());
        }

        public async Task<ApiResult<List<ProductSizeViewModel>>> GetQuantity(int Id)
        {
            var productSizes = await (from PS in _context.ProductSizes
                                      join S in _context.Sizes on PS.SizeId equals S.Id
                                      where PS.ProductId == Id
                                      select (new ProductSizeViewModel
                                      {
                                          Id = S.Id,
                                          Name = S.Name,
                                          Quantity = PS.Quantity
                                      })).ToListAsync();

            return new ApiSuccessResult<List<ProductSizeViewModel>>(productSizes);
        }

        public async Task<ApiResult<bool>> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.Where(x => x.Id == imageId).FirstOrDefaultAsync();
            if (productImage == null)
                new ApiErrorResult<bool>($"Cannot find a product image: {imageId}");
            _context.ProductImages.Remove(productImage);

            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Delete Successful Product Image ID {imageId}!");
            return new ApiErrorResult<bool>($"Delete UnSuccessful Product Image ID {imageId}");

        }

        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);

            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(
                x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);

            if (product == null || productTranslation == null)
                return new ApiErrorResult<bool>($"Cannot find a product: {request.Id}");

            product.Price = request.Price;
            product.OriginalPrice = request.OriginalPrice;
            productTranslation.Name = request.Name;
            productTranslation.Description = request.Description;
            productTranslation.Details = request.Details;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;


            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Update Successful For Product ID {request.Id}!");
            return new ApiErrorResult<bool>($"Update UnSuccessful For Product ID {request.Id}!");
        }

        public async Task<ApiResult<bool>> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);

            if (productImage == null)
                return new ApiErrorResult<bool>($"Cannot find a product image: {imageId}");

            if (request.ImageFile != null)
            {
                productImage.Caption = request.Caption;
                productImage.SortOrder = request.SortOrder;
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Update Successful For Product Image ID {imageId}!");
            return new ApiErrorResult<bool>($"Update UnSuccessful For Product Image ID {imageId}!");
        }

        public async Task<ApiResult<bool>> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new ApiErrorResult<bool>($"Cannot find a product: {productId}");
            product.Price = newPrice;
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Update Price Successful For Product ID {productId}!");
            return new ApiErrorResult<bool>($"Update Price UnSuccessful For Product Image ID {productId}!");
        }

        public async Task<ApiResult<bool>> UpdateQuantity(UpdateQuantityRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return new ApiErrorResult<bool>($"Cannot find a product: {request.ProductId}");
            int quantity = 0;
            for (int i = 1; i <= 6; i++)
            {
               var size =  await _context.ProductSizes.FindAsync(request.ProductId, i);
                size.Quantity = request.ProductSizes.Where(x=>x.Id==i).Select(request=>request.Quantity).Sum(); 
                quantity+= size.Quantity;
            }
            product.Stock = quantity;

            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Update Quantity Successful For Product ID {request.ProductId}!");
            return new ApiErrorResult<bool>($"Update Quantity UnSuccessful For Product Image ID {request.ProductId}!");
        }

        public async Task<ApiResult<bool>> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new ApiErrorResult<bool>($"Cannot find a product: {productId}");
            product.Stock += addedQuantity;
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Update Stock Successful For Product ID {productId}!");
            return new ApiErrorResult<bool>($"Update Price UnSuccessful For Product Image ID {productId}!");
        }


        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }


    }

}
