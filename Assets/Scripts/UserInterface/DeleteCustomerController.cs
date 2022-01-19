using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ePlayAdda.Dairy;
using System.Linq;

public class DeleteCustomerController : MonoBehaviour
{
    public Text _id_text;
    public Text _text;
    public Text _placeholder_text;
    CustomerInfo customerInfos;
    AllCustomerDeleteController mAllCustomercontroller;
    CustomerInfo mProfileResponse;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButton();
        }
    }

    public void SetDeleteCustomer(CustomerInfo custmInfo)
    {
        customerInfos = custmInfo;
        string rank = custmInfo.rank.ToString();
        _id_text.text = "' " + rank + " '";
        _placeholder_text.text = "Enter " + "' " + rank + " '";
    }

    void DeleteButtonClick()
    {
        int ranks = System.Convert.ToInt32(_text.text);
        if (customerInfos.rank == ranks)
        {
            UpdateStatushToserver(true);
        }
       

    }
    void UpdateStatushToserver(bool status)
    {
        customerInfos.c_delete = status;
        string profileBodyStr = JsonUtility.ToJson(customerInfos);
        Debug.Log("Profile String ...  " + profileBodyStr);
        DairyWebRequest.Instance.BeingPostRequest(DairyConstant.URL_Profile, "", profileBodyStr, ProfileCallback);
        UiManager.instance._loadingController.gameObject.SetActive(true);

    }
    void ProfileCallback(bool success, string response)
    {
        UiManager.instance._loadingController.gameObject.SetActive(false);

        if (success)
        {
            mProfileResponse = JsonUtility.FromJson<CustomerInfo>(response);

            var custInfo = DairyApplicationData.Instance._farmCustomers.customerInfos.Where(s => s.Id == mProfileResponse.Id).FirstOrDefault();
            if(custInfo != null)
            {
                DairyApplicationData.Instance._farmCustomers.customerInfos.Remove(custInfo);
            }
            UiManager.instance._allCustomerDeleteController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("......Call back null......");
        }
    }

    void OnBackButton()
    {
        gameObject.SetActive(false);
        UiManager.instance._allCustomerDeleteController.gameObject.SetActive(true);
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnBack_Btn":
                OnBackButton();
                break;
            case "OnDelete_Btn":
                DeleteButtonClick();
                break;
            default:
                break;

        }
    }

    private void OnDisable()
    {
        _id_text.text = "";
        _placeholder_text.text = "";
    }
}
