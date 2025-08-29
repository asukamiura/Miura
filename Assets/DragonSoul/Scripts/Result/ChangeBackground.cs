using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] GameObject targetImage;
    [SerializeField] Material material;

    void Start()
    {
        ShowImage();
    }

    void ShowImage()
    {
        if (!String.IsNullOrEmpty(ScreenShotManager.screenShotPath))
        {
            byte[] image = File.ReadAllBytes(ScreenShotManager.screenShotPath);

            Texture2D tex = new Texture2D(0, 0);
            tex.LoadImage(image);

            RawImage rawImage = targetImage.GetComponent<RawImage>();
            rawImage.texture = tex;

            material.SetTexture("_MainTex", tex);
        }
    }
}
