using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eLanguage
{
    none,
    hindi,
    english
}

public  class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public eLanguage currLanguage;
    public LogInController _logInController;
    public MainMenuController _mainMenuController;
    public OrderController _orderController;
    //public SubOrderController _subOrderController;
    public KhataController _khataController;
    public AllCustomercontroller _allCustomercontroller;
    public ProfileController _ProfileController;
    public MilkFeedController _milkFeedController;
    public LoadingController _loadingController;
    public AllOrderCustomerController _allOrderCustomerController;
    public InvoiceGeneratedMsgController _invoiceGeneratedMsgController;
    public PaidInvoiceController _paidInvoiceController;
    public MenuController _menuController;
    public MsgController _msgController;
    public ShareInvoiceController _shareInvoiceController;
    public AllCustomerDeleteController _allCustomerDeleteController;
    public SettingController _settingController;
    public ShopController _shopController;
    public AccountController _accountController;
    public DeleteCustomerController _deleteCustomerController;
    public ItemDetailsShop _ItemDetailsShop;
    public GraphController _graphController;
    public KhataMenuController _khataMenuController;
    public HelpController _helpController;
    public CharaController _charaController;
    public SinUpController _sinUpController;
    public ContactController _contactController;
    public LanguageController _languageController;
    public PaymentController _paymentController;

    public Transform paginationPanel;
    public Font hindi;
    public Font english;
    public Font currentFont;
    public List<Text> texts;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (instance == null)
            instance = this;
    }

    void OnEnable()
    {
        OnInt();
        LoadSettingPropreties();
        _languageController.gameObject.SetActive(true);
    }

    public void OnInt()
    {
        _logInController.gameObject.SetActive(false);
        _mainMenuController.gameObject.SetActive(false);
         _orderController.gameObject.SetActive(false);
        //_subOrderController.gameObject.SetActive(false);
        _khataController.gameObject.SetActive(false);
        _allCustomercontroller.gameObject.SetActive(false);
        _ProfileController.gameObject.SetActive(false);
        _milkFeedController.gameObject.SetActive(false);
        _allOrderCustomerController.gameObject.SetActive(false);
        _menuController.gameObject.SetActive(false);
        _invoiceGeneratedMsgController.gameObject.SetActive(false);
        _paidInvoiceController.gameObject.SetActive(false);
        _msgController.gameObject.SetActive(false);
        _shareInvoiceController.gameObject.SetActive(false);
        _allCustomerDeleteController.gameObject.SetActive(false);
        _settingController.gameObject.SetActive(false);
        _shopController.gameObject.SetActive(false);
        _accountController.gameObject.SetActive(false);
        _deleteCustomerController.gameObject.SetActive(false);
        _ItemDetailsShop.gameObject.SetActive(false);
        _graphController.gameObject.SetActive(false);
        _khataMenuController.gameObject.SetActive(false);
        _helpController.gameObject.SetActive(false);
        _charaController.gameObject.SetActive(false);
        _sinUpController.gameObject.SetActive(false);
        _contactController.gameObject.SetActive(false);
        _languageController.gameObject.SetActive(false);
        _paymentController.gameObject.SetActive(false);
       
    }
    void LoadSettingPropreties()
    {
        //Load data from json//
        string settingStr = DairyApplicationData.Instance.Setting;
        if (string.IsNullOrEmpty(settingStr))
        {
            DairyApplicationData.Instance.SettingModel = new SettingModel();
            DairyApplicationData.Instance.SettingModel.language = "English";
            DairyApplicationData.Instance.SettingModel.isLoop = false;
        }
        else
        {
            DairyApplicationData.Instance.SettingModel = JsonUtility.FromJson<SettingModel>(settingStr) ;
        }
        SetFont();
    }
    public void SetFont()
    {
        switch (DairyApplicationData.Instance.SettingModel.language)
        {
            case "English":
                currentFont = english;
                currLanguage = eLanguage.english;
                break;
            case "Hindi":
                currentFont = hindi;
                currLanguage = eLanguage.hindi;
                break;
            default:
                break;
        }
    }
}
