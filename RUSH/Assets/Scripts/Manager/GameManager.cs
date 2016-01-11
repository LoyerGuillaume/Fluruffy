using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager manager;
    private GameManager() { }


    #region Game States
    private GAME_STATE currentState;

    public enum GAME_STATE {
        menu,
        play,
        pause,
        gameover,
        victory
    };


    public bool IsMenu
    {
        get { return currentState == GAME_STATE.menu; }
    }

    public bool IsPlaying
    {
        get { return currentState == GAME_STATE.play; }
    }

    public bool IsPaused
    {
        get { return currentState == GAME_STATE.pause; }
    }

    public bool IsGameOver
    {
        get { return currentState == GAME_STATE.gameover; }
    }

    public bool IsVictory
    {
        get { return currentState == GAME_STATE.victory; }
    }
    #endregion

    #region Events
    public delegate void GameStepNotification(params object[] prms);

    public event GameStepNotification onGamePlay;
    public event GameStepNotification onGameResume;
    public event GameStepNotification onGamePause;
    public event GameStepNotification onGameMenu;
    public event GameStepNotification onGameOver;
    public event GameStepNotification onVictory;
    public event GameStepNotification onGameScoreHasChanged;
    #endregion

    //public static GameManager manager
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new GameManager();
    //            //GameObject go = new GameObject("Game Manager");
    //            //go.AddComponent<GameManager>();
    //            //_instance = go.GetComponent<GameManager>();
    //        }

    //        return _instance;
    //    }
    //}

    void Awake ()
    {
        manager = this;
    }
    
    IEnumerator Start ()
    {
        while (!(Metronome.manager.IsReady) || !(LevelManager.manager.IsReady))
            yield return null;

        // Play music test
        if (MusicLoopsManager.manager) MusicLoopsManager.manager.PlayMusic(MusicType.menuMusic);


        AddLevelManagerListener();
        AddMenuManagerListener();
        AddHudManagerListener();

        currentState = GAME_STATE.menu;
        if (onGameMenu != null) onGameMenu();
    }

    private void AddLevelManagerListener()
    {
        LevelManager.manager.onAllCubesArrived += GameWin;
        LevelManager.manager.onCubeCollidedWithDeathZone += GameOver;
        LevelManager.manager.onLevelReady += LevelIsReady;
    }

    private void AddMenuManagerListener()
    {
        MenuManager.manager.onLevelButtonClick += LevelButtonClick;
    }

    private void AddHudManagerListener()
    {
        HudManager.manager.onClickButtonEditionMode += SetModeEdition;
        HudManager.manager.onClickButtonPlayPhase += SetPhasePlay;
    }

    private void LevelButtonClick(string nameLevel)
    {
        MenuManager.manager.DestroyCurrentMenu();
        LaunchLevel(nameLevel);
    }

    public void LaunchLevel(string nameLevel)
    {
        LevelManager.manager.LoadLevel(nameLevel);
    }

    //private void PlayButtonClicked()
    //{
    //    print("GAME MANAGER : PlayButtonClicked");
    //    LevelManager.manager.LoadLevel("Level1");
    //}

    public void LevelIsReady()
    {
        currentState = GAME_STATE.play;

        
        CustomTimer.manager.ResetAndStop();
        HudManager.manager.StartHud();

        print("GAMEMANAGER - onGamePlay");
        if (onGamePlay != null)
            onGamePlay();

    }

    void Update () {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (IsPlaying)
            {
                CustomTimer.manager.StopTimer();
                currentState = GAME_STATE.pause;
                if (onGamePause != null)
                    onGamePause();
            }
            else if (IsPaused)
            {
                CustomTimer.manager.StartTimer();
                currentState = GAME_STATE.play;
                if (onGameResume != null)
                    onGameResume();
            }
        }
    }

    public void SetPhasePlay()
    {
        CustomTimer.manager.ResetAndStart();
    }

    public void SetModeEdition()
    {
        CustomTimer.manager.ResetAndStop();
        LevelManager.manager.RestartLevel();
    }
    
    public void GameOver()
    {
        print("Game MANAGER - GAME OVER");
        CustomTimer.manager.StopTimer();
        onGameOver();
    }

    public void GameWin()
    {
        print("GAME MANAGER - GAMEWIN");
        CustomTimer.manager.StopTimer();
        onVictory();
    }
}
