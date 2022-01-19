using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class SinUpController : MonoBehaviour
{
    public enum eMsgType
    {
        none,
        allready_Exits,
        wrongOTp,
        timerUp
    }
    public Text Msg_Text;

    public InputField _userID;
	public InputField _password;
	public InputField _confrimPassword;
	public InputField _OTP_1;
	public InputField _OTP_2;
	public InputField _OTP_3;
	public InputField _OTP_4;
	public Button _sinUpButton;
	public Button _loginButton;
	public Button _OTP_Send_Button;
	public Button _OTP_Cancel_Button;
	public Button _OTP_Resend_Button;
	public Button _OTP_OK_Button;
	public Button _forget_Password_Button;
    public Text _mobile_Lvl;
    public Text _OTP_Lvl;
    public Text _password_Lvl;
    public Text _ReEnter_password_Lvl;
    public Text _Set_password_Lvl;
    public GameObject _Timer_Lvl;
    public Text _TimerTxt;
   // public Text _msg_Text;
    public Text _otpMsgTxt;
    MobileIfno w_mobileIfno;
    OtpInfo w_otpInfo;
    Farm w_farm;
    int maxOTPTime =600;
    int timeInSecond;
    string wrongOTPMsg ="Wrong OTP";
    string otpTimeOverMsg="Time over Resend OTP";
    private void OnEnable()
    {
        OnInit();
        MobileVerficationUI();
    }
    public void OnMessage(eMsgType pMsgType)
    {
        switch (pMsgType)
        {
            case eMsgType.none:
                break;
            case eMsgType.allready_Exits:
                Msg_Text.text = "Mobile number already exist...";
                break;
            case eMsgType.wrongOTp:
                Msg_Text.text = " Please enter correct OTP...";
                break;
            case eMsgType.timerUp:
                Msg_Text.text = " Time Up! Please Resend OTP...";
                break;

            default:
                break;
        }
        Invoke("CloseThisPanel", 2f);
    }

    void CloseThisPanel()
    {
        Msg_Text.text = "";
        //gameObject.SetActive(false);
    }

    void MobileVerficationUI()
    {
        _mobile_Lvl.gameObject.SetActive(true);
        _userID.gameObject.SetActive(true);
        _OTP_Send_Button.gameObject.SetActive(true);
    }
    void EnableOTPUI()
    {
        OnInit();
        _mobile_Lvl.gameObject.SetActive(true);
        _userID.gameObject.SetActive(true);
        _OTP_Lvl.gameObject.SetActive(true);
        _OTP_1.gameObject.SetActive(true);
        _OTP_2.gameObject.SetActive(true);
        _OTP_3.gameObject.SetActive(true);
        _OTP_4.gameObject.SetActive(true);
        _Timer_Lvl.gameObject.SetActive(true);
        _OTP_Resend_Button.gameObject.SetActive(true);
        _OTP_OK_Button.gameObject.SetActive(true);

    }
    void EnablePasswordUI()
    {
        OnInit();
        _mobile_Lvl.gameObject.SetActive(true);
        _userID.gameObject.SetActive(true);
        _password.gameObject.SetActive(true);
        _confrimPassword.gameObject.SetActive(true);
        _password_Lvl.gameObject.SetActive(true);
        _ReEnter_password_Lvl.gameObject.SetActive(true);
      
        _loginButton.gameObject.SetActive(true);
        //_Set_password_Lvl.gameObject.SetActive(true);
        if(_password.text == _confrimPassword.text)
        {
            _sinUpButton.gameObject.SetActive(true);
        }
    }
    void OTPVerification()
    {

    }
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
    private void OnInit()
    {
        _otpMsgTxt.text = "";
        _userID.gameObject.SetActive(false);
        _password.gameObject.SetActive(false);
        _confrimPassword.gameObject.SetActive(false);
        _sinUpButton.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(false);
        _OTP_Send_Button.gameObject.SetActive(false);
        _OTP_Resend_Button.gameObject.SetActive(false);
        _mobile_Lvl.gameObject.SetActive(false);
        _OTP_Lvl.gameObject.SetActive(false);
        _password_Lvl.gameObject.SetActive(false);
        _ReEnter_password_Lvl.gameObject.SetActive(false);
        _Set_password_Lvl.gameObject.SetActive(false);
        _OTP_1.gameObject.SetActive(false);
        _OTP_2.gameObject.SetActive(false);
        _OTP_3.gameObject.SetActive(false);
        _OTP_4.gameObject.SetActive(false);
        _OTP_OK_Button.gameObject.SetActive(false);
        _Timer_Lvl.gameObject.SetActive(false);
        _forget_Password_Button.gameObject.SetActive(false);
    }

    void OnCancle()
    {
        UiManager.instance._sinUpController.gameObject.SetActive(false);
        UiManager.instance._logInController.gameObject.SetActive(true);

    }

    void MobileIfno(bool pIsForgotPassword)
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        w_mobileIfno = new MobileIfno();
        w_mobileIfno.mob_number = _userID.text;
        w_mobileIfno.isForgotPassword = pIsForgotPassword;


        string MobileIfnoBodyStr = JsonUtility.ToJson(w_mobileIfno);
        Debug.Log("---------" + MobileIfnoBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_SinUp_Number_Verification, "", MobileIfnoBodyStr, MobileIfnoCallback);
    }

    void MobileIfnoCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            WValidateNumberResponse wValidateNumberResponse = JsonUtility.FromJson<WValidateNumberResponse>(response);
            if (wValidateNumberResponse.isMobileAlreadyRegistered)
            {
                //Mobile number already exist
                OnMessage(eMsgType.allready_Exits);
                Debug.Log("Mobile number already exist");
            }
            else
            {
                //New Mobile number
                //Can create new a/c
                Debug.Log("New Mobile number");
                SendOTP();
            }
        }
        else
        {
            Debug.Log("............");
            
        }
    }
    void SendOTP()
    {
        _TimerTxt.text = "10 : 00";
        _otpMsgTxt.text = "";
        _OTP_OK_Button.interactable = false;
        _OTP_1.text = "";
        _OTP_2.text = "";
        _OTP_3.text = "";
        _OTP_4.text = "";
        ResetTimer();
        EnableOTPUI();
        timeInSecond = maxOTPTime;
        InvokeRepeating("OTPTimer", 0, 1);
    }
    public void OnMobNumEnter()
    {
        if(_userID.text.Length>=1)
        {
            _OTP_Send_Button.interactable = true;
            _OTP_Send_Button.gameObject.SetActive(true);
            _loginButton.gameObject.SetActive(true);


        }
        else
        {
            // _OTP_Send_Button.interactable = false;
            _OTP_Send_Button.gameObject.SetActive(false);
            _loginButton.gameObject.SetActive(false);

        }
    }
    public void AllOTPEntered()
    {
        _otpMsgTxt.text = "";
        if (!string.IsNullOrEmpty (_OTP_1.text) && !string.IsNullOrEmpty(_OTP_2.text )
            && !string.IsNullOrEmpty(_OTP_3.text) && !string.IsNullOrEmpty(_OTP_4.text ))
        {
            _OTP_OK_Button.interactable = true;
        }
        else
        {
            _OTP_OK_Button.interactable = false;
        }
    }
    void OnOTPSubmitWeb()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        w_otpInfo = new OtpInfo();
        w_otpInfo.mob_number = _userID.text;
        w_otpInfo.otp = _OTP_1.text + _OTP_2.text + _OTP_3.text + _OTP_4.text;


        string OTPSubmitBodyStr = JsonUtility.ToJson(w_otpInfo);
        Debug.Log("---------" + OTPSubmitBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_SinUp_OTP_Verification, "", OTPSubmitBodyStr, OnOTPSubmitWebCallback);
    }

    void OnOTPSubmitWebCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            //mLoginResponse = JsonUtility.FromJson<WLoginResponse>(response);
            WOTPResponse wOTPResponse = JsonUtility.FromJson<WOTPResponse>(response);
            if (wOTPResponse.isOTPValid)
            {
                //OTP Correct
                EnablePasswordUI();
            }
            else
            {
                //False OTP
                Debug.Log("False OTP");
                _otpMsgTxt.text = wrongOTPMsg;
                _OTP_OK_Button.interactable = false;
                OnMessage(eMsgType.wrongOTp);
            }
        }
        else
        {
            Debug.Log("............");

        }
    }


    void OnCreatePasswordWeb()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        w_farm = new Farm();
        w_farm.mob_number = _userID.text;
        w_farm.password = _password.text;


        string CreatePasswordBodyStr = JsonUtility.ToJson(w_farm);
        Debug.Log("---------" + CreatePasswordBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_SinUp_CreatePassword, "", CreatePasswordBodyStr, OnCreatePasswordWebCallback);
    }

    void OnCreatePasswordWebCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            w_farm = JsonUtility.FromJson<Farm>(response);
            DairyApplicationData.Instance.FarmID = w_farm.Id;
            DairyApplicationData.Instance.UserID = w_farm.mob_number;
            DairyApplicationData.Instance.User_password = w_farm.password;
            Debug.Log("Sinup Responce  ::  " + response);
            UiManager.instance._accountController.gameObject.SetActive(true);
            UiManager.instance._sinUpController.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("............");

        }
    }
    void OnLogIN()
    {
        UiManager.instance._logInController.gameObject.SetActive(true);
        UiManager.instance._sinUpController.gameObject.SetActive(false);
    }

    void OTPTimer()
    {
        timeInSecond--;
        int s = timeInSecond % 60;
        int m = timeInSecond / 60;
        string s_str = s.ToString();
        if(s_str.ToString().Length<=1)
        {
            s_str = "0" + s_str;
        }
        _TimerTxt.text = "0"+m + " : " + s_str;
        if ((maxOTPTime- timeInSecond)>= maxOTPTime)
        {
            //_otpMsgTxt.text = otpTimeOverMsg;
            OnMessage(eMsgType.timerUp);
            ResetTimer();
            
        }
    }
    void ResetTimer()
    {
        CancelInvoke("OTPTimer");
        timeInSecond = 0;
      

    }
    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnLogIN_Btn":
                OnLogIN();
                break;
            case "On_OTP_Send_Btn":
                MobileIfno(false);
                break;
            case "On__re_Send_Btn":
                SendOTP();
                break;
            case "On_Cancel_Btn":
                OnCancle();
                break;
            case "OnSinUp_Btn":
                OnCreatePasswordWeb();
                break;
            case "On_OK_Btn":
                OnOTPSubmitWeb();
                break;
            case "OnForgetPassword":
                OnInit(); 
                MobileVerficationUI();
                MobileIfno(true);
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        _userID.text = "";
        _password.text = "";
        _confrimPassword.text = "";
        _OTP_1.text = "";
        _OTP_2.text = "";
        _OTP_3.text = "";
        _OTP_4.text = "";
    }

}



[System.Serializable]
public class MobileIfno
{
    public string mob_number;
    public bool isForgotPassword;
}

[System.Serializable]
public class OtpInfo
{
    public string mob_number;
    public string otp;
}

[System.Serializable]
public class Farm
{
    public string Id;
    public string mob_number;
    public string password;
}
[System.Serializable]
class WValidateNumberResponse
{
    public bool isMobileAlreadyRegistered;
}
[System.Serializable]
class WOTPResponse
{
    public bool isOTPValid;
}