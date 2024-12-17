using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSC_Models
{
    public class EmployeeModel
    {
        public int EmployeeIDPK { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string Role { get; set; }
        public string DateOfJoiningstr { get; set; }
        public string DateOfLeavingstr { get; set; }


        public DateTime DateOfJoining
        {
            get
            {
                return (Convert.ToDateTime(DateOfJoiningstr));
            }
        }
        public DateTime? DateOfLeaving
        {

            get
            {
                if (DateTime.TryParse(DateOfLeavingstr, out DateTime Dt))
                {
                    return Dt;
                }
                return null;
            }

        }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public Decimal Salary { get; set; }
        public int Storeidfk { get; set; }




    }
}
