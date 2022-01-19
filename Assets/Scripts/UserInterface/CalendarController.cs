using ePlayAdda.Dairy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _yearNumText;
    public Text _monthNumText;
    public SubOrderController subOrderController;
    public GameObject _item;
    public List<GameObject> _dateItems = new List<GameObject>();
    public GameObject prevBtnGO;
    public GameObject nextBtnGO;
    const int _totalDateNum = 42;
    OrderList oldOrNewOrderList;
    public DateTime _dateTime;
    int currMonth = 0;
    int currYear = 0;
  
    public void InitData(List<OrderInfo> pCurrMonthOrders, SubOrderController pSubOrderController)
    {
      
        subOrderController = pSubOrderController;
        if(pCurrMonthOrders.Count !=0)
        {
            currMonth = pCurrMonthOrders[0].o_month;
            currYear = pCurrMonthOrders[0].o_year;
        }
        else
        {
            currMonth = DairyApplicationData.todayMonth;
            currYear = DairyApplicationData.todayYear;
        }
        CreateDates(pCurrMonthOrders);
        SetNextnPrevBtn();
    }
    void CreateDates(List<OrderInfo> pCurrMonthOrders)
    {
        for (int i = 1; i < _dateItems.Count; i++)
        {
            Destroy(_dateItems[i]);
        }
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 140 + startPos.x, startPos.y - (i / 7) * 140, startPos.z);
           
           
            _dateItems.Add(item);
        }

      //  _dateTime = DateTime.Now;
        _dateTime = new DateTime(currYear, currMonth, 1);
        CreateCalendar(pCurrMonthOrders);

        _calendarPanel.SetActive(true);

    }

    void CreateCalendar(List<OrderInfo> pCurrMonthOrders = null)
    {
      //  ChangeFont();
        if (pCurrMonthOrders == null)
            return;
       
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);
        List<int> pDate = new List<int>();

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            pDate.Clear();
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    
                    _dateItems[i].SetActive(true);
                    label.text = (date + 1).ToString();
                    pDate.Add(date + 1);
                    pDate.Add(_dateTime.Month);
                    pDate.Add(_dateTime.Year);
                    
                    _dateItems[i].GetComponent<CalendarDateItem>().Init(this, GetCurrOrderInfo(pCurrMonthOrders, pDate[0]), pDate);

                    date++;
                }
            }
        }
        string mMonth = Utility.GetMonthInString(_dateTime.Month, UiManager.instance.currLanguage);
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.GetComponent<DairyText>().DairyString(mMonth);
    }

    OrderInfo GetCurrOrderInfo(List<OrderInfo> pCurrMonthOrders,int pDate)
    {
        OrderInfo orderInfo = null;
        foreach (var item in pCurrMonthOrders)
        {
            if (item.o_date == pDate)
            {
                orderInfo = item;
                break;
            }
        }   
        return orderInfo;
    }
    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }


    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        GetMyOrderListByMonth(4, 2020);

    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        GetMyOrderListByMonth(4, 2020);

    }

    public void MonthPrev()
    {
        
        GetMyOrderListByMonth(-1, 0);

    }

    public void MonthNext()
    {
        GetMyOrderListByMonth(1, 0);
    }

    void GetMyOrderListByMonth(int month,int year)
    {
        WOrderByMonthRequestBody mOrderByMonthRequestBody = new WOrderByMonthRequestBody();
        mOrderByMonthRequestBody.farmID = DairyApplicationData.Instance.FarmID;
        mOrderByMonthRequestBody.customerID = subOrderController.customerMonthlyDetails.customerInfo.Id;
        List<int> MMYY = Utility.ConverInMMYY(_dateTime.Month + month, _dateTime.Year + year);
        mOrderByMonthRequestBody.month = MMYY[0];
        mOrderByMonthRequestBody.year = MMYY[1];

        string orderBodyStr = JsonUtility.ToJson(mOrderByMonthRequestBody);
        Debug.Log("order string .... " + orderBodyStr);
        string url = "";
        if (month > 0)
        {
            isNext = true;
            url = DairyConstant.URL_OrderListByNextMonth;
        }
        else
        {
            isNext = false;
            url = DairyConstant.URL_OrderListByPrevMonth;
        }
        if (mOrderByMonthRequestBody.month == DairyApplicationData.todayMonth && mOrderByMonthRequestBody.year == DairyApplicationData.todayYear)
            subOrderController.SetCurrentOrderDetails();
        else
            DairyWebRequest.Instance.BeingPostRequest(url, "", orderBodyStr, OrderCallback);
    }
    bool isNext;
    void OrderCallback(bool success, string response)
    {
        if (success && response != "null")
        {
            OrderList orderList = JsonUtility.FromJson<OrderList>(response);
            if (orderList != null)
            {
                _dateTime = new DateTime( orderList.order_Infos[0].o_year, orderList.order_Infos[0].o_month,1);
                CreateCalendar(orderList.order_Infos);
                GetInvoice(orderList);
                Debug.Log("New chart");
                SetNextnPrevBtn();
            }
            else
            {
                prevBtnGO.SetActive(false);
                Debug.Log("Data not present");
            }
        }
        else
        {
            Debug.Log("Bad equest");
            UiManager.instance._msgController.gameObject.SetActive(true);
            UiManager.instance._msgController.OnMessage(MsgController.eMsgType.noOrder);
            prevBtnGO.SetActive(false);
        }
    }
    void SetNextnPrevBtn()
    {
        if(!DairyApplicationData.isLastMonthInvoiceGenerated)
        {
            prevBtnGO.SetActive(false);
            nextBtnGO.SetActive(false);
        }
        else
        {
            if (_dateTime.Month == DairyApplicationData.todayMonth && _dateTime.Year == DairyApplicationData.todayYear)
            {
                nextBtnGO.SetActive(false);
                prevBtnGO.SetActive(true);
            }
            else
            {
                nextBtnGO.SetActive(true);
                prevBtnGO.SetActive(true);
            }
        }
      
   
    }
    void GetInvoice(OrderList orderList)
    {
        oldOrNewOrderList = orderList;
        InvoiceRequestBodyForOld mInvoiceRequestBodyForOld = new InvoiceRequestBodyForOld();
        mInvoiceRequestBodyForOld.farmID = DairyApplicationData.Instance.FarmID;
        mInvoiceRequestBodyForOld.customerID = subOrderController.customerMonthlyDetails.customerInfo.Id;
        mInvoiceRequestBodyForOld.month = orderList.order_Infos[0].o_month;
        mInvoiceRequestBodyForOld.year = orderList.order_Infos[0].o_year;

        string invoiceBodyStr = JsonUtility.ToJson(mInvoiceRequestBodyForOld);
        Debug.Log("order string .... " + invoiceBodyStr);
        string url = "";
        if (isNext)
            url = DairyConstant.URL_InvoiceByNextMonth;
        else
            url = DairyConstant.URL_InvoiceByPrevMonth;

        DairyWebRequest.Instance.BeingPostRequest(url, "", invoiceBodyStr, InvoiceCallBack);
    }
    void InvoiceCallBack(bool success, string response)
    {
        if (success )
        {
            if (response != "null")
            {
                CustomeList invoiceObj = JsonUtility.FromJson<CustomeList>(response);
                subOrderController.SetInvoiceData(oldOrNewOrderList,invoiceObj);

                Debug.Log("New chart");
            }
            else
            {
                subOrderController.SetInvoiceData(oldOrNewOrderList,null);
                Debug.Log("Data not present");
            }
        }
        else
        {
            Debug.Log("Bad equest");
            UiManager.instance._msgController.gameObject.SetActive(true);
            UiManager.instance._msgController.OnMessage(MsgController.eMsgType.noOrder);
        }
    }
    public void OrderPlaced(OrderInfo pOrderInfo)
    {
        subOrderController.OrderPlaced(pOrderInfo);

    }
}
