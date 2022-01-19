using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsDeleteCustomer : MonoBehaviour {

    public Text _name_Text;
    public Text _Id_Text;
    public Button _delete_Button;
    public CustomerInfo customerInfos;
    AllCustomerDeleteController mAllCustomercontroller;


    int customerIndex;
        
    void ScrollCellIndex(int idx)
    {
        mAllCustomercontroller = UiManager.instance._allCustomerDeleteController;
        customerIndex = idx;
        customerInfos = UiManager.instance._allCustomerDeleteController.allCustomers[idx];
        _name_Text.GetComponent<DairyText>().DairyString(customerInfos.c_name);
        _Id_Text.text = customerInfos.rank.ToString();

    }

    public void DeleteCustomer()
    {
        UiManager.instance._deleteCustomerController.gameObject.SetActive(true);
        mAllCustomercontroller.gameObject.SetActive(false);
        UiManager.instance._deleteCustomerController.SetDeleteCustomer(customerInfos);
    }


}
