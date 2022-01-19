using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public enum eMsgType
    {
        none,
        netSlow,
       
    }
    public Text Msg_Text;

    public void OnMessage(eMsgType pMsgType)
    {
        switch (pMsgType)
        {
            case eMsgType.none:
                break;
            case eMsgType.netSlow:
                Msg_Text.text = "Mobile Internet Speed is Slow...";
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
}
