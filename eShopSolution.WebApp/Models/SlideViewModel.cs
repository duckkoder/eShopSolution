using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Catalog.Categories;

namespace eShopSolution.WebApp.Models
{
    public class SlideViewModel
    {
        public List<BrandViewModel> Brands { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
