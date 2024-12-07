using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Catalog.Categories;

namespace eShopSolution.WebApp.Models
{
    public class NavigationViewModel
    {
        public List<BrandViewModel> Brands { get; set; } = new List<BrandViewModel>();

        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
