using Cinemachine;
using SoundSystem;
using System.Collections;
using UnityEngine;

public class DodgePerformance : MonoBehaviour
{
    [SerializeField] EffectController effectController;
    [SerializeField] CinemachineVirtualCamera performanceCamera;
    [SerializeField] Transform playerTransform;

    const float SlowDuration = 0.95f;
    const float DefaultAnimationSpeed = 1;
    const float PerformanceAnimationSpeed = 0.3f;
    const float CameraBlendTime = 0.5f;
    const float ReturnBlendTime = 1f;   // メインカメラへ戻す時のブレンド時間

    public void StartPerformance()
    {
        StartCoroutine(PerformanceFlow());
    }

    IEnumerator PerformanceFlow()
    {
        // SEの再生
        SoundManager.Instance.PlaySe("Dodge");

        // カメラの演出
        CameraManager.Instance.ApplyImpulse();
        CameraManager.Instance.SwitchCamera(performanceCamera, CameraBlendTime);
        CameraManager.Instance.EnabledRecentering();

        // エフェクトの再生
        EffectManager.Instance.PlayEffect("NovaLight", playerTransform.position);
        effectController.ShowEffect("SpikyExplosion");

        // スローにする
        SlowManager.Instance.ApplySlow(PerformanceAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(PerformanceAnimationSpeed, SlowTargetType.Enemy);

        // ポストエフェクトを変更
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.JustDodge, 0);

        yield return new WaitForSeconds(SlowDuration);

        SlowManager.Instance.ApplySlow(DefaultAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(DefaultAnimationSpeed, SlowTargetType.Enemy);

        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, 0);

        CameraManager.Instance.DisabledRecentering();
        CameraManager.Instance.ReturnToPlayerCamera(ReturnBlendTime);
    }
}
