using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ShareScreensort : MonoBehaviour
{

    bool shot = false;
    Texture2D tex;
    private object imageFile;

    void Start()
    {
        int width = Screen.width;
        int height = Screen.height;
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    }


    void OnGUI()
    {
        if (shot)
        {
            GUI.DrawTexture(new Rect(10, 10, 60, 80), tex, ScaleMode.StretchToFill);
        }
    }

    public void OnShareImage()
    {
        Debug.Log("Media Share");
        StartCoroutine(SaveAndShare());
    }


    IEnumerator SaveAndShare()
    {
        yield return new WaitForEndOfFrame();

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        string path = Application.persistentDataPath + "/SaveImage.png";
        File.WriteAllBytes(path, bytes);
        Destroy(tex);

        if (NativeShare.TargetExists("com.whatsapp","+91 7870526237"))
            new NativeShare().AddFile(path).SetText("Invoice...").SetTarget("com.whatsapp").Share();




       
    }

    


}
