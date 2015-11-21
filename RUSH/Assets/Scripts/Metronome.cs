using UnityEngine;
using System.Collections;
using System;

public class Metronome : BaseManager<Metronome> {

    private float durationTic = 1f;
    private float ratioTic;
    private float elapsedTime = 0;

    public float RatioTic
    {
        get
        {
            return elapsedTime / durationTic;
        }
    }

    #region Events
    public Action onTic;
    #endregion


    // Use this for initialization
    //void Start()
    //{
    //    if (LevelManager.manager)
    //    {
    //        LevelManager.manager.onStartTic += Menu;
    //    }
    //    else Debug.LogError("Metronome " + name + " tells you: NO LevelManager");
        
    //}

    protected override IEnumerator CoroutineStart()
    {
        IsReady = true;
        yield return null;
    }

    
    protected override void Play(params object[] prms)
    {
        print("METRONOME : PLAY");
        StartCoroutine(CoroutineTic());
    }

    private IEnumerator CoroutineTic()
    {
        while (elapsedTime < durationTic)
        {
            elapsedTime += CustomTimer.manager.DeltaTime;
            yield return null;
        }

        //print("METRONOME : TIC");
        elapsedTime = 0;
        onTic();
        StartCoroutine(CoroutineTic());
    }
}
