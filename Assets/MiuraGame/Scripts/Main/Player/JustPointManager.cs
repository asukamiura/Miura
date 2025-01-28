using UnityEngine;

public class JustPointManager : MonoBehaviour
{
    [SerializeField] int justPoints = 5;

    const int minJustPoints = 0;
    public int maxJustPoints { get; set; }
    public int JustPoints => justPoints;

    void Awake()
    {
        maxJustPoints = justPoints;
    }

    public void AddJustPoints(int points)
    {
        justPoints = Mathf.Clamp(justPoints + points, minJustPoints, maxJustPoints);
        Debug.Log(JustPoints);
    }

    public void UseJustPoints(int points)
    {
        justPoints = Mathf.Clamp(justPoints - points, minJustPoints, maxJustPoints);
        Debug.Log(JustPoints);
    }
}
