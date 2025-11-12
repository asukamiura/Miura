using UnityEngine;

public class LockOnUIPresenter : MonoBehaviour
{
    [SerializeField] LockOnUIView lockOnUIView;

    void Start()
    {
        CameraManager.Instance.OnLockOn += lockOnUIView.ChangeLockonUI;
    }
}
