using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class DairyInputField : MonoBehaviour {
    InputField _InputField;
    Font font;
    Text displayTxt;
    void Start () {
       
    }

    public void OnInputValueChanged( )
    {
        _InputField = gameObject.GetComponent<InputField>();
        displayTxt = transform.Find("Display").GetComponent<Text>();
        displayTxt.text = DairyLanguage.DairyString(_InputField.text, out font);
        displayTxt.font = font;
    }


}
