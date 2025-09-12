using Cinemachine;
using SoundSystem;
using System.Collections;
using UnityEngine;

public class DodgePerformance : MonoBehaviour
{
    [SerializeField] EffectPlayer effectPlayer;
    [SerializeField] CinemachineVirtualCamera performanceCamera;
    [SerializeField] float slowDuration = 1f;

    const float DefaultAnimationSpeed = 1;
    const float PerformanceAnimationSpeed = 0.3f;
    const float CameraBlendTime = 0.5f;

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
        EffectManager.Instance.PlayEffect("NovaLight", transform.position, Quaternion.Euler(-90, 0, 0));
        effectPlayer.ShowEffect("SpikyExplosion");

        // スローにする
        SlowManager.Instance.ApplySlow(PerformanceAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(PerformanceAnimationSpeed, SlowTargetType.Enemy);

        // ポストエフェクトを変更
        PostEffectManager.Instance.ChangePostEffect(ProfileNum.JustDodge, 0);

        yield return new WaitForSeconds(slowDuration);

        SlowManager.Instance.ApplySlow(DefaultAnimationSpeed, SlowTargetType.Player);
        SlowManager.Instance.ApplySlow(DefaultAnimationSpeed, SlowTargetType.Enemy);

        PostEffectManager.Instance.ChangePostEffect(ProfileNum.Normal, 0);

        CameraManager.Instance.DisabledRecentering();
        CameraManager.Instance.ReturnToPlayerCamera(1);
    }
}
