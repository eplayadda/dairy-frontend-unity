using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using ePlayAdda.Dairy;
using System;

public class CalendarDateItem : MonoBehaviour
{
    public CalendarController calendarController;
    public Text todayQuintity;
    public Image _sunImage;
    public Image _moonImage;
    OrderInfo currOrderDetails;
    WAddOrderRequestBody wAddOrderRequestBody;
    List<int> currDate= new List<int>();
    List<CustomerMonthlyDetails> customersMonthlyDetails;

    public void Init(CalendarController pCalendarController, OrderInfo pOrderInfo, List<int> pdate)
    {
        currDate.Clear();
        customersMonthlyDetails = DairyApplicationData.Instance.customersMonthlyDetails;
        currOrderDetails = pOrderInfo;
        calendarController = pCalendarController;
        currDate.AddRange(pdate);
        if (currOrderDetails != null)
        {
            SetOrderSiftIcon(currOrderDetails.o_sift);
            float pQuantity = currOrderDetails.o_quantity;
            if (pQuantity == 0)
                todayQuintity.text = "Ab";
            else
                todayQuintity.text = pQuantity.ToString();

        }
        else
        {
            todayQuintity.text = "";
        }
        IsTodayDate();
    }

    void IsTodayDate()
    {
        try
        {
            string pTodayDate = DairyApplicationData.todayDate+""+ DairyApplicationData.todayMonth+""+ DairyApplicationData.todayYear;
            string pDateInUI = transform.Find("Date_Text").GetComponent<Text>().text + ""+ calendarController._dateTime.Month+""+ calendarController._dateTime.Year;
            if (string.Equals(pTodayDate, pDateInUI))
            {
                gameObject.GetComponent<Image>().color = Color.gray;
            }
            else
                gameObject.GetComponent<Image>().color = Color.white;
        }
        catch (Exception e)
        { }
    }

    public void OnDateItemClick()
    {
        //if(pCalendarController.)  
        Debug.Log(calendarController._dateTime.Month + " / " + calendarController._dateTime.Year+"::"+ DairyApplicationData.todayMonth+"/"+ DairyApplicationData.todayYear);
        int mDif = DairyApplicationData.todayMonth - calendarController._dateTime.Month;
        int yDif = DairyApplicationData.todayYear - calendarController._dateTime.Year;
        //var 
        if (((mDif == 0 && yDif == 0) || !DairyApplicationData.isLastMonthInvoiceGenerated) && !calendarController.subOrderController.isCheckedOut)
        {
            UiManager.instance._milkFeedController.gameObject.SetActive(true);
            UiManager.instance._milkFeedController.SetCaller(this);
        }
     
    }

    public void OnDataEntered(double pData,string pSift)
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        wAddOrderRequestBody = new WAddOrderRequestBody();
        if (currOrderDetails != null && currOrderDetails.Id != null)
            wAddOrderRequestBody.Id = currOrderDetails.Id;
        else
            wAddOrderRequestBody.Id = null;

        wAddOrderRequestBody.farmID = DairyApplicationData.Instance.FarmID;
        wAddOrderRequestBody.o_customer_id = calendarController.subOrderController.customerMonthlyDetails.customerInfo.Id;
        wAddOrderRequestBody.o_date = currDate[0];
        wAddOrderRequestBody.o_month = currDate[1];
        wAddOrderRequestBody.o_year = currDate[2];
        wAddOrderRequestBody.o_quantity = pData;
        wAddOrderRequestBody.o_sift = pSift;
        string orderBodyStr = JsonUtility.ToJson(wAddOrderRequestBody);
        Debug.Log("order string .... " + orderBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_ADD_Order, "", orderBodyStr, OrderCallback);
    }
    void SetOrderSiftIcon(string pSift)
    {
        if (pSift == "Morning")
        {
            _sunImage.gameObject.SetActive(true);
            _moonImage.gameObject.SetActive(false);
        }
        else
        {
            _moonImage.gameObject.SetActive(true);
            _sunImage.gameObject.SetActive(false);
        }
    }
    void OrderCallback(bool success, string response)
    {
        if (success)
        {
            UiManager.instance._loadingController.gameObject.SetActive(false);
            currOrderDetails = JsonUtility.FromJson<OrderInfo>(response);
            float pQuantity = currOrderDetails.o_quantity;
            if (pQuantity == 0)
                todayQuintity.text = "Ab";
            else
                todayQuintity.text = pQuantity.ToString();
            SetOrderSiftIcon(currOrderDetails.o_sift);
            bool isNewOrder = true;
            foreach (var item in DairyApplicationData.Instance.orderList.order_Infos)
            {
                if (item.Id == currOrderDetails.Id)
                {
                    item.o_quantity = currOrderDetails.o_quantity;
                    isNewOrder = false;
                    break;
                }
            }
            if (isNewOrder)
            {
                foreach (var item in customersMonthlyDetails)
                {
                    if (item.customerInfo.Id == currOrderDetails.o_customer_id)
                    {
                        if (item.orderInfos != null)
                        {

                            item.orderInfos.Add(currOrderDetails);
                        }


                    }

                }
                DairyApplicationData.Instance.orderList.order_Infos.Add(currOrderDetails);
                calendarController.OrderPlaced(currOrderDetails);

            }
        }
        else
        {
            Debug.Log("............");
        }
    }
}


[System.Serializable]
public class WAddOrderRequestBody
{
    public string Id;
    public string o_sift;
    public string farmID;
    public int o_date;
    public int o_month;
    public int o_year;
    public string o_customer_id;
    public double o_quantity;
}


