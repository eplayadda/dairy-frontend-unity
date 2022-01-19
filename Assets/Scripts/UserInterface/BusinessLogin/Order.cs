using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace DairyBusiness
{
    public class Order
    {
        public bool IsOrderExist(string pCustomerID)
        {
            var orderList = DairyApplicationData.Instance.orderList.order_Infos;
            var custInvoice = orderList.Where(s => s.o_customer_id == pCustomerID);
            if (custInvoice.Count() > 0)
                return true;
            else
                return false;
        }

    }
}