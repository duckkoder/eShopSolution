using Azure.Core;
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace eShopSolution.Application.Catalog.Products
{
	public class ManageProductService : IManageProductService
	{
		private readonly EShopDbContext _context;
		private readonly IStorageService _storageService;
		public ManageProductService(EShopDbContext context,IStorageService storage)
		{
			_context = context;
			_storageService = storage;
		}

		public async Task<int> AddImages(int productId, List<IFormFile> files)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new EShopException($"Cannot find a product: {productId}");

			
			
			foreach(IFormFile file in files)
			{
				product.productImages.Add(
					new ProductImage()
					{
						Caption = file.FileName,
						DateCreated = DateTime.Now,
						FileSize = file.Length,
						ImagePath = await this.SaveFile(file),
					});
			}
			return await _context.SaveChangesAsync();
		}

		public async Task AddViewcount(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			product.ViewCount += 1;
			await _context.SaveChangesAsync();
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
			return await _context.SaveChangesAsync();
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

		public async Task<List<ProductImageViewModel>> GetListImage(int productId)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null) throw new EShopException($"Cannot find a product: {productId}");

			var images = _context.ProductImages.Where(i => i.ProductId == productId);

			var data = new List<ProductImageViewModel>();
			foreach ( var image in images)
			{
				data.Add(new ProductImageViewModel()
				{
					Id = image.Id,
					IsDefault = image.IsDefault,
					FileSize = image.FileSize,
					FilePath = image.ImagePath
					
				});
			}
			return data;
		}

		public async Task<int> RemoveImages(List<IFormFile> files)
		{
			foreach (var file in files)
			{
				var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
				await _storageService.DeleteFileAsync(fileName);
			}
			return await _context.SaveChangesAsync();
		}


		public async Task<int> Update(ProductUpdateRequest request)
		{
			var product = await _context.Products.FindAsync(request.Id);
			var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x=> x.Id == request.Id&&x.LanguageId==request.LanguageId);

			if (product == null || productTranslation==null)
				throw new EShopException($"Cannot find a product: {request.Id}");
			productTranslation.Name = request.Name;
			productTranslation.Description = request.Description;
			productTranslation.Details = request.Details;
			productTranslation.SeoTitle = request.SeoTitle;
			productTranslation.SeoAlias = request.SeoAlias;
			productTranslation.SeoDescription = request.SeoDescription;
			

			if(request.ThumbnailImage!= null)
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

		public async Task<int> UpdateImages(int imageId, string caption, bool isDefault)
		{
			var productImage = await _context.ProductImages.FindAsync(imageId);

			if(productImage==null)
			{
				throw new EShopException($"Can't find Image with Id {imageId}");
			}
			productImage.Caption = caption;
			productImage.IsDefault = isDefault;

			return await _context.SaveChangesAsync();

		}

		public async Task<bool> UpdatePrice(int productId, decimal newPrice)
		{
			var product = await _context.Products.FindAsync(productId);
			if (product == null )
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
