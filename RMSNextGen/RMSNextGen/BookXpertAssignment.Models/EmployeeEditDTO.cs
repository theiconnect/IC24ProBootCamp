using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookXpertAssignment.Models
{
	public class EmployeeEditDTO
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Designation { get; set; }
		public DateOnly DateOfJoin { get; set; }
		public decimal Salary { get; set; }
		public string Gender { get; set; }
		public int StateId { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public int Age { get; set; }
	}
}
