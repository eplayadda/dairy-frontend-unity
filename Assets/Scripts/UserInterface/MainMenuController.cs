using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;
using System.Linq;

public class MainMenuController : MonoBehaviour
{
    Coroutine _coroutineLastMonthInvoice;
    Coroutine _coroutineGetAllCustomer;
    Coroutine _coroutineGetAllInvoice;
    Coroutine _coroutineShowOrder;

    CustomerInfoRequestBody mCustomerInfoRequestBody;
    WAllInvoiceRequestBody mAllInvoiceRequestBody;
    CustomerInvoice mcustomerInvoice;
    List<CustomerMonthlyDetails> customersMonthlyDetails;
    WOrderByMonthRequestBody mOrderByMonthRequestBody;
    DairyByFramID w_dairyByFramID;

    private void OnEnable()
    {
        if(DairyApplicationData.allowRefresh)
        {
            IsLastMonthInvoiceGenerated();
            GetDairyProfile();
            DairyApplicationData.allowRefresh = false;
        }
       
        
    }


	// Update is called once per frame
	void Update ()
    {
        OnBackBtnClicked();
		
	}

    void OnAddNewCustomer()
    {
        gameObject.SetActive(false);
        UiManager.instance._ProfileController.gameObject.SetActive(true);
        //UiManager.instance._contactController.gameObject.SetActive(true);
    }

     void OnMilkOrder()
    {
        gameObject.SetActive(false);
        UiManager.instance._orderController.gameObject.SetActive(true);
    }

     void OnKhata()
    {
        gameObject.SetActive(false);
        UiManager.instance._khataController.gameObject.SetActive(true);
    }

     void OnAllCostomer()
    {
        gameObject.SetActive(false);
        UiManager.instance._allCustomercontroller.gameObject.SetActive(true);

    }

    void OnChara()
    {
        gameObject.SetActive(false);
        UiManager.instance._charaController.gameObject.SetActive(true);

    }

    void OnMenuButton()
    {
        UiManager.instance._menuController.gameObject.SetActive(true);

    }

    void OnShopButton()
    {
        UiManager.instance._shopController.gameObject.SetActive(true);
        UiManager.instance._mainMenuController.gameObject.SetActive(false);

    }

