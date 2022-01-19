using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DairyLanguage : MonoBehaviour
{
    static List<char> english_char = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    static bool IsEnglish(char pChar)
    {
        pChar = char.ToUpper(pChar);
        return english_char.Contains(pChar);
    }
    public static string DairyString(string pData, out Font font)
    {
        font = UiManager.instance.english;
        if (string.IsNullOrEmpty(pData))
        {
            font = null;
            return "";
        }
        if (!IsEnglish(pData[0]))
        {
            font = font = UiManager.instance.hindi;
            pData = UnicodeToKrutidev.UnicodeToKrutiDev(pData);
        }


        return pData;
    }
}
