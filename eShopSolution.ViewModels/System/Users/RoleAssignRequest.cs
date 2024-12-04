using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public Guid id {  get; set; }
        public List<SelectedItem> Roles { get; set; } = new List<SelectedItem>();
    }
}
