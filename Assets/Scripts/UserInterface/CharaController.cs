using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    public GameObject _add_Item_panel;

    private void OnEnable()
    {
        _add_Item_panel.SetActive(false);
    }

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
        UiManager.instance._mainMenuController.gameObject.SetActive(true);
    }

    void OnAddItemButton()
    {
        _add_Item_panel.SetActive(true);
    }

    void OnSubmit()
    {
        _add_Item_panel.SetActive(false);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            case "OnAdd_Item_Button":
                OnAddItemButton();
                break;
            case "OnSubmit":
                OnSubmit();
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        _add_Item_panel.SetActive(false);

    }
}
