using Cinemachine;
using System.Collections;
using UnityEngine;

public class UltimatePerformance : MonoBehaviour
{
    [SerializeField] EffectPlayer effectPlayer;
    [SerializeField] CinemachineVirtualCamera[] performanceCamera;

    [Header("Timing (seconds)")]
    [SerializeField] float firstCameraDelay = 0.1f;
    [SerializeField] float secondCameraDuration = 1f;
    [SerializeField] float thirdCameraDuration = 0.8f;
    [SerializeField] float returnToPlayerDuration = 0.5f;
    [SerializeField] float zoomOutWaitAfterThird = 0.5f;  // ← 0.5f をここに

    [Header("Blend Times")]
    [SerializeField] float firstToSecondBlend = 0.8f;
    [SerializeField] float toPlayerBlend = 0.5f;
    [SerializeField] float instantBlend = 0f;  // ← 0 を意味づけ

    [Header("FOV Settings")]
    [SerializeField] float zoomOutTargetFOV = 100f;
    [SerializeField] float zoomOutDuration = 0f;
    [SerializeField] float zoomInTargetFOV = 80f;
    [SerializeField] float zoomInDuration = 0f;

    public void StartPerformance()
    {
        StartCoroutine(PerformanceFlow());
    }

    IEnumerator PerformanceFlow()
    {
        CameraManager.Instance.IsInput = false;
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Ultimate, 0f);
        SlowManager.Instance.ApplySlow(0f, SlowTargetType.Enemy);

        // カメラ1へ
        CameraManager.Instance.SwitchCamera(performanceCamera[0], instantBlend);
        CameraManager.Instance.EnabledRecentering();
        yield return new WaitForSeconds(firstCameraDelay);

        // カメラ2へ
        CameraManager.Instance.SwitchCamera(performanceCamera[1], firstToSecondBlend);
        yield return new WaitForSeconds(secondCameraDuration);

        // カメラ3へ
        CameraManager.Instance.SwitchCamera(performanceCamera[2], instantBlend);

        yield return new WaitForSeconds(thirdCameraDuration);
        CameraManager.Instance.PlayCameraEffect(performanceCamera[2], zoomOutDuration, targetFOV: zoomOutTargetFOV);

        yield return new WaitForSeconds(zoomOutWaitAfterThird);

        // プレイヤーカメラへ戻す
        CameraManager.Instance.DisabledRecentering();
        CameraManager.Instance.ReturnToPlayerCamera(toPlayerBlend);
        yield return new WaitForSeconds(returnToPlayerDuration);

        CameraManager.Instance.IsInput = true;
        // ズームインで元に戻す
        CameraManager.Instance.PlayCameraEffect(performanceCamera[2], zoomInDuration, targetFOV: zoomInTargetFOV);
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, 0f);
    }
}
