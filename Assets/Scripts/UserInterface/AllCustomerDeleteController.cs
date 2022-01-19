using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCustomerDeleteController : MonoBehaviour
{
    public SG.InitOnStart initOnStart;
    public List<CustomerInfo> allCustomers;


    private void OnEnable()
    {
        OnRefresh();
    }

    public void OnRefresh()
    {
        GetAllCustomer();
        initOnStart.SetMaxScroolerItem(allCustomers.Count);
    }

    public void GetAllCustomer()
    {
        allCustomers = DairyApplicationData.Instance._farmCustomers.customerInfos;

    }

    void OnBackButton()
    {
        gameObject.SetActive(false);
        //UiManager.instance._mainMenuController.gameObject.SetActive(true);
        UiManager.instance._menuController.gameObject.SetActive(true);

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
    }
}
