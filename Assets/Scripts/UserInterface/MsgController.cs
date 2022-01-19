using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgController : MonoBehaviour
{
    public enum eMsgType
    {
        none,
        noOrder,
        delete,
        badRequest
    }
    public Text Msg_Text;

    public void OnMessage(eMsgType pMsgType)
    {
        switch (pMsgType)
        {
            case eMsgType.none:
                break;
            case eMsgType.noOrder:
                Msg_Text.text = "you have no order...";
                break;
            case eMsgType.delete:
                Msg_Text.text = " Can not delete existing order...";
                break;
            case eMsgType.badRequest:
                Msg_Text.text = "Bad request...";
                break;
            
            default:
                break;
        }
        Invoke("CloseThisPanel", 1.2f);
    }

    void CloseThisPanel()
    {
        Msg_Text.text = "";
        gameObject.SetActive(false);
    }
}
