using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Common
{
	public class PagedResult<T>
	{
		public int TotalRecord { get; set; }

		public List<T> Items { get; set; }
	}
}
