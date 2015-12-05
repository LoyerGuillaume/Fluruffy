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

        currentMenu = Instantiate(Resources.Load(PATH_UI + "MainMenu")) as GameObject;

        GameObject btnPlay = GameObject.Find("Play");
        btnPlay.GetComponent<Button>().onClick.AddListener(PlayClick);
        print("MENUMANAGER : Menu Started");



        //for (int i = 1; i <= 50; i++)
        //{

        //    button.name = PlayerPrefs.GetString("LeveltextNumber" + i.ToString());


        //    button.button.onClick.AddListener(delegate { test(button.name); });

        //if (onPlayButtonClicked != null)
        //{
        //    //onPlayButtonClicked();
        //}

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
        DestroyCurrentMenu();
        GameManager.manager.LaunchLevel(nameLevel);
    }

    private void DestroyCurrentMenu()
    {
        currentMenu.SetActive(false);
        Destroy(currentMenu);
    }
}
