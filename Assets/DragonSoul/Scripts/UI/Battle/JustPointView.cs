using UnityEngine;
using UnityEngine.UI;

public class JustPointView : MonoBehaviour
{
    [SerializeField] Image[] justPointUI;

    // ジャストポイントUIの更新処理
    public void SetJustPointsUI(int currentJustPoint)
    {
        for (int i = 0; i < justPointUI.Length; i++)
        {
            if (i < currentJustPoint)
            {
                justPointUI[i].enabled = true;
            }
            else
            {
                justPointUI[i].enabled = false;
            }
        }
    }
}
