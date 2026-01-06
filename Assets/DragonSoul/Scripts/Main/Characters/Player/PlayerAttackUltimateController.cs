using UnityEngine;

namespace Player
{
    public class PlayerAttackUltimateController : PlayerAttackControllerBase
    {
        [SerializeField] PlayerAttackData attackData;

        // 現在の攻撃のデータを取得
        public PlayerAttackData GetAttackData()
        {
            return attackData;
        }

        // アニメーションイベント
        public void PerformUltimateAttackHit()
        {
            OnHitDetectionPerformed?.Invoke();
        }
    }
}
