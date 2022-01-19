using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvoiceGeneratedMsgController : MonoBehaviour
{
    public enum eMsgType
    {
        none,
        success,
        badRequest,
        noOrder
    }
    eMsgType currMsgType;
    public SubOrderController _suborderControler;
    public Text Msg_Text;
	

    void Oncancel()
    {
        gameObject.SetActive(false);
    }

    public  void OnSuccessInvoice(eMsgType pMsgType)
    {
        switch (pMsgType)
        {
            case eMsgType.none:
                break;
            case eMsgType.success:
                Msg_Text.text = "Successfull Geneate...";
                break;
            case eMsgType.badRequest:
                Msg_Text.text = "Bad request...";
                break;
            case eMsgType.noOrder:
                Msg_Text.text = "you have no order in  current month...";
                break;
            default:
                break;
        }
        Invoke("CloseThisPanel", 0.8f);
    }

    void OnClickOK()
    {
        _suborderControler.OnAddInvoice();
    }

    void CloseThisPanel()
    {
        Msg_Text.text = "";
        gameObject.SetActive(false);
    }
    

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "Oncancel_Btn":
                Oncancel();
                break;
            case "OnClickOK_Btn":
                OnClickOK();
                break;

            default:
                break;

        }
    }
}