    void IsLastMonthInvoiceGenerated()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        InvoiceRequestBody invoiceRequestBody = new InvoiceRequestBody();
        invoiceRequestBody.farm_id = DairyApplicationData.Instance.FarmID;
        List<int> MMYY = Utility.ConverInMMYY(DairyApplicationData.todayMonth -1, DairyApplicationData.todayYear);
        invoiceRequestBody.inv_for_month = MMYY[0];
        invoiceRequestBody.inv_for_year = MMYY[1];
        string invoiceRequestBodyStr = JsonUtility.ToJson(invoiceRequestBody);
        Debug.Log("All Customer String .... " + invoiceRequestBodyStr);
        _coroutineLastMonthInvoice = DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_IS_Invoice_Created, "", invoiceRequestBodyStr, IsLastMonthInvoiceGeneratedCallBack);
    }
    
    void IsLastMonthInvoiceGeneratedCallBack(bool success, string response)
    {
        if (success)
        {
           DairyApplicationData.Instance.checkOutDone.Clear();
            Debug.Log(response);
            DairyApplicationData.Instance.orderList = JsonUtility.FromJson<OrderList>(response);
            if (DairyApplicationData.Instance.orderList.order_Infos.Count>0)
            {
                DairyApplicationData.isLastMonthInvoiceGenerated = false;
            }
            else
            {
                DairyApplicationData.isLastMonthInvoiceGenerated = true;
                OnShowOrder();
            }
            GetAllCustomer();


        }
        else
        {
            //testing
            DairyApplicationData.isLastMonthInvoiceGenerated = true;
            GetAllCustomer();
        }
    }

    void GetAllCustomer()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        mCustomerInfoRequestBody = new CustomerInfoRequestBody();
        mCustomerInfoRequestBody.farm_id = DairyApplicationData.Instance.FarmID;
        string customerBodyStr = JsonUtility.ToJson(mCustomerInfoRequestBody);
        Debug.Log("All Customer String .... " + customerBodyStr);
        _coroutineGetAllCustomer =  DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_ALL_Customer, "", customerBodyStr, AllCustomerCallback);
    }

    void AllCustomerCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            DairyApplicationData.Instance._farmCustomers = JsonUtility.FromJson<FarmCustomers>(response);
            Debug.Log("All CustomerList::" + DairyApplicationData.Instance._farmCustomers.customerInfos.Count);
            DairyApplicationData.Instance._farmCustomers.ShortCustomer();
        }
        else
        {
            Debug.Log("............");
        }
        OnAllInvoiceList();

    }




    void OnAllInvoiceList()
    {

        mAllInvoiceRequestBody = new WAllInvoiceRequestBody();
        mAllInvoiceRequestBody.farm_id = DairyApplicationData.Instance.FarmID;
        string AllInvoiceStr = JsonUtility.ToJson(mAllInvoiceRequestBody);
        Debug.Log("All Invoice String ,,,,, " + AllInvoiceStr);

        _coroutineGetAllInvoice =  DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_All_Invoice, "", AllInvoiceStr, AllInvoicecallback);
    }

    void AllInvoicecallback(bool success, string response)
    {
        if (success)
        {
            DairyApplicationData.Instance.customerInvoice = JsonUtility.FromJson<CustomerInvoice>(response);
            Debug.Log("invoice list  ............" + DairyApplicationData.Instance.customerInvoice.customeList.Count);
        }
        else
        {
            Debug.Log("............");

        }

    }
    void OnShowOrder()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        customersMonthlyDetails = DairyApplicationData.Instance.customersMonthlyDetails;

        mOrderByMonthRequestBody = new WOrderByMonthRequestBody();
        mOrderByMonthRequestBody.farmID = DairyApplicationData.Instance.FarmID;
        mOrderByMonthRequestBody.customerID = "";
        mOrderByMonthRequestBody.month = DairyApplicationData.todayMonth;
        mOrderByMonthRequestBody.year = DairyApplicationData.todayYear;

        string orderBodyStr = JsonUtility.ToJson(mOrderByMonthRequestBody);
        Debug.Log("order string .... " + orderBodyStr);
        _coroutineShowOrder = DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_OrderListBy_Month, "", orderBodyStr, OrderCallback);
    }

    void OrderCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            DairyApplicationData.Instance.orderList = JsonUtility.FromJson<OrderList>(response);
        }
        else
        {
            Debug.Log("............");
        }
    }

    void GetDairyProfile()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        w_dairyByFramID = new DairyByFramID();
        w_dairyByFramID.farm_id = DairyApplicationData.Instance.FarmID;
        string dairyByFramIDBodyStr = JsonUtility.ToJson(w_dairyByFramID);
        Debug.Log("---------" + dairyByFramIDBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Account_GetFarmID, "", dairyByFramIDBodyStr, DairyByFramIDCallback);
    }

    void DairyByFramIDCallback(bool success, string response)
    {
        if (success)
        {
            if (response != null && response != "null")
            {
                UiManager.instance._loadingController.gameObject.SetActive(false);
                DairyApplicationData.Instance.dairyProfile = JsonUtility.FromJson<DairyProfile>(response);

            }
        }
        else
        {
            Debug.Log("............");

        }
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "AddNewCustomer_Btn":
                OnAddNewCustomer();
                break;
            case "AllCustomer_Btn":
                OnAllCostomer();
                break;
            case "MilkOrder_Btn":
                OnMilkOrder();
                break;
            case "Khata_Btn":
                OnKhata();
                break;
            case "Chara_Btn":
                OnChara();
                break;
            case "Menu_Btn":
                OnMenuButton();
                break;
            case "Shop_Btn":
                OnShopButton();
                break;
            default:
                break;
        }
    }

    public void OnBackBtnClicked()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_coroutineLastMonthInvoice != null)
                StopCoroutine(_coroutineLastMonthInvoice);
            if (_coroutineGetAllCustomer != null)
                StopCoroutine(_coroutineGetAllCustomer);
            if (_coroutineGetAllInvoice != null)
                StopCoroutine(_coroutineGetAllInvoice);
            if (_coroutineShowOrder != null)
                StopCoroutine(_coroutineShowOrder);
            Application.Quit();
        }
    }
}

[System.Serializable]
public class CustomerInfoRequestBody
{
    public string farm_id;
}

[System.Serializable]
public class CustomerInfo
{
    public string Id;
    public string farm_id;
    public string c_name;
    public string c_mob_number;
    public string c_floor;
    public string c_building;
    public string c_road_number;
    public string c_area;
    public string c_pin_code;
    public string c_landmark;
    public string c_image_url;
    public string c_sift;
    public bool c_delete;
    public bool c_active;
    public int c_rate;
    public float c_preAdvance;
    public float c_preDue;
    public int rank;
    public string customerGenerated_date;
}
[System.Serializable]
public class FarmCustomers
{
    public List<CustomerInfo> customerInfos ;
    public void ShortCustomer()
    {
        //customerInfos = customerInfos.OrderBy(o => o.rank).ToList();

    }
}

[System.Serializable]
public class WAllInvoiceRequestBody
{
    public string farm_id;
}

[System.Serializable]
public class CustomeList
{
    public string Id;
    public string customer_id;
    public string farm_id;
    public string inv_for_month;
    public string inv_for_year;
    public string inv_generate_date;
    public int rate_per_ltr;
    public float total_ltr_morning;
    public float total_ltr_evening;
    public float advanceAmount;
    public float dueAmount;
    public float amount;
    public float payment;
    public List<PaymentDetail> payment_details;
}

[System.Serializable]
public class PaymentDetail
{
    public string payment_date;
    public float payment_amount;
}

[System.Serializable]
public class CustomerInvoice
{
    public List<CustomeList> customeList  =new List<CustomeList>();
}