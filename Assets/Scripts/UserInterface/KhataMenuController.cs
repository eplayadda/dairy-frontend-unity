using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KhataMenuController : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }

    void OnBackButton()
    {
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
