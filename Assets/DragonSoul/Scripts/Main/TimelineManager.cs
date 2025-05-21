using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] PlayableDirector introDirector;
    [SerializeField] PlayableDirector clearDirector;

    public static TimelineManager Instance { get; private set; }

    public PlayableDirector IntroDirector => introDirector;
    public PlayableDirector ClearDirector => clearDirector;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }
}
