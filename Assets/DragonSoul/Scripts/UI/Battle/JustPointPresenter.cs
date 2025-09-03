using UnityEngine;

public class JustPointPresenter : MonoBehaviour
{
    [SerializeField] JustPointManager manager;
    [SerializeField] JustPointView view;

    void Start()
    {
        view.SetJustPointsUI(manager.JustPoint);
    }

    void OnEnable()
    {
        manager.OnJustPointChanged += view.SetJustPointsUI;
    }

    void OnDisable()
    {
        manager.OnJustPointChanged -= view.SetJustPointsUI;
    }
}
