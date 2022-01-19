using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ePlayAdda.Dairy;
using System;

public class InvoiceController : MonoBehaviour
{
    WGetInvoiceByMonthRequestBody mGetInvoiceByMonth;
    //CustomerInvoice mcustomerInvoice;

    private void OnEnable()
    {
        OnAddInvoice();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnAddInvoice()
    {
        mGetInvoiceByMonth = new WGetInvoiceByMonthRequestBody();
        mGetInvoiceByMonth.farm_id = DairyApplicationData.Instance.FarmID;

        string InvoiceMonthStr = JsonUtility.ToJson(mGetInvoiceByMonth);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Get_InvoiceBy_Month, "", InvoiceMonthStr, GetInvoiceCallback);

    }

    void GetInvoiceCallback(bool success, string response)
    {
        if (success)
        {
            //UiManager.instance._loadingController.gameObject.SetActive(false);
          //  mcustomerInvoice = JsonUtility.FromJson<CustomerInvoice>(response);
          //  Debug.Log("Responce....." + mcustomerInvoice);
        }
        else
        {
            Debug.Log("............");
        }
    }
}



public class WGetInvoiceByMonthRequestBody
{
    public string customer_id;
    public string farm_id;
    public string inv_for_month;
    public string inv_for_year;
}



