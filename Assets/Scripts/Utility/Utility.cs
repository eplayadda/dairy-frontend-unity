using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{

    public static string GetMonthInString(int month,eLanguage currLanguage)
    {
        if (currLanguage == eLanguage.english)
            return mMonthEng(month);
        else if (currLanguage == eLanguage.hindi)
            return mMonthHin(month);
        else
            return "";

    }
    static string mMonthEng(int month)
    {
        string _monthStr = "";
        switch (month)
        {
            case 1:
                _monthStr = "January";
                break;
            case 2:
                _monthStr = "February";
                break;
            case 3:
                _monthStr = "March";
                break;
            case 4:
                _monthStr = "April";
                break;
            case 5:
                _monthStr = "May";
                break;
            case 6:
                _monthStr = "June";
                break;
            case 7:
                _monthStr = "July";
                break;
            case 8:
                _monthStr = "August";
                break;
            case 9:
                _monthStr = "September";
                break;
            case 10:
                _monthStr = "October";
                break;
            case 11:
                _monthStr = "November";
                break;
            case 12:
                _monthStr = "December";
                break;
            default:
                break;
        }

        return _monthStr;
    }
    static string mMonthHin(int month)
    {
        string _monthStr = "";
        switch (month)
        {
            case 1:
                _monthStr = "जनवरी";
                break;
            case 2:
                _monthStr = "फ़रवरी";
                break;
            case 3:
                _monthStr = "मार्च";
                break;
            case 4:
                _monthStr = "अप्रैल";
                break;
            case 5:
                _monthStr = "मई";
                break;
            case 6:
                _monthStr = "जून";
                break;
            case 7:
                _monthStr = "जुलाई";
                break;
            case 8:
                _monthStr = "अगस्त";
                break;
            case 9:
                _monthStr = "सितंबर";
                break;
            case 10:
                _monthStr = "अक्टूबर";
                break;
            case 11:
                _monthStr = "नवंबर";
                break;
            case 12:
                _monthStr = "दिसंबर";
                break;
            default:
                break;
        }
      //  _monthStr = UnicodeToKrutidev.UnicodeToKrutiDev(_monthStr);
        return _monthStr;
    }
    public static List<int> ConverInMMYY(int tMonth, int tyear)
    {
        List<int> ls = new List<int>();

        if (tMonth == 0)
        {
            tMonth = 12;
            tyear = tyear - 1;
        }
        if (tMonth == 13)
        {
            tMonth = 1;
            tyear = tyear + 1;
        }
        ls.Add(tMonth);
        ls.Add(tyear);
        return ls;
    }
}
