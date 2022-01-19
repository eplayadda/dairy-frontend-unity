using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetails_ShopController : MonoBehaviour
{
    public Text _item_Name;
    public Text _item_Price;
    public Text _item_MarketPrice;
    public Text _item_quantity;
    ShopController _shopController;
    DairyShop currItem;
    private void Start()
    {
        
    }

    public void OnItemSelected()
    {
        UiManager.instance._ItemDetailsShop.gameObject.SetActive(true);
        UiManager.instance._ItemDetailsShop.FillDataInUI(currItem);

    }
    public void ShowItem(DairyShop item)
    {
        currItem = item;
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

}
