using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllOrderCustomerController : MonoBehaviour
{
    public SG.InitOnStart initOnStart;
    List<CustomerInfo> allCustomers;
    public List<CustomerInfo> activeCustomer = new List<CustomerInfo>();


    private void OnEnable()
    {
        OnActiveCustomer();
        initOnStart.SetMaxScroolerItem(activeCustomer.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }
    public void OnCustomerSelected(int custIndex)
    {
        gameObject.SetActive(false);
        UiManager.instance._orderController.monthlyCustomeScroller.OnBtnClicked(custIndex);
    }
    void OnActiveCustomer()
    {
        allCustomers = DairyApplicationData.Instance._farmCustomers.customerInfos;
        foreach (var customer in allCustomers)
        {
            if (customer.c_active)
            {
                activeCustomer.Add(customer);
            }
           
        }
    }

    void OnBackButton()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            default:
                break;

        }
    }

    public void OnBackBtnClicked()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        activeCustomer.Clear();
    }
}
