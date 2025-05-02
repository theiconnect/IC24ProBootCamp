namespace RMSNextGen.Web.Models
{
    public class StoreEditViewModel
    {
        public int StoreId { get; set; } // For editing existing stores

        public string StoreCode { get; set; }

        public string StoreLocation { get; set; }

        public string NickName { get; set; }

        public string Address { get; set; }

        public string OfficeNo { get; set; }

        public string ManagerName { get; set; }

        public string ManagerNo { get; set; }

        public string GSTNo { get; set; }

        public string CINNo { get; set; }

    }
}
