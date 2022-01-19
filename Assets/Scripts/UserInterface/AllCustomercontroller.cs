using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class AllCustomercontroller : MonoBehaviour
{
    public SG.InitOnStart initOnStart;
    public Toggle _isActive_Toggle;
    List<CustomerInfo> allCustomers ;
    public List<CustomerInfo> activeCustomer = new List<CustomerInfo>();
    public List<CustomerInfo> deActiveCustomer = new List<CustomerInfo>();
    public List <CustomerInfo> customerInfo;
    public bool isShowActiveCust;
    public GameObject customerOptionPanel;
   
    private void OnEnable()
    {
        RefreshSliderItem();
    }
    public void RefreshSliderItem()
    {
       // _isActive_Toggle.isOn = true;
        OnActiveCustomer();
        OnClickActive();
    }
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }

    public void OnClickActive()
    {
        if (_isActive_Toggle.isOn)
        {
            isShowActiveCust = true;
            
            initOnStart.SetMaxScroolerItem(activeCustomer.Count);
        }
        else
        {
            isShowActiveCust = false;
           
            initOnStart.SetMaxScroolerItem(deActiveCustomer.Count);
        }
    }
    void OnAddNewCustomer()
    {
        gameObject.SetActive(false);
        UiManager.instance._ProfileController.gameObject.SetActive(true);
    }
    void OnActiveCustomer()
    {
        activeCustomer.Clear();
        deActiveCustomer.Clear();
        if(DairyApplicationData.Instance._farmCustomers!=null)
           allCustomers = DairyApplicationData.Instance._farmCustomers.customerInfos;
        if(allCustomers != null)
        {
            foreach (var customer in allCustomers)
            {
                if (customer.c_active)
                {
                    activeCustomer.Add(customer);
                }
                else
                {
                    deActiveCustomer.Add(customer);
                }
            }
        }
      
    }
    public void OnCustomerSelected(int custIndex)
    {
        gameObject.SetActive(false);
        UiManager.instance._orderController.gameObject.SetActive(true);
        UiManager.instance._orderController.monthlyCustomeScroller.OnBtnClicked(custIndex);
    }

    void OnBackButton()
    {
        UiManager.instance.OnInt();
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnKhataCreate_Btn":
              //  OnKhataCreate();
                break;
            case "OnBack_Button":
                OnBackButton();
                break;
            case "OnAddNewCustomer":
                OnAddNewCustomer();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        activeCustomer.Clear();
        deActiveCustomer.Clear();
    }
}

