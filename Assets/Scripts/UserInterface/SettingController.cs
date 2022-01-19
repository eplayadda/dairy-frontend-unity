using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour {
    public Toggle isEnglish;
    public Toggle isHindi;
    // Use this for initialization
    private void OnEnable()
    {
        if (DairyApplicationData.Instance.SettingModel.language == "Hindi")
        {
            isEnglish.isOn = false;
            isHindi.isOn = true;
        }
        else
        {
            isEnglish.isOn = true;
            isHindi.isOn = false;
        }
          
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }

    }

    void OnDeleteCustomer()
    {
        gameObject.SetActive(false);
        UiManager.instance._allCustomerDeleteController.gameObject.SetActive(true);
        UiManager.instance._mainMenuController.gameObject.SetActive(false);
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
    }

    void OnBackButton()
    {
        SaveSetting();
        gameObject.SetActive(false);
       // UiManager.instance._mainMenuController.gameObject.SetActive(false);
        UiManager.instance._menuController.gameObject.SetActive(true);

    }

    void SaveSetting()
    {
        DairyApplicationData.Instance.Setting =JsonUtility.ToJson( DairyApplicationData.Instance.SettingModel);
    }
    public void onLanguageChanged()
    {
        if (isEnglish.isOn)
        {
            DairyApplicationData.Instance.SettingModel.language = "English";
        }
        else
        {
            DairyApplicationData.Instance.SettingModel.language = "Hindi";

        }
        UiManager.instance.SetFont();
    }
    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            case "OnDeleteCustomer_Btn":
                OnDeleteCustomer();
                break;
            default:
                break;

        }
    }
}
