using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustPointManager : MonoBehaviour
{
    [SerializeField] private int justPoints = 5;

    private const int minJustPoints = 0;
    public int maxJustPoints { get; private set; }
    public int JustPoints => justPoints;

    private void Awake()
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
