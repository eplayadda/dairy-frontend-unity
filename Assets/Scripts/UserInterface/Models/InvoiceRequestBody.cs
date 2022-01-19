using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InvoiceRequestBody
{
    public string customer_id;
    public string farm_id;
    public int inv_for_month;
    public int inv_for_year;
}
[System.Serializable]
public class InvoiceRequestBodyForOld
{
    public string farmID;
    public int month;
    public int year;
    public string customerID;
}
