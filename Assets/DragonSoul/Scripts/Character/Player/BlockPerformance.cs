using System.Collections;
using UnityEngine;
using Cinemachine; 

public class BlockPerformance : MonoBehaviour
{
    [SerializeField] EffectPlayer effectPlayer;
    [SerializeField] CinemachineVirtualCamera performanceCamera;

    const float CameraBlendTime = 0.2f;
    const float FovChangeDuration = 0.4f;
    const float NormalFOV = 70f;
    const float JustGuardFOV = 55f;
    const float ReturnBlendTime = 1f;   // メインカメラへ戻す時のブレンド時間
    const float StopDuration = 0.4f;       // 動きを止める時間
    const float DefaultAnimationSpeed = 1f;
    const float StoppedAnimationSpeed = 0f;
    const float postEffectBlendTime = 1f;  // ポストエフェクトを戻すときのブレンド時間

    public void StartPerformance()
    {
        StartCoroutine(PerformanceFlow());
    }

    IEnumerator PerformanceFlow()
    {
        // ポストエフェクト切り替え
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.JustGuard, 0f);

        // カメラ演出
        CameraManager.Instance.SwitchCamera(performanceCamera, CameraBlendTime);
        CameraManager.Instance.PlayCameraEffect(performanceCamera, FovChangeDuration, targetFOV: JustGuardFOV);
        CameraManager.Instance.ApplyImpulse();
        CameraManager.Instance.EnabledRecentering();

        // エフェクト
        effectPlayer.ShowEffect("NovaLight");

        // 動きを停止
        SlowManager.Instance.ApplySlow(StoppedAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(StoppedAnimationSpeed, SlowTargetType.Enemy);

        yield return new WaitForSeconds(StopDuration);

        // 復帰処理
        CameraManager.Instance.DisabledRecentering();
        SlowManager.Instance.ApplySlow(DefaultAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(DefaultAnimationSpeed, SlowTargetType.Enemy);
        CameraManager.Instance.ReturnToPlayerCamera(ReturnBlendTime);
        CameraManager.Instance.PlayCameraEffect(performanceCamera, FovChangeDuration, targetFOV: NormalFOV);
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, postEffectBlendTime);
    }
}
