using System.Collections;
using UnityEngine;
using Cinemachine; 

public class BlockPerformance : MonoBehaviour
{
    [SerializeField] EffectPlayer effectPlayer;
    [SerializeField] CinemachineVirtualCamera performanceCamera;

    [SerializeField] float cameraBlendTime = 0.3f;
    [SerializeField] float fovChangeDuration = 0.1f;
    [SerializeField] float normalFOV = 50f;
    [SerializeField] float justGuardFOV = 45f;
    [SerializeField] float returnBlendTime = 1f;   // メインカメラへ戻す時のブレンド時間

    [SerializeField] float stopDuration = 0.4f;       // 動きを止める時間
    [SerializeField] float defaultAnimationSpeed = 1f;
    [SerializeField] float stoppedAnimationSpeed = 0f;

    [SerializeField] float postEffectBlendTime = 1f;  // ポストエフェクトを戻すときのブレンド時間

    public void StartPerformance()
    {
        StartCoroutine(PerformanceFlow());
    }

    IEnumerator PerformanceFlow()
    {
        // ポストエフェクト切り替え
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.JustGuard, 0f);

        // カメラ演出
        CameraManager.Instance.SwitchCamera(performanceCamera, cameraBlendTime);
        CameraManager.Instance.PlayCameraEffect(performanceCamera, fovChangeDuration, targetFOV: justGuardFOV);
        CameraManager.Instance.ApplyImpulse();
        CameraManager.Instance.EnabledRecentering();

        // エフェクト
        effectPlayer.ShowEffect("NovaLight");

        // 動きを停止
        SlowManager.Instance.ApplySlow(stoppedAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(stoppedAnimationSpeed, SlowTargetType.Enemy);

        yield return new WaitForSeconds(stopDuration);

        // 復帰処理
        CameraManager.Instance.DisabledRecentering();
        SlowManager.Instance.ApplySlow(defaultAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(defaultAnimationSpeed, SlowTargetType.Enemy);
        CameraManager.Instance.ReturnToPlayerCamera(returnBlendTime);
        CameraManager.Instance.PlayCameraEffect(performanceCamera, fovChangeDuration, targetFOV: normalFOV);
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, postEffectBlendTime);
    }
}
