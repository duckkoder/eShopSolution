using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Net.Http.Headers;

namespace eShopSolution.Application.Catalog.Products
{
	public class ManageProductService : IManageProductService
	{
		private readonly EShopDbContext _context;
		private readonly IStorageService _storageService;
		public ManageProductService(EShopDbContext context, IStorageService storage)
		{
			_context = context;
			_storageService = storage;
		}

		public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new EShopException($"Cannot find a product: {productId}");

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
			await _context.SaveChangesAsync();
			return productImage.Id;
		}

		public async Task<int> AddViewcount(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			product.ViewCount += 1;
			return await _context.SaveChangesAsync();

		}

		public async Task<int> Create(ProductCreateRequest request)
		{
			var product = new Product()
			{
				Price = request.Price,
				OriginalPrice = request.OriginalPrice,
				Stock = request.Stock,
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

			_context.Products.Add(product);
			await _context.SaveChangesAsync();
			return product.Id;
		}

		public async Task<int> Delete(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new EShopException($"Cannot find a product: {productId}");

			var images = _context.ProductImages.Where(i => i.ProductId == productId);
			foreach (var image in images)
			{
				await _storageService.DeleteFileAsync(image.ImagePath);
			}

			_context.Products.Remove(product);

			return await _context.SaveChangesAsync();
		}

		public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
		{
			//1. Select join
			var query = from p in _context.Products
						join pt in _context.ProductTranslations on p.Id equals pt.ProductId
						join pic in _context.ProductInCategories on p.Id equals pic.ProductId
						join c in _context.Categories on pic.CategoryId equals c.Id
						select new { p, pt, pic };
			//2. filter
			if (!string.IsNullOrEmpty(request.Keyword))
				query = query.Where(x => x.pt.Name.Contains(request.Keyword));

			if (request.CategoryIds.Count > 0)
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
				Items = data
			};
			return pagedResult;
		}

		public async Task<ProductViewModel> GetById(int productId, string languageId)
		{
			var product = await _context.Products.FindAsync(productId);
			var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
			&& x.LanguageId == languageId);

			var productViewModel = new ProductViewModel()
			{
				Id = product.Id,
				DateCreated = product.DateCreated,
				Description = productTranslation != null ? productTranslation.Description : null,
				LanguageId = productTranslation.LanguageId,
				Details = productTranslation != null ? productTranslation.Details : null,
				Name = productTranslation != null ? productTranslation.Name : null,
				OriginalPrice = product.OriginalPrice,
				Price = product.Price,
				SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
				SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
				SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
				Stock = product.Stock,
				ViewCount = product.ViewCount
			};
			return productViewModel;
		}

		public async Task<ProductImageViewModel> GetImageById(int imageId)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null) throw new EShopException($"Cannot find a product image: {imageId}");

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
			return Image;
		}

		public async Task<List<ProductImageViewModel>> GetListImages(int productId)
		{
			return await _context.ProductImages.Where(x => x.ProductId == productId)
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

			}).ToListAsync();
		}

		public async Task<int> RemoveImage( int imageId)
		{
			var productImage = await _context.ProductImages.Where(x =>  x.Id == imageId).FirstOrDefaultAsync();
			if (productImage == null) throw new EShopException($"Cannot find a product image: {imageId}");
			_context.ProductImages.Remove(productImage);

			return await _context.SaveChangesAsync();

		}

		public async Task<int> Update(ProductUpdateRequest request)
		{
			var product = await _context.Products.FindAsync(request.Id);
			var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.Id == request.Id && x.LanguageId == request.LanguageId);

			if (product == null || productTranslation == null)
				throw new EShopException($"Cannot find a product: {request.Id}");
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
			return await _context.SaveChangesAsync();
		}

		public async Task<int> UpdateImage( int imageId, ProductImageUpdateRequest request)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);
			if (productImage == null) throw new EShopException($"Cannot find a product image: {imageId}");

			if (request.ImageFile != null)
			{
				productImage.Caption = request.Caption;
				productImage.SortOrder = request.SortOrder;
				productImage.ImagePath = await this.SaveFile(request.ImageFile);
				productImage.FileSize = request.ImageFile.Length;
			}
			_context.ProductImages.Update(productImage);
			return await _context.SaveChangesAsync();
		}

		public async Task<bool> UpdatePrice(int productId, decimal newPrice)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				throw new EShopException($"Cannot find a product: {productId}");
			product.Price = newPrice;
			return await _context.SaveChangesAsync() > 0;
		}



		public async Task<bool> UpdateStock(int productId, int addedQuantity)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
				throw new EShopException($"Cannot find a product: {productId}");
			product.Stock += addedQuantity;
			return await _context.SaveChangesAsync() > 0;
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
