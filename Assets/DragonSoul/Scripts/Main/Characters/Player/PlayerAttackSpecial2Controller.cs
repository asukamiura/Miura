using UnityEngine;

namespace Player
{
    public class PlayerAttackSpecial2Controller : PlayerAttackControllerBase
    {
        [SerializeField] PlayerAttackData[] attackDatas;

        // 現在の攻撃のデータを取得
        public PlayerAttackData GetCurrentAttackData(int currentStep)
        {
            return attackDatas[currentStep - 1];
        }

        // 最大コンボ段数を取得
        public int GetMaxComboCount()
        {
            return attackDatas.Length;
        }

        // アニメーションイベント
        public void PerformSpecialAttack2Hit()
        {
            OnHitDetectionPerformed?.Invoke();
        }
    }
}
