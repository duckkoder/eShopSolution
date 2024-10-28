using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Common
{
    public class SelectedRole{
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
