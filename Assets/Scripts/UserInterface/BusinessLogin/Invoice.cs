using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace DairyBusiness
{
    public class Invoice
    {
        public bool IsInvoiceExist(string pCustomerID, out CustomeList pCustomeList)
        {
            var customeList = DairyApplicationData.Instance.customerInvoice.customeList;
            var custInvoice = customeList.Where(s => s.customer_id == pCustomerID);
            if (custInvoice.Count() > 0)
            {
                pCustomeList = custInvoice.First();
                return true;
            }
            else
            {
                pCustomeList = null;
                return false;
            }
        }

        public bool IsInvoiceExist(string pCustomerID)
        {
            var customeList = DairyApplicationData.Instance.customerInvoice.customeList;
            var custInvoice = customeList.Where(s => s.customer_id == pCustomerID);
            if (custInvoice.Count() > 0)
                return true;
            else
                return false;
        }

    }
}

