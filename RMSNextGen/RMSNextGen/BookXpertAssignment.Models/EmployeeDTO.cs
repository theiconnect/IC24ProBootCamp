using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookXpertAssignment.Models
{
    public class EmployeeDTO
    {
		public string Name { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoin { get; set; }
		public decimal Salary { get; set; }
		public string Gender { get; set; }
		public int StateId { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int Age { get; set; }
	}
}
