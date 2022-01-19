using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using DairyBusiness;
using ePlayAdda.Dairy;

public class ItemDetailsCustomerIndex : MonoBehaviour
{
    // public Image image;
    public Image _profileImage;
    public Text _name_Text;
    public Text _address_Text;
    public Text _mobile_Text;
    public Text _Id_Text;
    public Toggle _khataCreate_ButtonToggle;
    public GameObject _detailsBG;
    public CustomerInfo customerInfos;
    public string _name;
    List<CustomerInfo> allCustomers;
    AllCustomercontroller mAllCustomercontroller;
    ProfileController mProfileController;
    public GameObject _active_Level;
    public GameObject _deactive_Level;
    Coroutine _coroutineUpdateStatus;
    int customerIndex;


    void ScrollCellIndex(int idx)
    {
        customerIndex = idx;
        mAllCustomercontroller = UiManager.instance._allCustomercontroller;
        mProfileController = UiManager.instance._ProfileController;
        CustomerInfo customerInfo = null;
        if (mAllCustomercontroller.isShowActiveCust)
        {
            customerInfo = mAllCustomercontroller.activeCustomer[idx];
        }
        else
        {
            customerInfo = mAllCustomercontroller.deActiveCustomer[idx];

        }

        customerInfos = customerInfo;
        customerInfos.Id = customerInfo.Id;
        _name = customerInfo.c_name;
        string mobile = customerInfo.c_mob_number;
        string floor = customerInfo.c_floor;
        string building = customerInfo.c_building;
        string road = customerInfo.c_road_number;
        string area = customerInfo.c_area;
        string landmark = customerInfo.c_landmark;
        string pincode = customerInfo.c_pin_code;
        // bool toggle = customerInfo.c_active;
        //string address = area; 

        _Id_Text.text = (customerInfos.rank).ToString();
        _name_Text.GetComponent<DairyText>().DairyString(_name);
        _mobile_Text.text = mobile;
        _address_Text.GetComponent<DairyText>().DairyString(area);

        if (mAllCustomercontroller.isShowActiveCust)
        {
            _khataCreate_ButtonToggle.isOn = true;
        }
        else
        {
            _khataCreate_ButtonToggle.isOn = false;

        }


    }
    public void OnSelectCustomerToShowOrderDetails()
    {
        UiManager.instance._allCustomercontroller.OnCustomerSelected(customerIndex);
       
    }
    void OnDetails()
    {
        if(mAllCustomercontroller.isShowActiveCust == true)
        {
            _active_Level.SetActive(false);
            _deactive_Level.SetActive(true);
        }
        else
        {
            _active_Level.SetActive(true);
            _deactive_Level.SetActive(false);
        }
        SetEnableDetailsPanel(true);

    }

    void OnEdit()
    {
        SetEnableDetailsPanel(true);
        mAllCustomercontroller.gameObject.SetActive(false);
        mProfileController.gameObject.SetActive(true);
        mProfileController.OnEditProfile(customerInfos);

    }

    void OnRemove()
    {
        if (!mAllCustomercontroller.isShowActiveCust)
            UpdateStatushToserver(true);
        else
            CheckOrderExist();
    }

    void CheckOrderExist()
    {
        Order order = new Order();
        bool orderExist = order.IsOrderExist(customerInfos.Id);
        if(orderExist)
        {
            Debug.Log(" Can not delete existing order... ");
            UiManager.instance._msgController.gameObject.SetActive(true);
            UiManager.instance._msgController.OnMessage(MsgController.eMsgType.delete);

        }
        else
        {
           
            UpdateStatushToserver(false);
        }
    }
    void UpdateStatushToserver(bool status)
    {
        customerInfos.c_active = status;
        string profileBodyStr = JsonUtility.ToJson(customerInfos);
        Debug.Log("Profile String ...  " + profileBodyStr);
        _coroutineUpdateStatus =  DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Profile, "", profileBodyStr, ProfileCallback);
        UiManager.instance._loadingController.gameObject.SetActive(true);

    }
    void ProfileCallback(bool success, string response)
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);

        if (success)
        {
            mAllCustomercontroller.RefreshSliderItem();
            Debug.Log("Delete");
            Debug.Log("......Call back Success......");

        }
        else
        {
            Debug.Log("......Call back null......");
        }
    }
    void OnBack_Btn()
    {
        if (_coroutineUpdateStatus != null)
            StopCoroutine(_coroutineUpdateStatus);
        SetEnableDetailsPanel(false);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnDetails_Btn":
                OnDetails();
                break;
            case "OnEdit_Btn":
                OnEdit();
                break;
            case "OnRemove_Btn":
                OnRemove();
                break;
            case "OnBack_Btn":
                OnBack_Btn();
                break;
            default:
                break;
        }
    }
    void SetEnableDetailsPanel(bool status)
    {
        if (mAllCustomercontroller == null)
            return;
        if (mAllCustomercontroller.customerOptionPanel != null)
            mAllCustomercontroller.customerOptionPanel.SetActive(false);
        _detailsBG.SetActive(status);
        if(status)
           mAllCustomercontroller.customerOptionPanel = _detailsBG;
        else
            mAllCustomercontroller.customerOptionPanel = null;
    }
    private void OnDisable()
    {
        SetEnableDetailsPanel(false);
    }
}
