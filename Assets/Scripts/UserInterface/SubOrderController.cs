using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;
using System.Linq;

public class SubOrderController : MonoBehaviour
{
    public int data;
   
    public Image _profile_Image;
    public Text _Id_Text;
    public Text _name_Text;
    public Text _address_Text;
    public Button _contact_Button;
    public Button _whatsApp_Button;
    public Text _total_Milk_Text;
    public Text _rate_Text;
    public Text _SubTotal_Text;
    public Text _duesOrAdvance_Text;
    public Text _total_Text;
   // public Text _duesOrAdvance_Level;
    public GameObject _dues_Level;
    public GameObject _advance_Level;

    public GameObject _checkOutBtn;
    public CalendarController calendarController;
    public CustomerMonthlyDetails customerMonthlyDetails;
    public bool isCheckedOut;
    WAddInvoiceRequestBody mAddInvoiceRequestBody;
    float eveningMilk = 0;
    float morningMilk = 0;
    float totalMilk = 0;
    float amount = 0;
    int ratePerLetter = 0;
    float preDue = 0;
    public  WAddInvoiceResponce mInvoiceResponce;
    public string _Mobile;
    Coroutine _CoroutineAddInvoice;
    //public AccountController _accountController;

    private void OnEnable()
    {
    }

   
    public void SetCurrentOrderDetails()
    {
        SetMonthlyDetails(customerMonthlyDetails,data);
    }
    void ChangeFont()
    {
        Font font = UiManager.instance.currentFont;
        _name_Text.font = font;
        _address_Text.font = font;
    }
    public void SetMonthlyDetails(CustomerMonthlyDetails pCustomerMonthlyDetails,int i)
    {
        ResetData();
        ChangeFont();
        data = i;
        customerMonthlyDetails = pCustomerMonthlyDetails;
        PrevDue();
        AddTotalMilk(pCustomerMonthlyDetails.orderInfos);
        string pName = pCustomerMonthlyDetails.customerInfo.c_name;
        string pAddress = pCustomerMonthlyDetails.customerInfo.c_area;

        //string pAddress = pCustomerMonthlyDetails.customerInfo.c_building + " " + pCustomerMonthlyDetails.customerInfo.c_floor + " floor" + " " + " road no.- "
        //    + pCustomerMonthlyDetails.customerInfo.c_road_number + " " + pCustomerMonthlyDetails.customerInfo.c_area + " " + pCustomerMonthlyDetails.customerInfo.c_pin_code;

        //if (UiManager.instance.currLanguage == eLanguage.hindi)
        //{
        //    pName = UnicodeToKrutidev.UnicodeToKrutiDev(pName);
        //    pAddress = UnicodeToKrutidev.UnicodeToKrutiDev(pAddress);
        //}
        _name_Text.GetComponent<DairyText>().DairyString(pName);
        _address_Text.text = "";
        _address_Text.GetComponent<DairyText>().DairyString(pAddress);
        _Id_Text.text = pCustomerMonthlyDetails.customerInfo.rank.ToString();
        totalMilk = morningMilk + eveningMilk;
        _total_Milk_Text.text = totalMilk.ToString();
        ratePerLetter = pCustomerMonthlyDetails.customerInfo.c_rate;
        _rate_Text.text = ratePerLetter.ToString();
        _SubTotal_Text.text = (totalMilk * ratePerLetter).ToString();
        if (preDue > 0)
        {
            _dues_Level.SetActive(true);
            _advance_Level.SetActive(false);
            _duesOrAdvance_Text.text = preDue.ToString();
        }
        else
        {
            _dues_Level.SetActive(false);
            _advance_Level.SetActive(true);
            _duesOrAdvance_Text.text = preDue.ToString();
        }
        _total_Text.text = ((totalMilk * ratePerLetter) + preDue).ToString();
        calendarController.InitData(pCustomerMonthlyDetails.orderInfos,this);
        //_Mobile = pCustomerMonthlyDetails.customerInfo.c_mob_number;
        _Mobile = pCustomerMonthlyDetails.customerInfo.c_mob_number;
        if(!DairyApplicationData.isLastMonthInvoiceGenerated)
        {
            if (DairyApplicationData.Instance.checkOutDone.Contains(customerMonthlyDetails.customerInfo.Id))
            {
                isCheckedOut = true;
                _checkOutBtn.GetComponent<Button>().interactable = false;
            }
            else
            {
                isCheckedOut = false;
                _checkOutBtn.GetComponent<Button>().interactable = true;
            }
        }
      
    }

    void OnContactCustomer()
    {
        Application.OpenURL("tel:" + _Mobile);
        Debug.Log("calling" + _Mobile);
    }

    void PrevDue()
    {
        var invoiceData = DairyApplicationData.Instance.customerInvoice.customeList.Where(s => s.customer_id == customerMonthlyDetails.customerInfo.Id);
        if (invoiceData.Count() > 0)
        {
            if (invoiceData != null)
            {
                preDue = invoiceData.First().amount - invoiceData.First().payment;
            }
        }

        else 
        {
            if (customerMonthlyDetails.customerInfo.c_preDue > 0)
            {
                preDue = customerMonthlyDetails.customerInfo.c_preDue;
            }
            else
            {
                preDue = 1 * customerMonthlyDetails.customerInfo.c_preAdvance;
            }
        }

    }

