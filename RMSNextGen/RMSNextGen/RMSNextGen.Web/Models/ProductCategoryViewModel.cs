namespace RMSNextGen.Web.Models
{
    public class ProductCategoryViewModel
    {
        public int CategoryIdPK { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateOnly CreatedOn { get; set; }
    }

}
