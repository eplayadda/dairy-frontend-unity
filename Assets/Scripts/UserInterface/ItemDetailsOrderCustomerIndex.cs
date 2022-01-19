using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsOrderCustomerIndex : MonoBehaviour
{
    public Image _profileImage;
    public Text _name_Text;
    public Text _Id_Text;
    public Text _road_Text;
    public CustomerInfo customerInfos;
    public string _name;
    int customerIndex;
    void ScrollCellIndex(int idx)
    {
        customerIndex = idx;
        CustomerInfo customerInfo = null;
        customerInfo = UiManager.instance._allOrderCustomerController.activeCustomer[idx];
        
        customerInfos = customerInfo;
        customerInfos.Id = customerInfo.Id;
        _name = customerInfo.c_name;

        _name_Text.GetComponent<DairyText>().DairyString(_name);
        _Id_Text.text = customerInfo.rank.ToString();
        _road_Text.text = customerInfo.c_road_number;

    }

    public void OnSelectCustomerToShowOrderDetails()
    {
        UiManager.instance._allOrderCustomerController.OnCustomerSelected(customerIndex);
    }
}