    void AddTotalMilk(List<OrderInfo> pOrderInfos)
    {
        foreach (var item in pOrderInfos)
        {
            if (item.o_sift == "Evening")
            {
                eveningMilk += item.o_quantity;
            }
            else
            {
                morningMilk += item.o_quantity;
            }
        }
        amount = (eveningMilk + morningMilk) * ratePerLetter;
    }
    public void OnAddInvoice()
    {
        Debug.Log(transform.name+"???");
        if (customerMonthlyDetails.orderInfos.Count > 0)
        {
            UiManager.instance._loadingController.gameObject.SetActive(true);
            mAddInvoiceRequestBody = new WAddInvoiceRequestBody();
            mAddInvoiceRequestBody.farm_id = DairyApplicationData.Instance.FarmID;
            mAddInvoiceRequestBody.customer_id = customerMonthlyDetails.customerInfo.Id;
            mAddInvoiceRequestBody.inv_generate_date = DairyApplicationData.todayDate;
            mAddInvoiceRequestBody.inv_for_month = customerMonthlyDetails.orderInfos[0].o_month;
            mAddInvoiceRequestBody.inv_for_year = customerMonthlyDetails.orderInfos[0].o_year;
            mAddInvoiceRequestBody.total_ltr_evening = eveningMilk;
            mAddInvoiceRequestBody.total_ltr_morning = morningMilk;
            mAddInvoiceRequestBody.rate_per_ltr = ratePerLetter;
            mAddInvoiceRequestBody.amount = amount;
            string AddInvoiceStr = JsonUtility.ToJson(mAddInvoiceRequestBody);
            Debug.Log("invoice........" + AddInvoiceStr);
            _CoroutineAddInvoice =  DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Add_Invoice, "", AddInvoiceStr, AddInvoiceCallback);
            Debug.Log("Customer Name ........  " + customerMonthlyDetails.customerInfo.c_name);
        }
        else
        {
            Debug.Log("No need to create invoice for this months.");
            UiManager.instance._invoiceGeneratedMsgController.OnSuccessInvoice(InvoiceGeneratedMsgController.eMsgType.noOrder);
        }
    }

    public void AddInvoiceCallback(bool success, string response)
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);
        if (success)
        {
            mInvoiceResponce = JsonUtility.FromJson<WAddInvoiceResponce>(response);
            Debug.Log("Responce....." + response);
            UiManager.instance._invoiceGeneratedMsgController.OnSuccessInvoice(InvoiceGeneratedMsgController.eMsgType.success);
            DairyApplicationData.Instance.checkOutDone.Add(mInvoiceResponce.customer_id);
            _checkOutBtn.GetComponent<Button>().interactable = false;
            isCheckedOut = true;
        }
        else
        {
            UiManager.instance._invoiceGeneratedMsgController.OnSuccessInvoice(InvoiceGeneratedMsgController.eMsgType.badRequest);
        }
    }


    void OnCheckOut()
    {
        UiManager.instance._invoiceGeneratedMsgController.gameObject.SetActive(true);
        UiManager.instance._invoiceGeneratedMsgController._suborderControler = this;
        Debug.Log("Checkout.........."+transform.name);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnCheckout_Btn":
                OnCheckOut();
                break;
            case "OnContact_Btn":
                OnContactCustomer();
                break;
            default:
                break;
        }
    }
    public void OrderPlaced(OrderInfo pOrderInfo)
    {
        totalMilk += pOrderInfo.o_quantity;
        _total_Milk_Text.text = totalMilk.ToString();
        _SubTotal_Text.text = (totalMilk * ratePerLetter).ToString();
        _total_Text.text = ((totalMilk * ratePerLetter) + preDue).ToString();

    }
    public void SetInvoiceData(OrderList oldOrNewOrderList,CustomeList pInvoiceObj)
    {
        ResetData();
        AddTotalMilk(oldOrNewOrderList.order_Infos);
        float rate = 0;
        if (pInvoiceObj != null)
        {
            rate = pInvoiceObj.rate_per_ltr;
            if(pInvoiceObj.dueAmount>0)
            {
                
                preDue = pInvoiceObj.dueAmount ;
            }
            else
            {
              
                preDue = -1 * pInvoiceObj.advanceAmount;
            }

        }
        else
        {
            var invoice = DairyApplicationData.Instance.customerInvoice.customeList.Where(s => s.customer_id == customerMonthlyDetails.customerInfo.Id).First();
            rate = invoice.rate_per_ltr;
            preDue = invoice.amount - invoice.payment;

        }

        totalMilk = morningMilk + eveningMilk;
        _total_Milk_Text.text = totalMilk.ToString();
        _SubTotal_Text.text = (totalMilk * rate).ToString();
        _duesOrAdvance_Text.text = Mathf.Abs( preDue).ToString();
        _total_Text.text = ((totalMilk * rate) + preDue).ToString();
    }
    private void ResetData()
    {
        eveningMilk = 0;
        morningMilk = 0;
        totalMilk = 0;
        amount = 0;
        ratePerLetter = 0;
        preDue = 0;
        _checkOutBtn.SetActive(!DairyApplicationData.isLastMonthInvoiceGenerated);
        isCheckedOut = false;
      

    }

    //public void OnGoToPaidPage()
    //{
    //    UiManager.instance._paidInvoiceController.gameObject.SetActive(true);
    //    //UiManager.instance._paidInvoiceController.SetInvoiceDetails();
    //}
}


[System.Serializable]
public class WAddInvoiceRequestBody
{
    public string customer_id;
    public string farm_id;
    public int inv_for_month;
    public int inv_for_year;
    public int inv_generate_date;
    public int rate_per_ltr;
    public float total_ltr_morning;
    public float total_ltr_evening;
    public float previous_balance;
    public float amount;
    public float payment;
}

[System.Serializable]
public class WAddInvoiceResponce
{
    public string Id;
    public string customer_id;
    public string farm_id;
    public int inv_for_month;
    public int inv_for_year;
    public int inv_generate_date;
    public int rate_per_ltr;
    public float total_ltr_morning;
    public float total_ltr_evening;
    public float previous_balance;
    public float amount;
    public float payment;
}