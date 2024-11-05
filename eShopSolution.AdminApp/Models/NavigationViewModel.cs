using eShopSolution.ViewModels.System.Language;

namespace eShopSolution.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageViewModel> Languages {  get; set; }
        
        public string CurrentLanguage { get; set; }
    }
}
