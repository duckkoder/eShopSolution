﻿using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace eShopSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly EShopDbContext _context;
        private readonly IConfiguration _configuration;
        public LanguageService(EShopDbContext context,IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ApiResult<List<LanguageVM>>> GetAll()
        {
            var languages = await _context.Languages.Select(x=> new LanguageVM() { Id = x.Id,Name=x.Name}).ToListAsync();
            if (languages.Any())
            {
                return new ApiSuccessResult<List<LanguageVM>>(languages);
            }
            return new ApiErrorResult<List<LanguageVM>>("Get Languages Unsuccessfully");
        }
    }
}
