using UnityEngine;

namespace Player
{
    public class PlayerAttackNormalController : PlayerAttackControllerBase
    {
        [SerializeField] PlayerAttackData[] attackDatas;

        // 現在の攻撃のデータを取得
        public PlayerAttackData GetCurrentAttackData(int currentStep)
        {
            return attackDatas[currentStep - 1];
        }

        public int GetMaxComboCount()
        {
            return attackDatas.Length;
        }

        // アニメーションイベント
        public void PerformNormalAttackHit()
        {
            OnHitDetectionPerformed?.Invoke();
        }
    }
}
