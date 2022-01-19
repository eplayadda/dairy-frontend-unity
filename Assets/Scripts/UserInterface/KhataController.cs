using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;


public class KhataController : MonoBehaviour
{
    public SG.InitOnStart initOnStarts;

    private void OnEnable()
    {
        Debug.Log("Invoice for last month :: "+ DairyApplicationData.Instance.customerInvoice.customeList.Count);
        initOnStarts.SetMaxScroolerItem(DairyApplicationData.Instance.customerInvoice.customeList.Count);
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }
    }

    void OnBack()
    {
        UiManager.instance._khataController.gameObject.SetActive(false);
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch(pBtnName)
        {
            case "OnBack_Btn":
                OnBack();
                break;
            default:
                break;
        }
    }
}






