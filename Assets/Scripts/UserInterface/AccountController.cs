using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class AccountController : MonoBehaviour
{
    public Image Logo;
    public InputField InputField_Name;
    public InputField InputField_DairyName;
    public InputField InputField_Address;
    public InputField inputField_Mobile;

    DairyProfile w_dairyProfile;
    DairyByFramID _dairyByFram;

    private void OnEnable()
    {
        w_dairyProfile = DairyApplicationData.Instance.dairyProfile;
        if (w_dairyProfile != null)
        {
            InputField_Name.text = w_dairyProfile.owner_name;
            InputField_DairyName.text = w_dairyProfile.dairy_name;
            inputField_Mobile.text = w_dairyProfile.mob_number;
            InputField_Address.text = w_dairyProfile.address;
        }

    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }

    void OnAccount()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        w_dairyProfile = new DairyProfile();
        w_dairyProfile.farm_id = DairyApplicationData.Instance.FarmID;
        if (DairyApplicationData.Instance.dairyProfile != null)
            w_dairyProfile.Id = DairyApplicationData.Instance.dairyProfile.Id;
        w_dairyProfile.mob_number = inputField_Mobile.text;
        w_dairyProfile.owner_name = InputField_Name.text;
        w_dairyProfile.dairy_name = InputField_DairyName.text;
        w_dairyProfile.address = InputField_Address.text;
        //w_dairyProfile.logo_id = Logo;

        string accountBodyStr = JsonUtility.ToJson(w_dairyProfile);
        Debug.Log("---------" + accountBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Account, "", accountBodyStr, AccountCallback);
    }

    void AccountCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            DairyApplicationData.Instance.dairyProfile = JsonUtility.FromJson<DairyProfile>(response);
        }
        else
        {
            Debug.Log("............");

        }
    }
    void OnBackButton()
    {
        UiManager.instance._menuController.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void OnSubmitAccount()
    {
        OnAccount();
        UiManager.instance._menuController.gameObject.SetActive(true);
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
                OnSubmitAccount();
                break;
            default:
                break;

        }
    }
}

[System.Serializable]
public class DairyProfile
{
    public string Id;
    public string farm_id;
    public string mob_number;
    public string dairy_name;
    public string owner_name;
    public string address;
    public int logo_id;
}

[System.Serializable]
public class DairyByFramID
{
    public string farm_id;
}