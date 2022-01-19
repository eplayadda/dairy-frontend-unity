using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class PaidInvoiceController : MonoBehaviour
{
    public Text _nameText;
    public Text _addressText;
    public Text _mobile_Text;
    public Text _total_Milk_Text;
    public Text _rate_Text;
    public Text _subTotal_Text;
    public Text _dues_Text;
    public Text _total_Text;
    public Text _month_Text;
    public Text _year_Text;
    public Text _paid_Text;
    public Text _date_Text;
    public Text _id_Text;
    public GameObject _dues_Level;
    public GameObject _advance_Level;

    public Text _remaing_Amount_Text;
    public Text _remaing_Amount_lVL;
    public Button _submitBtn; 
    public GameObject paidDetailsPrefab;
    public Transform parentForPaidDetails;
    public Transform reamaningAmountPanel;
    public InputField _Paid_InputField;
    public GameObject nextBtn;
    public GameObject prevBtn;
    int invoiceMonth;
    int invoiceYear;
    string idForEdit;
    WPaymentDetailsRequestBody mpaymentDetailsRequestBody;
    SubOrderController subOrderController;
    CustomeList orderInfo;
    CustomerInfo customerInfo;
    List<GameObject> paimentDetailsGo = new List<GameObject>();
    ItemDetailsInvoiceIndex itemDetailsInvoiceCallback;
    int latestInvoiceMonth;
    int latestInvoiceYear;
    public bool saveFirstTime;
    Coroutine _coroutineGetInvoice;
    Coroutine _CoroutinePaymentSubmit;
    public int data;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }
    void ChangeFont()
    {
        Font font = UiManager.instance.currentFont;
        _nameText.font = font;
        _addressText.font = font;
        _month_Text.font = font;
        _dues_Text.font = font;
        _remaing_Amount_lVL.font = font;
    }
    public void SetInvoiceDetails(CustomeList pOrderInfo, CustomerInfo pCustomerInfo, ItemDetailsInvoiceIndex pItemDetailsInvoiceCallback)
    {
        Debug.Log("Disply Invoice Data");
       // ChangeFont();
        var invID = DairyApplicationData.Instance.customerInvoice.customeList.Where(s => s.Id == pOrderInfo.Id).ToList();
        if (invID.Count > 0)
        {
            _Paid_InputField.interactable = true;
            _submitBtn.interactable = true;
        }
        else
        {
            _submitBtn.interactable = false;
            _Paid_InputField.interactable = false;
        }
        orderInfo = pOrderInfo;
        customerInfo = pCustomerInfo;
        itemDetailsInvoiceCallback = pItemDetailsInvoiceCallback;
        string name = pCustomerInfo.c_name;
        string address = pCustomerInfo.c_area;

        //string address = pCustomerInfo.c_building + ", " + pCustomerInfo.c_floor + " floor" + ", " + " road no:- "
        //    + pCustomerInfo.c_road_number + ", " + pCustomerInfo.c_area + ", " + pCustomerInfo.c_pin_code; 
        //if (UiManager.instance.currLanguage == eLanguage.hindi)
        //{
        //    name = UnicodeToKrutidev.UnicodeToKrutiDev(name);
        //    address = UnicodeToKrutidev.UnicodeToKrutiDev(address);
        //}
        _nameText.GetComponent<DairyText>().DairyString(name);
        _addressText.GetComponent<DairyText>().DairyString(address);
        _mobile_Text.text = pCustomerInfo.c_mob_number;
        float totalMilk = (pOrderInfo.total_ltr_morning + pOrderInfo.total_ltr_evening);
        _total_Milk_Text.text = totalMilk.ToString();
        _rate_Text.text = pOrderInfo.rate_per_ltr.ToString();
        float subTotal = totalMilk * pOrderInfo.rate_per_ltr;
        _subTotal_Text.text = subTotal.ToString();

        invoiceMonth = Convert.ToInt32(pOrderInfo.inv_for_month);
        invoiceYear = Convert.ToInt32(pOrderInfo.inv_for_year);


        if (pOrderInfo.dueAmount > 0)
        {
            _dues_Level.SetActive(true);
            _advance_Level.SetActive(false);
            _dues_Text.text = pOrderInfo.dueAmount.ToString();
        }
        else
        {
            _dues_Level.SetActive(false);
            _advance_Level.SetActive(true);
            _dues_Text.text = (pOrderInfo.advanceAmount).ToString();
        }
        float total = subTotal + pOrderInfo.dueAmount - pOrderInfo.advanceAmount;
        _total_Text.text = total.ToString();
        int month = Convert.ToInt32(pOrderInfo.inv_for_month);
        string strMonth = Utility.GetMonthInString(month, UiManager.instance.currLanguage);
        _month_Text.GetComponent<DairyText>().DairyString(strMonth);
        _id_Text.text = pCustomerInfo.rank.ToString();
        _year_Text.text = pOrderInfo.inv_for_year;
        //float payment = pOrderInfo.payment;
        _paid_Text.text = pOrderInfo.payment.ToString();
        DisplayAllPaymentDetails();
        float remainingAmount = total - pOrderInfo.payment;
        _remaing_Amount_Text.text = remainingAmount.ToString();
        if (!saveFirstTime)
        {
            latestInvoiceMonth = int.Parse(pOrderInfo.inv_for_month);
            latestInvoiceYear = int.Parse(pOrderInfo.inv_for_year);
            saveFirstTime = true;
        }
        SetNextnPrevBtn(latestInvoiceMonth, latestInvoiceYear);
    }
    void DisplayAllPaymentDetails()
    {
        foreach (var item in paimentDetailsGo)
        {
            Destroy(item);
        }
        paimentDetailsGo.Clear();
        foreach (var item in orderInfo.payment_details)
        {
            GameObject go = Instantiate(paidDetailsPrefab);
            go.SetActive(true);
            go.transform.SetParent(parentForPaidDetails, false);
            PaidDetailsItems paidDetailsItems = go.GetComponent<PaidDetailsItems>();
            DateTime dateTime = DateTime.Parse(item.payment_date);
            paidDetailsItems._date.text = "( "+ dateTime.Day+"/"+dateTime.Month+"/"+dateTime.Year + " )";
            paidDetailsItems._amount.text= item.payment_amount.ToString();
            paimentDetailsGo.Add(go);
        }
        reamaningAmountPanel.SetAsLastSibling();
    }
    void OnPaymentSubmit()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        mpaymentDetailsRequestBody = new WPaymentDetailsRequestBody();
        idForEdit = orderInfo.Id;
        mpaymentDetailsRequestBody.invoice_id = orderInfo.Id;
        mpaymentDetailsRequestBody.dateTime = DateTime.Now.ToString();
        mpaymentDetailsRequestBody.paid_amount = float.Parse(_Paid_InputField.text);

        string paymentBodyStr = JsonUtility.ToJson(mpaymentDetailsRequestBody);
        Debug.Log("Payment string .... " + paymentBodyStr);
        Debug.Log("time string .... " + mpaymentDetailsRequestBody.dateTime);

        _CoroutinePaymentSubmit =  DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Payment, "", paymentBodyStr, PaymentCallback);
    }

    void PaymentCallback(bool success, string response)
    {
        if (success)
        {
            orderInfo = JsonUtility.FromJson<CustomeList>(response);
            AddAtSameIndex(orderInfo);
            UiManager.instance._loadingController.gameObject.SetActive(false);
            Debug.Log("PaymentCallback response :: " + response);
            SetInvoiceDetails(orderInfo, customerInfo,itemDetailsInvoiceCallback);
          //  UpdatePaymentDdetails();
            _Paid_InputField.text = "";
        }
        else
        {
        }
    }

    public void InvoiceDetails(CustomerMonthlyDetails  customerMonthlyDetails)
    {
        SetInvoiceDetails(orderInfo, customerInfo, itemDetailsInvoiceCallback);

    }
    void AddAtSameIndex(CustomeList customeList)
    {
        int i = 0;
        foreach (var item in DairyApplicationData.Instance.customerInvoice.customeList)
        {
            if (item.Id == idForEdit)
            {
                DairyApplicationData.Instance.customerInvoice.customeList.Remove(item);
                break;
            }
            i++;
        }
        //DairyApplicationData.Instance._farmCustomers.customerInfos.Add(mProfileResponse);
        DairyApplicationData.Instance.customerInvoice.customeList.Insert(i, customeList);
    }
    void UpdatePaymentDdetails()
    {
        foreach (var item in DairyApplicationData.Instance.customerInvoice.customeList)
        {
            if (item.Id == orderInfo.Id)
            {
                DairyApplicationData.Instance.customerInvoice.customeList.Remove(item);
                break;
            }
        }
        DairyApplicationData.Instance.customerInvoice.customeList.Add(orderInfo);
    }
    void OnBackButton()
    {
        if (_coroutineGetInvoice != null)
            StopCoroutine(_coroutineGetInvoice);
        if (_CoroutinePaymentSubmit != null)
            StopCoroutine(_CoroutinePaymentSubmit);
        UiManager.instance._khataController.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnSelectNextInvoice()
    {
        invoiceMonth = invoiceMonth + 1;
        isNext = true;
        GetInvoice(invoiceMonth,invoiceYear);
    }
    public void OnSelectPrevInvoice()
    {
        isNext = false;
        invoiceMonth = invoiceMonth - 1;
        GetInvoice(invoiceMonth, invoiceYear);
    }
    List<int> MMYY;
    bool isNext;
    void GetInvoice(int month,int year)
    {
        InvoiceRequestBodyForOld mInvoiceRequestBodyForOld = new InvoiceRequestBodyForOld();
        mInvoiceRequestBodyForOld.farmID = DairyApplicationData.Instance.FarmID;
        mInvoiceRequestBodyForOld.customerID = customerInfo.Id;
        MMYY = Utility.ConverInMMYY( month,year);
        mInvoiceRequestBodyForOld.month = MMYY[0];
        mInvoiceRequestBodyForOld.year = MMYY[1];
        string invoiceBodyStr = JsonUtility.ToJson(mInvoiceRequestBodyForOld);
        Debug.Log("order string .... " + invoiceBodyStr);
        string url = "";
        if (isNext)
            url = DairyConstant.URL_InvoiceByNextMonth;
        else
            url = DairyConstant.URL_InvoiceByPrevMonth;

        _coroutineGetInvoice =  DairyWebRequest.Instance.BeingPostRequest(url, "", invoiceBodyStr, InvoiceCallBack);
    }
    void InvoiceCallBack(bool success, string response)
    {
        if (success)
        {
            if (response != "null")
            {
                CustomeList invoiceObj = JsonUtility.FromJson<CustomeList>(response);
                SetInvoiceDetails(invoiceObj, customerInfo, itemDetailsInvoiceCallback);
                SetNextnPrevBtn(int.Parse( invoiceObj.inv_for_month), int.Parse(invoiceObj.inv_for_year));
                Debug.Log("New chart");
            }
            else
            {
                if (isNext)
                {
                    nextBtn.SetActive(false);
                }
                else
                {
                    prevBtn.SetActive(false);
                }
                
                Debug.Log("Data not present");
            }
        }
        else
        {
            Debug.Log("Bad equest");
            UiManager.instance._msgController.gameObject.SetActive(true);
            UiManager.instance._msgController.OnMessage(MsgController.eMsgType.noOrder);
        }
    }
    void OnShareButton()
    {
        UiManager.instance._shareInvoiceController.gameObject.SetActive(true);
        gameObject.SetActive(false);
        UiManager.instance._shareInvoiceController.SetValueShareInvoice(orderInfo,customerInfo);


    }
    void SetNextnPrevBtn(int m,int y)
    {

        if (m == latestInvoiceMonth && y == latestInvoiceYear)
        {
            nextBtn.SetActive(false);
            prevBtn.SetActive(true);
        }
        else
        {
            nextBtn.SetActive(true);
            prevBtn.SetActive(true);

        }

    }
    void OmSubmitBtn()
    {
      
      
            OnPaymentSubmit();
    }

    //public void OnCustomerSelected(int id)
    //{
    //    id = System.Int32.Parse(customerInfo.Id);
    //    gameObject.SetActive(false);
    //    UiManager.instance._orderController.gameObject.SetActive(true);
    //    UiManager.instance._orderController.monthlyCustomeScroller.OnBtnClicked(id);
    //}

    //public void OnSelectCustomerToShowOrderDetails()
    //{
    //    UiManager.instance._paidInvoiceController.OnCustomerSelected(System.Int32.Parse(customerInfo.Id));
    //}

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Button":
                OnBackButton();
                break;
            case "OnSubmit_Btn":
                OmSubmitBtn();
                break;
            case "OnShare_Btn":
                OnShareButton();
                break;
            case "OnPrevInvoice":
                OnSelectPrevInvoice();
                break;
            case "OnNextInvoice":
                OnSelectNextInvoice();
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class WPaymentDetailsRequestBody
{
    public string invoice_id;
    public string dateTime;
    public float paid_amount;

}

[Serializable]
public class PrevInvoiceByCustomey
{
    public string farmID;
    public int month;
    public int year;
    public string customerID;
}