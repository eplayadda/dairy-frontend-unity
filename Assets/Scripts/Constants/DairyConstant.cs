using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DairyConstant 
{
    public static string URL_Port = ":5555";//Live server port
  //  public static string URL_Port = "";
    public static string URL_Domain = "http://13.234.211.102" + URL_Port;
    public static string URL_LogIn = URL_Domain + "/api/Login/getLogin";
    public static string URL_Profile = URL_Domain + "/api/Customer/addCustomerByFarmID";
    public static string URL_ALL_Customer = URL_Domain + "/api/Customer/getCustomerByFarmID";
    public static string URL_Order = URL_Domain + "/api/Order/addOrder";
    public static string URL_OrderBy_ID = URL_Domain + "/api/Order/getOrderByOrderID";
    public static string URL_OrderListBy_Month = URL_Domain + "/api/Order/getOrderListByMonth";
    public static string URL_OrderListByPrevMonth = URL_Domain + "/api/Order/getPrevOrder";
    public static string URL_OrderListByNextMonth = URL_Domain + "/api/Order/getNextOrder";
    public static string URL_ADD_Order = URL_Domain + "/api/Order/addOrder";
    public static string URL_Add_Invoice = URL_Domain + "/api/Invoice/addInvoice";
    public static string URL_Get_InvoiceBy_Month = URL_Domain + "/api/Invoice/getInvoiceByMonth";
    public static string URL_All_Invoice = URL_Domain + "/api/Invoice/getAllInvoice";
    public static string URL_IS_Invoice_Created = URL_Domain + "/api/Invoice/IsInvoiceCreated";
    public static string URL_Payment = URL_Domain + "/api/Invoice/addPaymentDetails";
    public static string URL_InvoiceByPrevMonth = URL_Domain + "/api/Invoice/getPrevInvoice";
    public static string URL_InvoiceByNextMonth = URL_Domain + "/api/Invoice/getNextInvoice";
    public static string URL_SinUp_Number_Verification = URL_Domain + "/api/Sinup/validateMobile";
    public static string URL_SinUp_OTP_Verification = URL_Domain + "/api/Sinup/onSubmitOTP";
    public static string URL_SinUp_CreatePassword = URL_Domain + "/api/Sinup/createPassword";
    public static string URL_Account = URL_Domain + "/api/DairyProfile/createDairyProfile";
    public static string URL_Account_GetFarmID = URL_Domain + "/api/DairyProfile/getDairyByFarmID";
    public static string URL_Dairy_Shop = URL_Domain + "/api/DairyShop/getProducts";

}
