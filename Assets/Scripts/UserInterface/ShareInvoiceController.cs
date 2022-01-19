using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareInvoiceController : MonoBehaviour
{
    public Image _dairy_logo;
    public Text _owner_nameText;
    public Text _dairy_nameText;
    public Text _phone_numberText;
    public Text _dairy_addressText;
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
    public Text _remaing_Amount_Text;
    public Text _id_Text;

    CustomeList mInvoice;
    CustomerInfo customerInfo;

    int invoiceMonth;
    int invoiceYear;
    public GameObject _dues_Level;
    public GameObject _advance_Level;
    List<GameObject> paimentDetailsGo = new List<GameObject>();
    public GameObject paidDetailsPrefab;
    public Transform parentForPaidDetails;
    public Transform reamaningAmountPanel;
    CustomeList orderInfo;
    DairyProfile dairyProfile;

    private void OnEnable()
    {
        DairyHeader();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }

    }

    void DairyHeader()
    {
        dairyProfile = DairyApplicationData.Instance.dairyProfile;
        _owner_nameText.text = dairyProfile.owner_name;
        _dairy_nameText.text = dairyProfile.dairy_name;
        _phone_numberText.text = dairyProfile.mob_number;
        _dairy_addressText.text = dairyProfile.address;
    }
    void ChangeFont()
    {
        Font font = UiManager.instance.currentFont;
        _nameText.font = font;
        _addressText.font = font;
        _month_Text.font = font;
        _dues_Text.font = font;
    }

    public void SetValueShareInvoice(CustomeList pOrderInfo, CustomerInfo pCustomerInfo)
    {
        orderInfo = pOrderInfo;
        ChangeFont();
        _nameText.text = pCustomerInfo.c_name;
        _id_Text.text = pCustomerInfo.rank.ToString();
        _addressText.text = pCustomerInfo.c_area;
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
        _month_Text.text = Utility.GetMonthInString(month, UiManager.instance.currLanguage);
        _year_Text.text = pOrderInfo.inv_for_year;
        //float payment = pOrderInfo.payment;
        _paid_Text.text = pOrderInfo.payment.ToString();
        DisplayAllPaymentDetails();
        float remainingAmount = total - pOrderInfo.payment;
        _remaing_Amount_Text.text = remainingAmount.ToString();
        
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
            paidDetailsItems._date.text = "( " +dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year + " )";
            paidDetailsItems._amount.text = item.payment_amount.ToString();
            paimentDetailsGo.Add(go);
        }
        reamaningAmountPanel.SetAsLastSibling();
    
}

   

    void OnBackButton()
    {
        UiManager.instance._shareInvoiceController.gameObject.SetActive(false);
        UiManager.instance._paidInvoiceController.gameObject.SetActive(true);
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
}
