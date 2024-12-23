using FileModel;
using OMS_IDAL;
using OMSEntityDAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSEntityDAL
{
    public class ReturnsEntityDAL:IReturnsDAL
    {

        public OMSEntities OMSEntities {  get; set; }
        public ReturnsEntityDAL()
        {
            OMSEntities=new OMSEntities();
        }
        public bool PushReturnsDataToDB(List<ReturnsModel> returnsList, string returnFilePath)
        {
            try
            {
                foreach (var returnRecord in returnsList)
                {
                    Returns returns = new Returns();
                    var orderidFk=OMSEntities.Orders.Where(o => o.InvoiceNumber == returnRecord.InvoiceNumber).Select(x=>x.OrderIdpk).FirstOrDefault();
                    var returnStatusIdfk=OMSEntities.ReturnStatus.Where(o => o.ReturnStatus1==returnRecord.ReturnStatus).Select(x=>x.ReturnStatusIdpk).FirstOrDefault();    
                    returns.OrderIdfk= orderidFk;
                    returns.ReturnDate=Convert.ToDateTime(returnRecord.Date);
                    returns.Reason= returnRecord.Reason;
                    returns.ReturnStatusIdfk = returnStatusIdfk;
                    returns.AmountRefund= returnRecord.AmountRefund;
                    OMSEntities.Returns.Add(returns);
                    OMSEntities.SaveChanges();


                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
