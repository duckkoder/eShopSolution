using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Common;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Azure.Core;

namespace eShopSolution.Application.Catalog.Brands
{
   
    public class BrandService : IBrandService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public BrandService(EShopDbContext context, IStorageService storage)
        {
            _context = context;
            _storageService = storage;
        }
        public async Task<ApiResult<bool>> Create(BrandCreateRequest request)
        {
            var brand = new Brand()
            {
                Name = request.Name,
                Description = request.Description,
            };
            brand.FileSize = 1;
            if(request.ThumbnailImage!=null)
            {
                brand.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
            _context.Brands.Add(brand);
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Create Successful Brand Name {request.Name}");
            return new ApiErrorResult<bool>($"Create Unsuccessful Brand Name {request.Name}!");
        }

        public async Task<ApiResult<bool>> Delete(int Id)
        {
            var brand = await _context.Brands.FindAsync(Id);
            if (brand == null)
            {
                return new ApiErrorResult<bool>($"Cannot Find Brand Id {Id}!");
            }
            _context.Brands.Remove(brand);
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Delete Successful Brand Id {Id}");
            return new ApiErrorResult<bool>($"Delete Unsuccessful Brand Id  {Id}!");

        }

        public async Task<ApiResult<List<BrandViewModel>>> GetAll()
        {
            var brands = await _context.Brands.Select(x => new BrandViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                imagePath = x.ImagePath
            }).ToListAsync();

            return new ApiSuccessResult<List<BrandViewModel>>(brands);
        }

        public async Task<ApiResult<BrandViewModel>> GetById(int Id)
        {
            var brand = await _context.Brands.FindAsync(Id);
            if (brand == null)
            {
                return new ApiErrorResult<BrandViewModel>($"Cannot Find Brand Id {Id}!");
            }
            var brandVM = new BrandViewModel()
            {
                Id = brand.Id,
                Description = brand.Description,
                Name = brand.Name,
                imagePath = brand.ImagePath
            };

            return new ApiSuccessResult<BrandViewModel>(brandVM);
        }

        public async Task<ApiResult<bool>> Update(BrandUpdateRequest request)
        {
            var brand = await _context.Brands.FindAsync(request.Id);
            if (brand == null)
            {
                return new ApiErrorResult<bool>($"Cannot Find Brand Id {request.Id}!");
            }

            brand.Name = request.Name;
            brand.Description = request.Description;
            if (request.ThumbnailImage != null)
            {
                brand.ImagePath = await this.SaveFile(request.ThumbnailImage);
            }
           
            if (await _context.SaveChangesAsync() > 0)
                return new ApiSuccessResult<bool>().CreateMessage($"Update Successful Brand Id {request.Id}");
            return new ApiErrorResult<bool>($"Update Unsuccessful Brand Id  {request.Id}!");

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
