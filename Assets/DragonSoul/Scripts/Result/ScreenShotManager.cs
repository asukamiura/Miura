using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    string timeStamp;

    public static string screenShotPath;
    public static ScreenShotManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }        
    }

    string GetScreenShotPath()
    {
        string path = "";
        path = timeStamp + ".png";
        return path;
    }

    // スクリーンショット作成
    IEnumerator CreateScreenShot()
    {
        // 現在日時の取得
        DateTime dateTime = DateTime.Now;
        timeStamp = dateTime.ToString("yyy-MM-dd-HH-mm-ss-fff");

        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cam.targetTexture = renderTexture;

        Texture2D texture = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);

        texture.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        texture.Apply();

        byte[] pngData = texture.EncodeToPNG();
        screenShotPath = GetScreenShotPath();

        // ファイルとして保存
        File.WriteAllBytes(screenShotPath, pngData);

        cam.targetTexture = null;
    }

    // スクリーンショットを削除
    public void DeleteScreenShot()
    {
        if (!string.IsNullOrEmpty(screenShotPath) && File.Exists(screenShotPath))
        {
            File.Delete(screenShotPath);
        }
    }

    public void TakeScreenShot()
    {
        StartCoroutine(CreateScreenShot());
    }
}
