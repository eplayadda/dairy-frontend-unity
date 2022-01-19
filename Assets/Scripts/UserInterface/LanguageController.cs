using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(DairyApplicationData.Instance.FarmID))
        {
            UiManager.instance._logInController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void OkClick()
    {
        UiManager.instance._languageController.gameObject.SetActive(false);
        UiManager.instance._logInController.gameObject.SetActive(true);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnOK_Btn":
                OkClick();
                break;
            default:
                break;
        }
    }
}
