using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hindiGlyph : MonoBehaviour {

	public Text hindiText;

    // Use this for initialization
    void Start()
    {
        if (UiManager.instance.currLanguage == eLanguage.hindi)
            hindiText.text = UnicodeToKrutidev.UnicodeToKrutiDev(hindiText.text);
    }

}
