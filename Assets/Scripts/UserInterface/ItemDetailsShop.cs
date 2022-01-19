using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsShop : MonoBehaviour
{
    public Text _item_Name;
    public Text _item_Price;
    public Text _item_MarketPrice;
    public Text _item_quantity;
    // Use this for initialization
    void Start () {
		
	}
	public void FillDataInUI(DairyShop item)
    {
        string pName = "";
        if (UiManager.instance.currLanguage == eLanguage.english)
        {
            pName = item.e_product_name;
        }
        else
        {
            pName = item.h_product_name;
        }
        _item_Name.gameObject.GetComponent<DairyText>().DairyString(pName);
        _item_Price.text = item.h_product_price;
        _item_MarketPrice.text = item.h_product_marketPrice;
        _item_quantity.text = item.h_product_quantity;
    }
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }

    void OnContactCustomer()
    {
        string _Mobile = "7870526237";
        Application.OpenURL("tel:" + _Mobile);
        Debug.Log("calling" + _Mobile);
    }

    void OnBackButton()
    {
        gameObject.SetActive(false);
        UiManager.instance._shopController.gameObject.SetActive(true);

    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            case "OnContact_Btn":
                OnContactCustomer();
                break;
            default:
                break;

        }
    }
}
