using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId);
    }
}
