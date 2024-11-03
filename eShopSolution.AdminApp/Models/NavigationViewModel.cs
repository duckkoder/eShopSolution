using eShopSolution.ViewModels.System.Language;

namespace eShopSolution.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageVM> Languages {  get; set; }
        
        public string CurrentLanguage { get; set; }
    }
}
