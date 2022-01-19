using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class OrderController : MonoBehaviour
{
    public Image _Logo;
    public Text _Owner_name;
    public Text _dairy_Name;
    public Text _phoneNumber;
    public Text _address;

    WOrderByIdRequestBody mOrderByIdRequestBody;
    WOrderByIdResponce mOrderByIdResponce;
    OrderInfo mOrderInfo;
    public List<SubOrderController> subOrderController;
    public ScrollManager monthlyCustomeScroller;
    Coroutine orderCoroutine;
    List<CustomerMonthlyDetails> customersMonthlyDetails;
    int customerContainsOrderCount = 0;
    public GameObject dontHaveCustomerPanel;
    public GameObject customerSliderPanel;
    DairyProfile dairyProfile;
    public GameObject MilkFeed;
    public bool IsMilkFeedEnable;


    private void OnEnable()
    {
        BindCustomerOrders();
        DairyHeader();
    }

	void Update ()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackBtnClicked();
        }
    }

    void DairyHeader()
    {
        if (DairyApplicationData.Instance.dairyProfile == null)
            return;
        dairyProfile = DairyApplicationData.Instance.dairyProfile;
        _Owner_name.text = dairyProfile.owner_name;
        _dairy_Name.text = dairyProfile.dairy_name;
        _phoneNumber.text = dairyProfile.mob_number;
        _address.text = dairyProfile.address;
    }


    public void BindCustomerOrders()
    {
        customersMonthlyDetails = DairyApplicationData.Instance.customersMonthlyDetails;
        if (DairyApplicationData.Instance._farmCustomers != null)
        {
            List<CustomerInfo> allCustomer = DairyApplicationData.Instance._farmCustomers.customerInfos;
            List<OrderInfo> allOrder = DairyApplicationData.Instance.orderList.order_Infos;
            List<CustomerInfo> activeCustomer = new List<CustomerInfo>();
            List<OrderInfo> custOrderInfos = new List<OrderInfo>();
            foreach (var currCustomer in allCustomer)
            {
                if (currCustomer.c_active)
                {
                    activeCustomer.Add(currCustomer);
                    CustomerMonthlyDetails customerMonthlydetail = new CustomerMonthlyDetails();
                    customerMonthlydetail.customerInfo = currCustomer;
                    customersMonthlyDetails.Add(customerMonthlydetail);

                }

            }
            foreach (CustomerInfo currActiveCutomer in activeCustomer)
            {

                List<OrderInfo> myOrder = allOrder.Where(a => a.o_customer_id == currActiveCutomer.Id).ToList();
                customersMonthlyDetails[customerContainsOrderCount].orderInfos = myOrder;
                customerContainsOrderCount++;
            }
            SetDataToSubOrderPanel();
        }
        else
        {
            Debug.Log("You don't have sufficient customer..." + customersMonthlyDetails.Count);
            dontHaveCustomerPanel.SetActive(true);
            customerSliderPanel.SetActive(false);
        }
      
    }

    void FlitterByNoOrder()
    {
        List<CustomerMonthlyDetails> cm = new List<CustomerMonthlyDetails>();
        foreach (var item in customersMonthlyDetails)
        {
            if (item.orderInfos.Count == 0)
                cm.Add(item);
        }
        foreach (var item in cm)
        {
            customersMonthlyDetails.Remove(item);
        }
    }

    void SetDataToSubOrderPanel()
    {
        if (!DairyApplicationData.isLastMonthInvoiceGenerated)
            FlitterByNoOrder();
        dontHaveCustomerPanel.SetActive(false);
        customerSliderPanel.SetActive(true);
        switch (customersMonthlyDetails.Count)
        {
            case 0:
                Debug.Log("You don't have sufficient customer..." + customersMonthlyDetails.Count);
                //dontHaveCustomerPanel.SetActive(true);
                customerSliderPanel.SetActive(false);
                break;
            case 1:
                monthlyCustomeScroller.SetMaxScrollerItem(customersMonthlyDetails.Count);
                subOrderController[0].SetMonthlyDetails(customersMonthlyDetails[0], 0);
                subOrderController[1].SetMonthlyDetails(customersMonthlyDetails[0], 0);
                subOrderController[2].SetMonthlyDetails(customersMonthlyDetails[0], 0);
                break;
            case 2:
                monthlyCustomeScroller.SetMaxScrollerItem(customersMonthlyDetails.Count);
                subOrderController[0].SetMonthlyDetails(customersMonthlyDetails[1], 1);
                subOrderController[1].SetMonthlyDetails(customersMonthlyDetails[0], 0);
                subOrderController[2].SetMonthlyDetails(customersMonthlyDetails[1], 1);
                break;
            default:
                monthlyCustomeScroller.SetMaxScrollerItem(customersMonthlyDetails.Count);
                subOrderController[0].SetMonthlyDetails(customersMonthlyDetails[customersMonthlyDetails.Count - 1], customersMonthlyDetails.Count - 1);
                subOrderController[1].SetMonthlyDetails(customersMonthlyDetails[0], 0);
                subOrderController[2].SetMonthlyDetails(customersMonthlyDetails[1], 1);
                break;
        }
    }
    public void SetData(SubOrderController panalID, int DataID)
    {
        panalID.SetMonthlyDetails(customersMonthlyDetails[DataID],DataID);
    }

   

    void OnMenuButton()
    {
        UiManager.instance._allOrderCustomerController.gameObject.SetActive(true);
    }
    
    void OnMenuBackButton()
    {
        UiManager.instance._allOrderCustomerController.gameObject.SetActive(false);
    }

    void OnHomeButton()
    {
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
        UiManager.instance._orderController.gameObject.SetActive(false);
    }

    void OnAddNewCustomer()
    {
        gameObject.SetActive(false);
        UiManager.instance._ProfileController.gameObject.SetActive(true);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackBtnClicked();
                break;
            case "OnShare_Btn":
               // OnShareScreenShot();
                break;
            case "OnMenu_Btn":
                OnMenuButton();
                break;
            case "OnMenuBack_Btn":
                OnMenuBackButton();
                break;
            case "OnHome_Btn":
                OnHomeButton();
                break;
            case "OnAddNewCustomer_Btn":
                OnAddNewCustomer();
                break;
            default:
                break;
        }
    }
    public void OnBackBtnClicked()
    {
        gameObject.SetActive(false);
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
      //  StopCoroutine(orderCoroutine);
    }
    private void OnDisable()
    {
        customersMonthlyDetails.Clear();
        customerContainsOrderCount = 0;
        //DairyApplicationData.Instance.customersMonthlyDetails.Clear();
    }
}


[System.Serializable]
public class WOrderByIdRequestBody
{
    public string order_id;
}

[System.Serializable]
public class WOrderByIdResponce
{
    public string _id;
    public string o_sift;
    public string farmID;
    public int o_date;
    public int o_month;
    public int o_year;
    public string o_customer_id;
    public int o_quantity;
}

[System.Serializable]
public class WOrderByMonthRequestBody
{
    public string farmID;
    public int month;
    public int year;
    public string customerID;
}

[System.Serializable]
public class OrderInfo
{
    public string Id;
    public string o_sift;
    public string farmID;
    public int o_date;
    public int o_month;
    public int o_year;
    public string o_customer_id;
    public float o_quantity;
}

[System.Serializable]
public class OrderList
{
    public List<OrderInfo> order_Infos;
}

public class CustomerMonthlyDetails
{
    public int month;
    public int year;
    public CustomerInfo customerInfo;
    public List<OrderInfo> orderInfos = new List<OrderInfo>();
}

