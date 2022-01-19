using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;

public class ShopController : MonoBehaviour
{
    public Transform parentObj;
    public GameObject itemPrefab;
    public DairyShopList allProductList;
    DairyStr W_ShopString;

    public Text _item_Name;
    public Text _item_Price;
    public Text _item_MarketPrice;
    public Text _item_quantity;

    List<GameObject> allIteam = new List<GameObject>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }

    }
    private void OnEnable()
    {
        OnDairyShop();
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
        UiManager.instance._mainMenuController.gameObject.SetActive(true);

    }
     
    void OnItemClick()
    {
        gameObject.SetActive(false);
        UiManager.instance._ItemDetailsShop.gameObject.SetActive(true);
    }

    void OnDairyShop()
    {
        UiManager.instance._loadingController.gameObject.SetActive(true);
        W_ShopString = new DairyStr();
        string dairyShopBodyStr = JsonUtility.ToJson(W_ShopString);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Dairy_Shop, "", dairyShopBodyStr, DairyShopCallback);
    }

    void DairyShopCallback(bool success, string response)
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);
        if (success)
        {
            allProductList = JsonUtility.FromJson<DairyShopList>(response);
            CreateProduct(allProductList);
        }
        else
        {
            Debug.Log("............");

        }
    }
    void CreateProduct(DairyShopList pProductes )
    {
        foreach (var item in allIteam)
        {
            Destroy(item);
        }
        allIteam.Clear();
        foreach (var item in pProductes._allProduts)
        {
            GameObject go = Instantiate(itemPrefab);
            go.transform.SetParent(parentObj, false);
            go.SetActive(true);
            go.GetComponent<ItemDetails_ShopController>().ShowItem(item);
            allIteam.Add(go);
        }
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

[System.Serializable]
public class DairyShop
{
    public string Id;
    public string h_product_name;
    public string h_product_discription;
    public string h_product_about;
    public string h_product_price;
    public string h_product_discount;
    public string h_product_marketPrice;
    public string h_product_quantity;
    public string e_product_name;
    public string e_product_discription;
    public string e_product_about;
    public string e_product_price;
    public string e_product_discount;
    public string e_product_marketPrice;
    public string e_product_quantity;

}
[System.Serializable]
public class DairyShopList
{
    public List<DairyShop> _allProduts;
}

public class DairyStr
{
    string str;
}
    
