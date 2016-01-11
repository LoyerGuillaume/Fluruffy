using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : BaseManager<HudManager>
{

    private const string PATH_UI = "UI/";

    private GameObject currentHud;
    private GameObject btnPlay;
    private GameObject btnEdit;


    #region Events
    public Action onClickButtonPlayPhase;
    public Action onClickButtonEditionMode;
    #endregion

    protected override IEnumerator CoroutineStart()
    {
        IsReady = true;
        yield return null;
    }

    public void StartHud()
    {
        currentHud = Instantiate(Resources.Load(PATH_UI + "HUD")) as GameObject;
        btnPlay = GameObject.Find("Play");
        btnEdit = GameObject.Find("EditButton");
        btnEdit.SetActive(false);

        btnPlay.GetComponent<Button>().onClick.AddListener(PlayClick);
    }


    public void PlayClick()
    {
        btnPlay.SetActive(false);
        btnPlay.GetComponent<Button>().onClick.RemoveListener(PlayClick);
        
        btnEdit.SetActive(true);
        btnEdit.GetComponent<Button>().onClick.AddListener(EditClick);

        if (onClickButtonPlayPhase != null) onClickButtonPlayPhase();
   }

    public void EditClick()
    {
        btnEdit.SetActive(false);
        btnEdit.GetComponent<Button>().onClick.RemoveListener(EditClick);
        
        btnPlay.SetActive(true);
        btnPlay.GetComponent<Button>().onClick.AddListener(PlayClick);
        
        if (onClickButtonEditionMode != null) onClickButtonEditionMode();
    }

}
