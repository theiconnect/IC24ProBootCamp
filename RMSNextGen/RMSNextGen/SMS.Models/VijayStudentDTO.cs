using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class VijayStudentDTO
    {
        public String Studentcode { get; set; }
        public string studentName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Grade { get; set; }
        public string IsOwnTransport { get; set; }
        public string Comments { get; set; }
    }
}
