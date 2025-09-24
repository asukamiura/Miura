using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] CinemachineVirtualCamera playerCamera;

    CinemachinePOV pov;
    float sensitivityX = 500;
    float sensitivityY = 50;
    CinemachineVirtualCamera currentCamera;     // 現在のカメラ
    const float RecenteringTime = 0.1f;

    InputReciver Input => InputReciver.Instance;

    public bool IsInput { get; set; } = true;
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
        SwitchCamera(playerCamera, 0);
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        if (IsInput)
        {
            // 視点を動かす処理
            pov.m_HorizontalAxis.Value += Input.Look.x  * sensitivityX * Time.deltaTime;
            pov.m_VerticalAxis.Value -= Input.Look.y * sensitivityY * Time.deltaTime;
        }
    }

    /// <summary>
    /// 視野角の変更
    /// </summary>
    /// <param name="camera">対象カメラ</param>
    /// <param name="targetFOV">目標視野角</param>
    /// <param name="duration">変更にかかる時間</param>
    /// <returns></returns>
    IEnumerator ChangeFOV(CinemachineVirtualCamera camera, float targetFOV, float duration)
    {
        if (duration <= 0)
        {
            camera.m_Lens.FieldOfView = targetFOV;
            yield break;
        }

        float startFOV = camera.m_Lens.FieldOfView;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            camera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time / duration);
            yield return null;
        }

        camera.m_Lens.FieldOfView = targetFOV;
    }

    /// <summary>
    /// 傾きの変更
    /// </summary>
    /// <param name="camera">対象カメラ</param>
    /// <param name="targetDutch">目標傾き</param>
    /// <param name="duration">変更にかかる時間</param>
    /// <returns></returns>
    IEnumerator ChangeDutch(CinemachineVirtualCamera camera, float targetDutch, float duration)
    {
        if (duration <= 0)
        {
            camera.m_Lens.Dutch = targetDutch;
            yield break;
        }

        float startDutch = camera.m_Lens.Dutch;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            camera.m_Lens.Dutch = Mathf.Lerp(startDutch, targetDutch, time / duration);
            yield return null;
        }

        camera.m_Lens.Dutch = targetDutch;
    }

    /// <summary>
    /// カメラ演出開始
    /// </summary>
    /// <param name="camera">対象カメラ</param>
    /// <param name="targetFOV">目標視野角</param>
    /// <param name="targetDutch">目標傾き</param>
    /// <param name="duration">変更にかかる時間</param>
    public void PlayCameraEffect(CinemachineVirtualCamera camera, float duration, float? targetFOV = null, float? targetDutch = null)
    {
        //StopAllCoroutines();

        if (targetFOV.HasValue)
        {
            StopCoroutine(nameof(ChangeFOV));
            StartCoroutine(ChangeFOV(camera, targetFOV.Value, duration));
        }

        if (targetDutch.HasValue)
        {
            StopCoroutine(nameof(ChangeDutch));
            StartCoroutine(ChangeDutch(camera, targetDutch.Value, duration));
        }
    }

    /// <summary>
    /// カメラを揺らす処理
    /// </summary>
    /// <param name="force">揺らす力の大きさ</param>
    /// <param name="duration">減衰にかかる時間</param>
    public void ApplyImpulse(float force = 1, float duration = 0.2f)
    {
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = duration;
        impulseSource.GenerateImpulse(force);
    }

    public void ApplyImpulse(CinemachineImpulseSource cinemachineImpulseSorce, float force = 1, float duration = 0.2f)
    {
        cinemachineImpulseSorce.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = duration;
        cinemachineImpulseSorce.GenerateImpulse(force);
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

    public void SwitchCamera(CinemachineVirtualCamera camera, float blendTime)
    {
        cinemachineBrain.m_DefaultBlend.m_Time = blendTime;

        if (currentCamera != null)
        {
            currentCamera.Priority = 0;
        }

        currentCamera = camera;

        if (currentCamera != null)
        {
            currentCamera.Priority = 1;
        }
    }

    public void ReturnToPlayerCamera(float blendTime)
    {
        SwitchCamera(playerCamera, blendTime);
    }
}
