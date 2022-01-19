using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public Text dateTxt;
    public Button _settings_Btn;
    public Text _dairy_Name;
    public Image _logo;
    DairyProfile dairyProfile;


    private void OnEnable()
    {
        ResetData();
        DairyHeader();
    }
    void ResetData()
    {
        dateTxt.text = "MMYY :: " + DairyApplicationData.todayMonth + "/" + DairyApplicationData.todayYear;
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
        if (DairyApplicationData.Instance.dairyProfile == null)
            return;
        dairyProfile = DairyApplicationData.Instance.dairyProfile;
        _dairy_Name.text = dairyProfile.dairy_name;
    }

    void OnAccount()
    {
        UiManager.instance._accountController.gameObject.SetActive(true);
        UiManager.instance._menuController.gameObject.SetActive(false);

    }
    
    void OnKhataMenu()
    {
        UiManager.instance._khataMenuController.gameObject.SetActive(true);
    }

    void OnGraph()
    {
        UiManager.instance._graphController.gameObject.SetActive(true);
    }

    void OnSettings()
    {
        UiManager.instance._settingController.gameObject.SetActive(true);
       // UiManager.instance._mainMenuController.gameObject.SetActive(false);
        UiManager.instance._menuController.gameObject.SetActive(false);

    }

    void OnHelp()
    {
        UiManager.instance._helpController.gameObject.SetActive(true);
    }

    void OnBackButton()
    {
       gameObject.SetActive(false);
        UiManager.instance._mainMenuController.gameObject.SetActive(false);
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
    }
    void IncressMonth()
    {
        DairyApplicationData.todayMonth++;
        PlayerPrefs.SetInt("todaymonth"+DairyApplicationData.Instance.FarmID, DairyApplicationData.todayMonth);
        dateTxt.text = "MMYY :: " + DairyApplicationData.todayMonth + "/" + DairyApplicationData.todayYear;
    }

    void OnLogOut()
    {
        DairyApplicationData.Instance.FarmID = "";
        DairyApplicationData.Instance.UserID = "";
        DairyApplicationData.Instance.User_password = "";
        UiManager.instance._logInController.gameObject.SetActive(true);
        UiManager.instance._mainMenuController.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            case "LogOut":
                OnLogOut();
                break;
            case "IncressMonth":
                IncressMonth();
                break;
            case "OnAccount_Btn":
                OnAccount();
                break;
            case "OnKhataMenu_Btn":
                OnKhataMenu();
                break;
            case "OnGraph_Btn":
                OnGraph();
                break;
            case "OnSettings_Btn":
                OnSettings();
                break;
            case "OnHelp_Btn":
                OnHelp();
                break;
            default:
                break;
        }
    }
}
