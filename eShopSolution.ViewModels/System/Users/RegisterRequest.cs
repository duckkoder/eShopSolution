using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
	public class RegisterRequest
	{
		public string UserName { get; set; } 

		public string Password { get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime Dob { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public string PasswordConfirmed { get; set;}

		public bool IsEmailConfirmed { get; set; } = false;
	}
}
