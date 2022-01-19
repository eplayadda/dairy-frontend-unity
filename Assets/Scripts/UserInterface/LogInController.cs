using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class LogInController : MonoBehaviour
{
    public InputField _mob_number;
	public InputField _password;
	public Button _logIn;
    public Button _forgetPassword;
    public Text _worngMsgText;
    WLoginRequestBody mLoginRequest;
    WLoginResponse mLoginResponse;

    private void OnEnable()
    {
        CheckAllreadyLogin();
    }
    void CheckAllreadyLogin()
    {
        if (!string.IsNullOrEmpty(DairyApplicationData.Instance.UserID) && !string.IsNullOrEmpty(DairyApplicationData.Instance.User_password))
        {
            _mob_number.text = DairyApplicationData.Instance.UserID;
            _password.text = DairyApplicationData.Instance.User_password;
            OnLogIN();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void ResetData()
    {
        _mob_number.text = "";
        _password.text = "";
        _worngMsgText.text = "";
    }
    void OnLogIN()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        mLoginRequest = new WLoginRequestBody();
        mLoginRequest.mob_number = _mob_number.text;
        mLoginRequest.password = _password.text;
        string loginBodyStr = JsonUtility.ToJson(mLoginRequest);
        Debug.Log("---------" + loginBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_LogIn, "",loginBodyStr,LoginCallback); 
    }

    void LoginCallback(bool success, string response)
    {
            Debug.Log(success+"Login " +response);
        if (success)
        {
            //if(response.)
            //{

            UiManager.instance._loadingController.gameObject.SetActive(false);
            //}
            mLoginResponse = JsonUtility.FromJson<WLoginResponse>(response);
            DairyApplicationData.Instance.FarmID = mLoginResponse.Id;
            DairyApplicationData.Instance.UserID = mLoginResponse.mob_number;
            DairyApplicationData.Instance.User_password = mLoginResponse.password;
            OnLoginSuccess();
        }
        else
        {
            Debug.Log("............");
            UiManager.instance._loadingController.gameObject.SetActive(false);
            _worngMsgText.text ="Please correct id or password...";
            _password.text = "";
        }
    }

    public void OnLoginSuccess()
    {
        UiManager.instance.OnInt();
      //  DairyApplicationData.Instance.ForTesting();
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
    }

    void OnSinUp()
    {
        UiManager.instance._sinUpController.gameObject.SetActive(true);
        UiManager.instance._logInController.gameObject.SetActive(false);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnLogIN_Btn":
                OnLogIN();
                break;
            case "OnSinUp_Btn":
                OnSinUp();
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        ResetData();
    }
}

[System.Serializable]
public class WLoginRequestBody
{
    public string mob_number;
    public string password;
}

[System.Serializable]
public class WLoginResponse
{
    public string Id;
    public string mob_number;
    public string password;
}
