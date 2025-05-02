using System.ComponentModel.DataAnnotations;

namespace RMSNextGen.Web.Models
{
    public class AddStoreViewModel
    {
        [Required(ErrorMessage = "Store Code is required")]
        public string StoreCode { get; set; }

        [Required(ErrorMessage = "Store Location is required")]
        public string StoreLocation { get; set; }

        [Required(ErrorMessage = "Store Name is required")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "NickName is required")]

        public string NickName { get; set; }

        [Required(ErrorMessage = "Address is required")]

        public string Address { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        public string OfficeNo { get; set; }

        public string ManagerName { get; set; }

        public string ManagerNo { get; set; }

        public string GSTNo { get; set; }

        public string CINNo { get; set; }

        public string FAX { get; set; }

        public bool IsCorporateOffice { get; set; }

        // Optional: make these nullable if not submitted from view
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
