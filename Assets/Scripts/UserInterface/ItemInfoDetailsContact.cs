using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDetailsContact : MonoBehaviour
{
    public Image _profile_texture;
    public Text _name_text;
    public Text _number_text;
    GUIStyle contactFaceStyle;
    public Contact c;

    public void Start()
    {
        contactFaceStyle = new GUIStyle();
        
    }
    void ScrollCellIndex(int idx)
    {
        SetDataInUI(idx);
    }

    public void OnContactClick()
    {
        UiManager.instance._contactController.gameObject.SetActive(false);
        UiManager.instance._ProfileController.gameObject.SetActive(true);
        UiManager.instance._ProfileController._name.text = c.Name;
        UiManager.instance._ProfileController._mobile.text = c.Phones[0].Number.ToString();
    }
    public void SetDataInUI(int idx)
    {
         c = Contacts.ContactsList[idx];
        _name_text.text = c.Name;
        _number_text.text = c.Phones[0].Number.ToString();
        if (c.PhotoTexture != null)
        {
            Texture2D tex = c.PhotoTexture;
            Sprite mySprite = Sprite.Create(tex, new Rect(0.5f, 0.5f, tex.width, tex.height), new Vector2(1f, 1f), 100.0f);
            _profile_texture.sprite = mySprite;
            _profile_texture.SetNativeSize();
        }
        else
        {
           //_profile_texture.sprite = contactFaceStyle.normal.background;
        }
    }

    public void OnButtonClicked(string pBtnName)
    {
        switch (pBtnName)
        {
            case "OnContact_Btn":
                OnContactClick();
                break;
            default:
                break;
        }
    }
}
