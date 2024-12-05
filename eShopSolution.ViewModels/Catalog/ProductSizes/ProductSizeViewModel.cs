using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.ProductSizes
{
    public class ProductSizeViewModel
    {
        public int Id {  get; set; }

        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
    }
}
