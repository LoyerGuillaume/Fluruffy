using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

using System.Collections.Generic;
using System.Linq;

public class MenuManager : BaseManager<MenuManager>
{

    private const string PATH_UI = "UI/";

    private GameObject currentMenu;

    #region Events
    //public Action<string> onPlayerButtonClicked;
    //public Action onPlayButtonClicked;
    public Action onPauseButtonClicked;
    public Action onResumeButtonClicked;
    public Action<string> onLevelButtonClick;
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
        //LevelButtonClick("Level1");

        currentMenu = Instantiate(Resources.Load(PATH_UI + "MainMenu")) as GameObject;

        GameObject btnPlay = GameObject.Find("Play");
        btnPlay.GetComponent<Button>().onClick.AddListener(PlayClick);
        print("MENUMANAGER : Menu Started");

    }

    public void PlayClick()
    {
        print("PLAY CLICK");
        DestroyCurrentMenu();
        currentMenu = Instantiate(Resources.Load(PATH_UI + "LevelSelectorMenu")) as GameObject;
        InitLevelButton();
    }

    private void InitLevelButton()
    {
        GameObject[] levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        
        foreach (GameObject btn in levelButtons)
        {
            print("NAME BUTTON LEVEL : " + btn.name);
            string btnName = btn.name;
            btn.GetComponent<Button>().onClick.AddListener(delegate { LevelButtonClick(btnName); });
        }
    }

    private void LevelButtonClick(String nameLevel)
    {
        print("LEVEL CLICKED : " + nameLevel);
        if (onLevelButtonClick != null) onLevelButtonClick(nameLevel);
    }

    public void DestroyCurrentMenu()
    {
        currentMenu.SetActive(false);
        Destroy(currentMenu);
    }
}
