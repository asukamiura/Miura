using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleOperationUIView : MonoBehaviour
{
    [SerializeField] Image attackIcon;
    [SerializeField] Image dashIcon;
    [SerializeField] Image guardIcon;
    [SerializeField] Image ultimateIcon;
    [SerializeField] Image healIcon;
    [SerializeField] Image powerUpIcon;

    [SerializeField] Animator blockAnimator;
    [SerializeField] Animator dodgeAnimator;
    [SerializeField] Animator ultimateAnimator;

    Color defaultColor = Color.white;
    Color pushColor = Color.red;

    const float ChangeColorFrame = 20;    // UIの色変更継続時間

    IEnumerator ChangeColor(Image icon)
    {
        icon.color = pushColor;

        for (int i = 0; i < ChangeColorFrame; i++)
        {
            yield return null;
        }

        icon.color = defaultColor;
    }

    public void ReactDashIcon()
    {
        StartCoroutine(ChangeColor(dashIcon));
    }

    public void ReactGuardIcon()
    {
        StartCoroutine(ChangeColor(guardIcon));
    }

    public void ReactHealIcon()
    {
        StartCoroutine(ChangeColor(healIcon));
    }

    public void ReactPowerUpIcon()
    {
        StartCoroutine(ChangeColor(powerUpIcon));
    }

    public void ReactDodgeIcon()
    {
        dodgeAnimator.SetTrigger("IsPressed");
    }

    public void ReactBlockIcon()
    {
        blockAnimator.SetTrigger("IsPressed");        
    }

    public void ReactUltimateIcon()
    {
        ultimateAnimator.SetTrigger("IsPressed");
    }
}
