using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class DairyText : MonoBehaviour
{
    Font font;
    public Text text;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        DairyString(text.text);
    }

    public void DairyString(string pData)
    {
        text = gameObject.GetComponent<Text>();
        if (string.IsNullOrEmpty(pData))
            return;
        if(text!=null)
        {
            text.text = DairyLanguage.DairyString(pData, out font);
            text.font = font;
        }
      
    }
}
