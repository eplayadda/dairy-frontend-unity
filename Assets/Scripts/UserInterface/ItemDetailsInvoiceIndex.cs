using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsInvoiceIndex : MonoBehaviour
{
    public Image _profileImage;
    public Text _name_Text;
    public Text _address_Text;
    public Text _id_Text;
    public Text _month_Text;
    public Text _milk_Text;
    public Text _total_Text;
    public Text _dues_Text;
    public Image _paidImage ;
    // public Button _paid_Button;
    string _Mobile;
    CustomeList mInvoice;
    CustomerInfo customerInfo;
   


    void ScrollCellIndex(int idx)
    {
       
        Debug.Log("Invoice List......  " + idx);
        mInvoice = DairyApplicationData.Instance.customerInvoice.customeList[idx];
        customerInfo = null;
        foreach (var item in DairyApplicationData.Instance._farmCustomers.customerInfos)
        {
            customerInfo = item;
            
            if (string.Equals(customerInfo.Id, mInvoice.customer_id))
            {
                Debug.Log(mInvoice.customer_id + "............ ==" + item.Id);
                break;
            }
            else
            {
                customerInfo = null;
            }
        }
       if(customerInfo == null)
       {
            Debug.Log("Destoring....");
            DairyApplicationData.Instance.customerInvoice.customeList.Remove(mInvoice);
            return;
       }
        BindDataInUI();
    }

   

    public void BindDataInUI()
    {
        int month = System.Convert.ToInt32(mInvoice.inv_for_month);
        string name = customerInfo.c_name;
        string mMonth = Utility.GetMonthInString(month, UiManager.instance.currLanguage); 
        string id = customerInfo.Id;
        string mobile = customerInfo.c_mob_number;
        string address = customerInfo.c_area;

        //string address = customerInfo.c_building + ", " + customerInfo.c_floor + " floor" + ", "
        //    + " road no:- " + customerInfo.c_road_number + ", " + customerInfo.c_area + ", " + customerInfo.c_pin_code;

        _Mobile = mobile;
        float milk = mInvoice.total_ltr_morning + mInvoice.total_ltr_evening;
        _name_Text.GetComponent<DairyText>().DairyString(name);
        _address_Text.GetComponent<DairyText>().DairyString(address);
        _month_Text.GetComponent<DairyText>().DairyString(mMonth);
        _milk_Text.text = milk.ToString();
        _total_Text.text = (mInvoice.amount - mInvoice.payment).ToString();
        _paidImage.enabled = false;
        _dues_Text.enabled = false;
        _id_Text.text = customerInfo.rank.ToString();
    }
    void OnContactCustomer()
    {
        Application.OpenURL("tel:" + _Mobile);
        Debug.Log("calling" + _Mobile);
    }

    void OnCustomer()
    {
        UiManager.instance._khataController.gameObject.SetActive(false);
        UiManager.instance._paidInvoiceController.gameObject.SetActive(true);
        UiManager.instance._paidInvoiceController.saveFirstTime = false;
        UiManager.instance._paidInvoiceController.SetInvoiceDetails(mInvoice, customerInfo,this);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnContact_Btn":
                OnContactCustomer();
                break;
                case "OnCustomer_Btn":
                OnCustomer();
                break;
            default:
                break;
        }
    }

}
