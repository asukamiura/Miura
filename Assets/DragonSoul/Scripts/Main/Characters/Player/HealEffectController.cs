using Player;
using UnityEngine;

public class HealEffectController : MonoBehaviour
{
    [SerializeField] PlayerCore playerCore;
    [SerializeField] EffectController effectController;

    void Awake()
    {
        playerCore.OnHealed += () => effectController.ShowEffect("LifeEnchant");
    }  
}
