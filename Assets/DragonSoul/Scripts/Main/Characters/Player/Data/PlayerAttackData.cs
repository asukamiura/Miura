using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "ScriptableObjects/PlayerAttackData")]
public class PlayerAttackData : ScriptableObject
{
    [SerializeField, Header("アニメーションステート名")] string animationStateName;
    [SerializeField, Header("最終段か")] bool isFinalStep;
    [SerializeField, Header("遷移にかける時間")] float transitionDuration;
    [SerializeField, Header("アニメーションの再生を開始する時間"), Range(0, 1)] float timeOffset;
    [SerializeField, Header("先行入力の受付開始時間"), Range(0, 1)] float comboEnableTime;
    [SerializeField, Header("次の段に遷移する時間"), Range(0, 1)] float transitionTime;
    [SerializeField, Header("コンボ受付終了時間"), Range(0, 1)] float comboResetTime;
    [SerializeField, Header("攻撃倍率")] float attackMultiplier;
    [SerializeField, Header("攻撃範囲")] float attackRange;
    [SerializeField, Header("獲得必殺技ゲージ量")] float ultAmount;
    [SerializeField, Header("攻撃でひるませられるか")] bool canFlinch;
    [SerializeField, Header("ヒットストップ継続時間")] float hitStopDuration;
    [SerializeField, Header("カメラを揺らす力")] float cameraShakeForce;
    [SerializeField, Header("ヒット時の獲得スコア")] int score;

    public string AnimationStateName => animationStateName;
    public bool IsFinalStep => isFinalStep;
    public float TransitionDuration => transitionDuration;
    public float TimeOffset => timeOffset;
    public float ComboEnableTime => comboEnableTime;
    public float TransitionTime => transitionTime;
    public float ComboResetTime => comboResetTime;
    public float AttackMultiplier => attackMultiplier;
    public float AttackRange => attackRange;
    public float UltAmount => ultAmount;
    public bool CanFlinch => canFlinch;
    public float HitStopDuration => hitStopDuration;
    public float CameraShakeForce => cameraShakeForce;
    public int Score => score;
}
