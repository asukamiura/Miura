using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移用簡易フェードクラス
/// </summary>
public class FadeManager : MonoBehaviour
{
    private static Canvas canvas;
    private static Image image;

    private static FadeManager instance;
    public static FadeManager Instance
    {
        get
        {
            if (instance == null) { Init(); }
            return instance;
        }
    }

    IEnumerator fadeCoroutine = null;
    AsyncOperation async;

    private FadeManager() { }

    private static void Init()
    {
        // Canvas作成
        GameObject canvasObject = new GameObject("CanvasFade");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        // Image作成
        image = new GameObject("ImageFade").AddComponent<Image>();
        image.transform.SetParent(canvas.transform, false);

        // 画面中央をアンカーとし、Imageのサイズをスクリーンサイズに合わせる
        image.rectTransform.anchoredPosition = Vector3.zero;
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        // 遷移先シーンでもオブジェクトを破棄しない
        DontDestroyOnLoad(canvas.gameObject);

        // シングルトンオブジェクトを保持
        canvasObject.AddComponent<FadeManager>();
        instance = canvasObject.GetComponent<FadeManager>();
    }

    // フェード付きシーン遷移を行う
    public void LoadScene(string sceneName, float interval = 0.5f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = null;

        fadeCoroutine = Fade(sceneName, interval);
        StartCoroutine(fadeCoroutine);
    }

    private IEnumerator Fade(string sceneName, float interval)
    {
        float time = 0f;
        canvas.enabled = true;

        // フェードアウト
        while (time <= interval)
        {
            float fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            image.color = new Color(0.0f, 0f, 0f, fadeAlpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        //
        async = SceneManager.LoadSceneAsync(sceneName);
        //
        async.allowSceneActivation = false;
      
        //  ロード完了後、0.5秒待ってからシーン遷移
        yield return new WaitForSecondsRealtime(0.5f);
        async.allowSceneActivation = true;

        // シーン非同期ロード
        //yield return SceneManager.LoadSceneAsync(sceneName);

        // フェードイン
        time = 0f;
        while (time <= interval)
        {
            float fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            image.color = new Color(0f, 0f, 0f, fadeAlpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        // 描画を更新しない
        canvas.enabled = false;
    }
}
