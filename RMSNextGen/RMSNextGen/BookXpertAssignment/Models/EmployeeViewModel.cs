using BookXpertAssignment.Models.ModelsUsingEFCore;
using System.ComponentModel.DataAnnotations;

namespace BookXpertAssignment.Models
{
	public class EmployeeViewModel
	{
		[Required(ErrorMessage = "Name is required")]

		public string Name { get; set; }
		[Required(ErrorMessage = "Designation is required")]

		public string Designation { get; set; }
		[Required(ErrorMessage = "DateOfJoin is required")]

		public DateTime DateOfJoin { get; set; }
		[Required(ErrorMessage = "Salary is required")]

		public decimal Salary { get; set; }
		[Required(ErrorMessage = "Gender is required")]

		public string Gender { get; set; }
		[Required(ErrorMessage = "State is required")]


		public int StateId { get; set; }
		[Required(ErrorMessage = "DateOfBirth is required")]

		public DateTime DateOfBirth { get; set; }
		public int Age { get; set; }

	}
}
