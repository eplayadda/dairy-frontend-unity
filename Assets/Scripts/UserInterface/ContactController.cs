using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ContactController : MonoBehaviour
{
    public Text _text;
    //public GameObject retryBtn;
    public SG.InitOnStart initOnStart;
    string failString;
    List<Contact> _contacts;

    private void OnEnable()
    {
        LoadContacts();

    }

    public void LoadContacts()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        if (string.IsNullOrEmpty(failString))
        {
            _text.text += "Loading/";
            Contacts.LoadContactList(onDone, onLoadFailed);
            failString = null;
        }
    }
   
	//void Update ()
 //   {
 //       if (Input.GetKeyDown(KeyCode.Escape))
 //       {
 //           gameObject.SetActive(false);
 //           UiManager.instance._mainMenuController.gameObject.SetActive(true);

 //       }
 //   }

    void onLoadFailed(string reason)
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);
        failString = reason;
        _text.text += reason+"/";
        //retryBtn.SetActive(true);
    }

    void onDone()
    {
        //retryBtn.SetActive(false);
        UiManager.instance._loadingController.gameObject.SetActive(false);
        initOnStart.SetMaxScroolerItem(Contacts.ContactsList.Count);
        failString = null;
        _text.text += "Null" + "/"+ Contacts.ContactsList.Count;
    }

    void OnBackButton()
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            default:
                break;
        }
    }
}
