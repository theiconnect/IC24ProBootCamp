using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSNextGen.Models
{
	public class UserDTO
	{
		
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		
		public string RoleName { get; set; } // Needed for role-based claims



	}
}
