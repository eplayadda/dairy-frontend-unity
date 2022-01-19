using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ePlayAdda.Dairy
{
    public class DairyWebRequest : MonoBehaviour
    {
        static DairyWebRequest mInstance;
        public static DairyWebRequest Instance
        {
            get
            {
                if (mInstance == null)
                {
                    GameObject tobj = new GameObject();
                    tobj.name = "MyHttp";
                    DontDestroyOnLoad(tobj);
                    mInstance = tobj.AddComponent<DairyWebRequest>();
                }
                return mInstance;
            }
        }

        // Create the header with token
        public Dictionary<string, string> StandardHeader(string token)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Content-Type", "application/json");
            dict.Add("Cache-Control", "max-age=0, no-cache, no-store");
            dict.Add("Pragma", "no-cache");

            if(!string.IsNullOrEmpty(token))
                dict.Add("Authorization", "Bearer " + token);
            return dict;
        }

        // Called to start a post request
        public Coroutine BeingPostRequest(string url, string accesstoken, string payload = null, Action<bool, String> callBack = null)
        {
            Dictionary<string, string> header = StandardHeader(accesstoken);
            Coroutine corotutine = StartCoroutine(PostRequest(url, header, payload, callBack));
            return corotutine;
        }

        // Called to start a get request
        public Coroutine BeingGetRequest(string url, string accesstoken, Action<bool, String> callBack = null)
        {
            Dictionary<string, string> header = StandardHeader(accesstoken);
            Coroutine corotutine = StartCoroutine(GetRequest(url, header, callBack));
            return corotutine;
        }

        // Begin a Unity web post request
        IEnumerator PostRequest(string url, Dictionary<string, string> header, string payload = null, Action<bool, String> callBack = null)
        {
            var reqU = new UnityWebRequest();
            reqU.url = url;
            reqU.method = "POST";
            reqU.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            // Adding all header string key values pairs
            List<string> keys = new List<string>(header.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string keyStr = keys[i];
                string valueStr = header[keyStr];
                reqU.SetRequestHeader(keyStr, valueStr);
            }

            // Attack pay load
            if (payload != null)
            {
                byte[] payloadUTF8 = new System.Text.UTF8Encoding().GetBytes(payload);
                reqU.uploadHandler = (UploadHandler)new UploadHandlerRaw(payloadUTF8);
            }

            // Wait till request completes
            yield return reqU.SendWebRequest();

            // Handle request response
            if (reqU.isNetworkError || reqU.isHttpError)
            {
                if (callBack != null)
                    callBack(false, reqU.error);
            }
            else
            {
                if (callBack != null)
                    callBack(true, reqU.downloadHandler.text);
            }
        }

        // Begin a Unity web get request
        IEnumerator GetRequest(string url, Dictionary<string, string> header, Action<bool, String> callBack = null)
        {
            var reqU = new UnityWebRequest();
            reqU.url = url;
            reqU.method = "GET";

            // Adding all header string key values pairs
            List<string> keys = new List<string>(header.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string keyName = keys[i];
                reqU.SetRequestHeader(keyName, header[keyName]);
            }

            // Wait till request completes
            yield return reqU.SendWebRequest();

            // Handle request response
            if (reqU.isNetworkError || reqU.isHttpError)
            {
                if (callBack != null)
                    callBack(false, reqU.error);
            }
            else
            {
                if (callBack != null)
                    callBack(true, reqU.downloadHandler.text);
            }
        }
    }
}