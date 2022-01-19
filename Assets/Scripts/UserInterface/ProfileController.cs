using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;
using DairyBusiness;
using System;

public class ProfileController : MonoBehaviour
{
    public Text _idText;
    public Image _profileImage;
    public InputField _name;
    public InputField _mobile;
    public InputField _floor;
    public InputField _buildingNmae;
    public InputField _roadNo;
    public InputField _area;
    public InputField _pincode;
    public InputField _landmarks;
    public InputField _rate;
    public InputField _advance;
    public InputField _dues;
    public GameObject _advanceLevel;
    public GameObject _duesLevel;
    public Toggle _morningToggle;
    public Toggle _eveningToggle;
    public Button _profileImageButton;
    public Button _submitButton;
    CustomerInfo mProfileRequest;
    public CustomerInfo mProfileResponse;
    public ItemDetailsCustomerIndex _scrollIndexCallback1;
    string idForEdit;
    Invoice invoiceLogic = new Invoice();
    public Text testing;
    public string language;
    public GameObject headerName_Lvl;
    public GameObject headerName_Edit_Lvl;
    Coroutine _coroutineProfile;


    private void OnEnable()
    {
        ResetData();
    }
    private void ResetData()
    {
       

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }



    }

    public void OnDueInputFieldValueChanged()
    {

        if (_dues.text != "" && _dues.text != "0")
        {
            _advance.text = "";
        }

    }
    public void OnAdvanceInputFieldValueChanged()
    {

        if (_advance.text != "" && _advance.text != "0")
        {
            _dues.text = "";
        }


    }
  
    void OnSubmit()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        mProfileRequest = new CustomerInfo();
        mProfileRequest.farm_id = DairyApplicationData.Instance.FarmID;
        mProfileRequest.Id = idForEdit;
        //_idText.text =  mProfileRequest.rank.ToString();
       
        mProfileRequest.c_name = _name.text;
        mProfileRequest.c_mob_number = _mobile.text;
        mProfileRequest.c_floor = _floor.text;
        mProfileRequest.c_building = _buildingNmae.text;
        mProfileRequest.c_road_number = _roadNo.text;
        mProfileRequest.c_area = _area.text;
        mProfileRequest.c_pin_code = _pincode.text;
        mProfileRequest.c_landmark = _landmarks.text;
        mProfileRequest.c_image_url = "";
        mProfileRequest.c_active = true;
        mProfileRequest.customerGenerated_date =   DairyApplicationData.todayMonth +"/1/" + DairyApplicationData.todayYear + " 2:22:22 PM";
        if (_morningToggle.isOn || !_eveningToggle.isOn)
        {
            mProfileRequest.c_sift = "Morning";
        }
        else
        {
            mProfileRequest.c_sift = "Evening";
        }
        if (_rate.text == "")
            _rate.text = "0";
        if (_dues.text == "")
            _dues.text = "0";
        if(_advance.text == "")
            _advance.text = "0";
        mProfileRequest.c_rate = int.Parse(_rate.text);
        mProfileRequest.c_preAdvance = float.Parse(_advance.text);
        mProfileRequest.c_preDue = float.Parse(_dues.text);
        _idText.text = mProfileResponse.rank.ToString();



        string profileBodyStr = JsonUtility.ToJson(mProfileRequest);
        Debug.Log("Profile String ...  " + profileBodyStr);
        _coroutineProfile = DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Profile, "", profileBodyStr, ProfileCallback);
    }

    void ProfileCallback(bool success, string response)
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);

        if (success)
        {
            mProfileResponse = JsonUtility.FromJson<CustomerInfo>(response);
            //   OnEditProfile(mProfileRequest);
            bool isEdit = false;
            Debug.Log(" ProfileCallback response" + response);
            int i = 0;
            if (DairyApplicationData.Instance._farmCustomers != null)
            {
                foreach (var item in DairyApplicationData.Instance._farmCustomers.customerInfos)
                {
                    if (item.Id == idForEdit)
                    {
                        DairyApplicationData.Instance._farmCustomers.customerInfos.Remove(item);
                        isEdit = true;
                        break;
                    }
                    i++;
                }

                if (isEdit)
                    DairyApplicationData.Instance._farmCustomers.customerInfos.Insert(i, mProfileResponse);
                else
                    DairyApplicationData.Instance._farmCustomers.customerInfos.Add(mProfileResponse);

            }
            UiManager.instance._allCustomercontroller.gameObject.SetActive(true);
            UiManager.instance._ProfileController.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("......Call back null......");
        }
    }

    public void OnEditProfile(CustomerInfo pcustomerInfo)
    {
        idForEdit = pcustomerInfo.Id;
        _idText.text = pcustomerInfo.rank.ToString();
        _name.text = pcustomerInfo.c_name;
        _mobile.text = pcustomerInfo.c_mob_number;
        _floor.text = pcustomerInfo.c_floor;
        _buildingNmae.text = pcustomerInfo.c_building;
        _roadNo.text = pcustomerInfo.c_road_number;
        _area.text = pcustomerInfo.c_area;
        _pincode.text = pcustomerInfo.c_pin_code;
        _landmarks.text = pcustomerInfo.c_landmark;
        _rate.text = pcustomerInfo.c_rate.ToString();
        if(invoiceLogic.IsInvoiceExist(pcustomerInfo.Id))
        {
            _advanceLevel.SetActive(false);
            _duesLevel.SetActive(false);
        }
        else
        {
            _dues.text = pcustomerInfo.c_preDue.ToString();
            _advance.text = pcustomerInfo.c_preAdvance.ToString();
        }
        

        if (pcustomerInfo.c_sift == "Morning")
        {
            _morningToggle.isOn = true;
        }
        else
        {
            _eveningToggle.isOn = true;
        }


    }

    void OnBackButton()
    {
        if (_coroutineProfile != null)
            StopCoroutine(_coroutineProfile);
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            case "OnSubmit_Btn":
                OnSubmit();
                break;
            case "OnContactList_Btn":
                OnContactList();
                break;
            default:
                break;
        }
    }

    private void OnContactList()
    {
        UiManager.instance._contactController.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _idText.text = "";
        _name.text = "";
        _mobile.text = "";
        _floor.text = "";
        _buildingNmae.text = "";
        _roadNo.text = "";
        _area.text = "";
        _pincode.text = "";
        _landmarks.text = "";
        _rate.text = "";
        _dues.text = "";
        _advance.text = "";
        idForEdit = "";
        _advanceLevel.SetActive(true);
        _duesLevel.SetActive(true);
    }
}

