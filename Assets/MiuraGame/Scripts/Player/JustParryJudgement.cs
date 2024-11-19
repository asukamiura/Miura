using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParryTiming
{
    Fast,
    Just,
    Slow,
    None,
}

public class JustParryJudgement : MonoBehaviour
{
    [SerializeField] MutantCore mutantCore; 

    public ParryTiming timing = ParryTiming.None;

    private void Start()
    {
        mutantCore.OnParryTimingChanged += HandleParryTimingChanged;
    }

    void HandleParryTimingChanged(ParryTiming timigNum)
    {
        this.timing = timigNum;
        //Debug.Log(timing.ToString());
    }

    private void OnDestroy()
    {
        mutantCore.OnParryTimingChanged -= HandleParryTimingChanged;
    }

    public void ParryTimingChange(ParryTiming timing)
    {
        this.timing = timing;
    }
}
