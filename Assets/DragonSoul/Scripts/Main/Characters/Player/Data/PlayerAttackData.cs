using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackData", menuName = "ScriptableObjects/PlayerAttackData")]
public class PlayerAttackData : ScriptableObject
{
    [SerializeField, Header("攻撃の名前")] string attackName;
    [SerializeField, Header("攻撃倍率")] float attackMultiplier;
    [SerializeField, Header("攻撃範囲")] float attackRange;
    [SerializeField, Header("獲得必殺技ゲージ量")] float ultAmount;
    [SerializeField, Header("攻撃でひるませられるか")] bool canFlinch;
    [SerializeField, Header("ヒットストップ継続時間")] float hitStopDuration;
    [SerializeField, Header("カメラを揺らす力")] float cameraShakeForce;
    [SerializeField, Header("ヒット時の獲得スコア")] int score;

    public float AttackMultiplier => attackMultiplier;
    public float AttackRange => attackRange;
    public float UltAmount => ultAmount;
    public bool CanFlinch => canFlinch;
    public float HitStopDuration => hitStopDuration;
    public float CameraShakeForce => cameraShakeForce;
    public int Score => score;
}
