using UnityEngine;
using System.Collections;
using System;

public class MenuManager : BaseManager<MenuManager>
{

    #region Events
    //public Action<string> onPlayerButtonClicked;
    public Action onPlayButtonClicked;
    public Action onPauseButtonClicked;
    public Action onResumeButtonClicked;
    //public Action onNewGameButtonClicked;
    //public Action onMainMenuButtonClicked;
    #endregion

    protected override IEnumerator CoroutineStart()
    {
        IsReady = true;
        yield return null;
    }


    protected override void Menu(params object[] prms)
    {
        print("MENUMANAGER : Menu Started");

        if (onPlayButtonClicked != null)
        {
            onPlayButtonClicked();
        }

    }
}
