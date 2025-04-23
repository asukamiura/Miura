using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineVirtualCamera justDodgeCamera;

    CinemachinePOV pov;
    float horizontalSpeed = 2;
    float verticalSpeed = 1;
    float sensitivity = 50;
    public bool IsInput { get; set; } = true;

    const float RecenteringTime = 0.1f;

    InputReciver Input => InputReciver.Instance;

    public static CameraManager Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        if (IsInput)
        {
            // 視点を動かす処理
            pov.m_HorizontalAxis.Value += Input.Look.x * horizontalSpeed * sensitivity * Time.deltaTime;
            pov.m_VerticalAxis.Value -= Input.Look.y * verticalSpeed * sensitivity * Time.deltaTime;
        }
    }

    IEnumerator ChangeFOV(float targetFOV, float duration)
    {
        float startFOV = playerCamera.m_Lens.FieldOfView;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time / duration);
            yield return null;
        }

        playerCamera.m_Lens.FieldOfView = targetFOV;
    }

    IEnumerator ChangeDutch(float targetDutch, float duration)
    {
        float startDutch = playerCamera.m_Lens.Dutch;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            playerCamera.m_Lens.Dutch = Mathf.Lerp(startDutch, targetDutch, time / duration);
            yield return null;
        }

        playerCamera.m_Lens.Dutch = targetDutch;
    }

    /// <summary>
    /// 視野角・カメラ傾きをまとめて管理する演出メソッド
    /// </summary>
    /// <param name="targetFOV">目標視野角</param>
    /// <param name="targetDutch">目標傾き</param>
    /// <param name="duration">演出にかける時間</param>
    public void PlayCameraEffect(float? targetFOV = null, float? targetDutch = null, float duration = 0.5f)
    {
        StopAllCoroutines();

        if (targetFOV.HasValue)
        {
            StartCoroutine(ChangeFOV(targetFOV.Value, duration));
        }

        if (targetDutch.HasValue)
        {
            StartCoroutine(ChangeDutch(targetDutch.Value, duration));
        } 
    }

    /// <summary>
    /// カメラを揺らす処理
    /// </summary>
    /// <param name="force">揺らす力の大きさ</param>
    /// <param name="duration">減衰にかかる時間</param>
    public void ApplyImpulse(float force = 1, float duration = 0.2f)
    {
        //impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = duration /2; 
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = duration; 
        impulseSource.GenerateImpulse(force);
    }

    // カメラの視点を中央に戻す処理開始
    public void EnabledRecentering()
    {
        pov.m_VerticalRecentering.m_enabled = true;
        pov.m_HorizontalRecentering.m_enabled = true;

        pov.m_VerticalRecentering.m_RecenteringTime = RecenteringTime;
        pov.m_HorizontalRecentering.m_RecenteringTime = RecenteringTime;
    }

    // カメラの視点を中央に戻す処理を終了
    public void DisabledRecentering()
    {
        pov.m_VerticalRecentering.m_enabled = false;
        pov.m_HorizontalRecentering.m_enabled = false;
    }

    public IEnumerator SwitchCamera(float blendTime)
    {
        IsInput = false;

        //　ブレンドタイムを設定
        cinemachineBrain.m_DefaultBlend.m_Time = blendTime;

        // カメラの描画優先順位を入れ替え
        int priority = justDodgeCamera.Priority;
        justDodgeCamera.Priority = playerCamera.Priority;
        playerCamera.Priority = priority;

        yield return new WaitForSeconds(blendTime);

        IsInput = true;
    }
}
