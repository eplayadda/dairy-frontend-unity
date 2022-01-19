using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilkFeedController : MonoBehaviour
{

    public Toggle morningToggle;
    public Toggle eveningToggle;
    public InputField mMilkQuintityIF;
    public Button submitBtn;
    CalendarDateItem dateItem;
    double mTodayMilk;


    private void OnEnable()
    {
        submitBtn.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }

    }
    public void SetCaller(CalendarDateItem pDateItem)
    {
        string default_sift = pDateItem.calendarController.subOrderController.customerMonthlyDetails.customerInfo.c_sift;
        if (string.Equals(default_sift, "Morning"))
            morningToggle.isOn = true;
        else
            eveningToggle.isOn = true;

        dateItem = pDateItem;
    }
    void OnSubmit()
    {

        mTodayMilk = System.Convert.ToDouble(mMilkQuintityIF.text);

        if (morningToggle.isOn || !eveningToggle.isOn)
        {
            dateItem.OnDataEntered(mTodayMilk, "Morning");
        }
        else
        {
            dateItem.OnDataEntered(mTodayMilk, "Evening");

        }
        // dateItem.OnDataEntered(mTodayMilk, "Morning");
        gameObject.SetActive(false);
        mMilkQuintityIF.text = "";
    }

    void OnBackButton()
    {
        gameObject.SetActive(false);
        UiManager.instance._orderController.customerSliderPanel.SetActive(true);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnSubmit_Btn":
                OnSubmit();
                break;
            case "OnBack_Btn":
                OnBackButton();
                break;
           
            default:
                break;

        }
    }

    public void OnAmountChoose(float pTodayMilk)
    {

        mMilkQuintityIF.text = pTodayMilk.ToString();
        submitBtn.interactable = true;

    }
    public void OnMilkEnterd()
    {
        try
        {
            if (mMilkQuintityIF.text == "" || float.Parse(mMilkQuintityIF.text) < 0)
                submitBtn.interactable = false;
            else
                submitBtn.interactable = true;
        }
        catch(System.Exception e)
        {

        }
      

    }
    private void OnDisable()
    {
        mMilkQuintityIF.text = "";
    }
}
