using UnityEngine;
using System.Collections;
using System;

public abstract class BaseManager<T> : MonoBehaviour where T : Component
{
    //sorte de classe Observer de base ... qui observe le GameManager

    public static T manager;

    public bool dontDestroyGameObjectOnLoad = false;

    [HideInInspector]
    public bool IsReady { get; protected set; }

    protected Transform _transform;

    protected virtual void Awake()
    {
        if (manager != null)
            Destroy(manager.gameObject);

        manager = this as T;

        if (dontDestroyGameObjectOnLoad) DontDestroyOnLoad(gameObject);

        IsReady = false;
        _transform = transform;

    }

    protected virtual void OnDestroy()
    {
        if (GameManager.manager)
        {
            GameManager.manager.onGameMenu -= Menu;
            GameManager.manager.onGamePlay -= Play;
            GameManager.manager.onGameResume -= Resume;
            GameManager.manager.onGamePause -= Pause;
            GameManager.manager.onGameOver -= GameOver;
            GameManager.manager.onVictory -= Victory;
            GameManager.manager.onGameScoreHasChanged -= ScoreHasChanged;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (GameManager.manager)
        {
            GameManager.manager.onGameMenu += Menu;
            GameManager.manager.onGamePlay += Play;
            GameManager.manager.onGameResume += Resume;
            GameManager.manager.onGamePause += Pause;
            GameManager.manager.onGameOver += GameOver;
            GameManager.manager.onVictory += Victory;
            GameManager.manager.onGameScoreHasChanged += ScoreHasChanged;
        }
        else Debug.LogError("BaseManager " + name + " tells you: NO GameManager");

        StartCoroutine(CoroutineStart());
    }

    protected abstract IEnumerator CoroutineStart();

    // Update is called once per frame
    protected virtual void Play(params object[] prms)
    {
    }

    protected virtual void Resume(params object[] prms)
    {
    }

    protected virtual void Pause(params object[] prms)
    {
    }

    protected virtual void Menu(params object[] prms)
    {
    }

    protected virtual void GameOver(params object[] prms)
    {
    }

    protected virtual void Victory(params object[] prms)
    {
    }

    protected virtual void ScoreHasChanged(params object[] prms)
    {
    }
}
