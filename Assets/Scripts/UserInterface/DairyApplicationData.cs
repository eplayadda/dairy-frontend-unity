using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DairyApplicationData : MonoBehaviour
{
    static DairyApplicationData mInstance;
    public static string KEY_FarmID = "FarmID";
    public static string KEY_DairyProfileID  = "DairyProfileID";
    public static string KEY_Setting = "Setting_obj";
    public static string Key_CustomerID = "CustomerID";
    public static string Key_UserID = "Login_Mobile_Number";
    public static string Key_User_Passord = "Login_Password";
    public static bool allowRefresh = true;

    public FarmCustomers _farmCustomers;
    public OrderList orderList;
    public CustomerInvoice customerInvoice;
    public DairyProfile dairyProfile;
    public List<CustomerMonthlyDetails> customersMonthlyDetails = new List<CustomerMonthlyDetails>();
    public static int todayDate;
    public  static int todayMonth;
    public static int todayYear;
    public static bool isLastMonthInvoiceGenerated;
    public List<string> checkOutDone = new List<string>();
    public SettingModel SettingModel;
    string _dairyProfileID;
    string _FarmID;
    string _setting;
    string _userID;
    string _user_password;

    public static DairyApplicationData Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tobj = new GameObject();
                tobj.name = "DairyApplicationData";
                DontDestroyOnLoad(tobj);
                mInstance = tobj.AddComponent<DairyApplicationData>();
                todayDate = DateTime.Now.Day;
                //todayMonth = DateTime.Now.Month;
                todayMonth = 10;
                todayYear = DateTime.Now.Year;
                isLastMonthInvoiceGenerated = true;
                mInstance.Initialized();
            }
            return mInstance;
        }
    }
    void UpendKeyName()
    {
        KEY_DairyProfileID = mInstance.FarmID+ KEY_DairyProfileID;
        KEY_Setting = mInstance.FarmID+ KEY_Setting;
        Key_CustomerID = mInstance.FarmID+ Key_CustomerID;
        Key_UserID = mInstance.FarmID+ Key_UserID;
        Key_User_Passord = mInstance.FarmID+ Key_User_Passord;
    }
    public void ForTesting()
    {
        string flag = "InitMonth"+ mInstance.FarmID;
        string flagMonth = "todaymonth" + mInstance.FarmID;
        Debug.Log(flag+"="+PlayerPrefs.GetInt(flag)+"????");
        if (PlayerPrefs.GetInt(flag) == 0)
        {
            PlayerPrefs.SetInt(flag, 1);
            todayMonth = DateTime.Now.Month;
            //todayMonth = 4;
            Debug.Log("Current Months !!! " +DateTime.Now.Month);
            PlayerPrefs.SetInt(flagMonth, todayMonth);
            Debug.Log(PlayerPrefs.GetInt(flag) + "????"+todayMonth);

        }
        else
            todayMonth = PlayerPrefs.GetInt(flagMonth);
    }
    void Initialized()
    {
        _dairyProfileID = PlayerPrefs.GetString(KEY_DairyProfileID, "");
        _FarmID = PlayerPrefs.GetString(KEY_FarmID, "");
        _userID = PlayerPrefs.GetString(Key_UserID, "");
        _user_password = PlayerPrefs.GetString(Key_User_Passord, "");
        _setting = PlayerPrefs.GetString(KEY_Setting + Instance.FarmID, "");
    }

    public string DairyProfileID
    {
        get
        {
            return _dairyProfileID;
        }
        set
        {
            _dairyProfileID = value;
            PlayerPrefs.SetString(KEY_DairyProfileID, _dairyProfileID);
        }
    }

    public string FarmID
    {
        get
        {
            return _FarmID;
        }
        set
        {
            _FarmID = value;
            PlayerPrefs.SetString(KEY_FarmID, _FarmID);
        }
    }

    public string UserID
    {
        get
        {
            return _userID;
        }
        set
        {
            _userID = value;
            PlayerPrefs.SetString(Key_UserID, _userID);
        }
    }

    public string User_password
    {
        get
        {
            return _user_password;
        }
        set
        {
            _user_password = value;
            PlayerPrefs.SetString(Key_User_Passord, _user_password);
        }
    }
    public string Setting
    {
        get
        {
            return _setting;
        }
        set
        {
            _setting = value;
            PlayerPrefs.SetString(KEY_Setting+Instance.FarmID, _setting);
        }
    }

    
}
