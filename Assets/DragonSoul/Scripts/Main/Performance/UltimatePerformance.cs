using Cinemachine;
using System.Collections;
using UnityEngine;

public class UltimatePerformance : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] performanceCamera;

    const float FirstCameraDuration = 0.1f;
    const float SecondCameraDuration = 1f;
    const float ThirdCameraDuration = 0.75f;
    const float ReturnToPlayerDuration = 0.5f;
    const float ZoomOutWaitAfterThird = 0.5f;

    const float FirstToSecondBlend = 0.8f;
    const float ReturnBlendTime = 0.5f;   // メインカメラへ戻す時のブレンド時間
    const float InstantBlend = 0f;

    const float ZoomOutTargetFOV = 100f;
    const float ZoomOutDuration = 0.05f;
    const float ZoomInTargetFOV = 80f;
    const float ZoomInDuration = 0f;

    public void StartPerformance()
    {
        StartCoroutine(PerformanceFlow());
    }

    IEnumerator PerformanceFlow()
    {
        CameraManager.Instance.CanInputLook = false;
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Ultimate, 0f);
        SlowManager.Instance.ApplySlow(0f, SlowTargetType.Enemy);
        CameraManager.Instance.EnabledRecentering();

        // カメラ1へ
        CameraManager.Instance.SwitchCamera(performanceCamera[0], InstantBlend);
        yield return new WaitForSeconds(FirstCameraDuration);

        // カメラ2へ
        CameraManager.Instance.SwitchCamera(performanceCamera[1], FirstToSecondBlend);
        yield return new WaitForSeconds(SecondCameraDuration);

        // カメラ3へ
        CameraManager.Instance.SwitchCamera(performanceCamera[2], InstantBlend);

        yield return new WaitForSeconds(ThirdCameraDuration);
        CameraManager.Instance.PlayCameraEffect(performanceCamera[2], ZoomOutDuration, targetFOV: ZoomOutTargetFOV);

        yield return new WaitForSeconds(ZoomOutWaitAfterThird);

        // プレイヤーカメラへ戻す
        CameraManager.Instance.DisabledRecentering();
        CameraManager.Instance.ReturnToPlayerCamera(ReturnBlendTime);
        yield return new WaitForSeconds(ReturnToPlayerDuration);

        CameraManager.Instance.CanInputLook = true;
        // ズームインで元に戻す
        CameraManager.Instance.PlayCameraEffect(performanceCamera[2], ZoomInDuration, targetFOV: ZoomInTargetFOV);
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, 0f);
    }
}
