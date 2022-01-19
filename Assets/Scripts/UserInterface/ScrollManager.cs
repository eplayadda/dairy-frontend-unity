using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ScrollManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform pnl;
    public List<RectTransform> point;
    public List<GameObject> allPanel;
    public GameObject temp;
    float offset;
    public int currIndex;
    public int dataID;
    public bool isStartAnimate;
    public OrderController orderController;
    public int customerCount;
    public bool isNextSlid = true;
    public bool isPrevSlid;
    public bool isLoop;
    float slideOffset;
    void Start()
    {
        slideOffset = Screen.width / 6;
    }
    public void SetMaxScrollerItem(int pData)
    {
        customerCount = pData;
        if (customerCount == 1)
        {
            isNextSlid = false;
            isPrevSlid = false;
        }
        if (isLoop)
        {
            isNextSlid = true;
            isPrevSlid = true;
        }
        allPanel[0].GetComponent<SubOrderController>().data = pData - 1;
        allPanel[1].GetComponent<SubOrderController>().data = 0;
        allPanel[2].GetComponent<SubOrderController>().data = 1;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isStartAnimate)
            return;
        if (eventData.dragging)
        {
            Vector2 v2 = pnl.anchoredPosition;
            v2.x = eventData.position.x - offset;
            if ((isNextSlid && v2.x < 0) || (isPrevSlid && v2.x > 0))
                pnl.anchoredPosition = v2;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isStartAnimate)
            return;
        offset = eventData.position.x - pnl.anchoredPosition.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(pnl.anchoredPosition.x);
        if (Mathf.Abs(pnl.anchoredPosition.x) - slideOffset <= 0)
        {
            LeanTween.move(pnl, Vector2.zero, .2f);
            return;
        }
        if (pnl.anchoredPosition.x > 0)
        {

            if (isPrevSlid)
            {
                currIndex++;
                InitSwap();

            }
        }
        else
        {
            if (isNextSlid)
            {
                currIndex--;
                InitSwap();

            }
        }
    }

    void InitSwap()
    {
        isStartAnimate = true;
        LeanTween.move(pnl, point[currIndex].anchoredPosition, .2f).setOnComplete(() => OnSlideComplite());
    }

    void OnSlideComplite()
    {
        if (isPrevSlid || isNextSlid)
            StartCoroutine( "SwapPanel");
    }
  
    float DistanceBtnPints()
    {
        return Vector2.Distance(pnl.anchoredPosition, point[currIndex].anchoredPosition);
    }
    IEnumerator SwapPanel()
    {
        while (true)
        {
            yield return null;
            if (DistanceBtnPints() < 1f)
            {
                break;
            }
        }
        isStartAnimate = false;
        for (int i = 0; i <  allPanel.Count; i++)
        {
            allPanel[i].transform.parent = temp.transform;
        }
        pnl.anchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < allPanel.Count; i++)
        {
            allPanel[i].transform.parent = pnl.transform;
            allPanel[i].GetComponent<RectTransform>().anchoredPosition = allPanel[i].GetComponent<RectTransform>().anchoredPosition;
        }

        allPanel[currIndex].GetComponent<RectTransform>().anchoredPosition = point[2 - currIndex].anchoredPosition;
        SwapList();
        currIndex = 1;
    }

    void SwapList()
    {
        if (currIndex == 0)
        {
            GameObject go = allPanel[0];
            allPanel.Remove(go);
            allPanel.Add(go);
            dataID = allPanel[1].GetComponent<SubOrderController>().data + 1;
            dataID = dataID % customerCount;

            SetSliderDirection();
            orderController.SetData(allPanel[2].GetComponent<SubOrderController>(), dataID);
            int prevData = dataID - 2;
            if (prevData < 0)
                prevData = customerCount - 2;
            orderController.SetData(allPanel[0].GetComponent<SubOrderController>(), prevData);

        }
        else
        {
            for (int i = 0; i < allPanel.Count - 1; i++)
            {
                GameObject go = allPanel[0];
                allPanel.Remove(go);
                allPanel.Add(go);
            }

            dataID = allPanel[1].GetComponent<SubOrderController>().data - 1;
            if (dataID < 0)
                dataID = customerCount - 1;
            SetSliderDirection();
            orderController.SetData(allPanel[0].GetComponent<SubOrderController>(), dataID);
            int next = dataID + 2;
            next = next % customerCount;
            orderController.SetData(allPanel[2].GetComponent<SubOrderController>(), next);

        }
    }
    void SetSliderDirection()
    {
        if (isLoop)
            return;
        int currID = allPanel[1].GetComponent<SubOrderController>().data;

        if (currID < customerCount)
        {
            isNextSlid = true;
            isPrevSlid = true;
        }
        if (currID == customerCount - 1)
        {
            isNextSlid = false;
            isPrevSlid = true;
        }
        if (currID == 0)
        {
            isNextSlid = true;
            isPrevSlid = false;
        }
    }
    public void OnBtnClicked(int id)
    {
        int centerUI = allPanel[1].GetComponent<SubOrderController>().data;
        if (centerUI == id)
            return;
        if (centerUI > id)
        {
            orderController.SetData(allPanel[0].GetComponent<SubOrderController>(), id);
            currIndex++;
            InitSwap();
        }
        else
        {
            orderController.SetData(allPanel[2].GetComponent<SubOrderController>(), id);
            currIndex--;
            InitSwap();
        }
        SetSliderDirection();

    }
}