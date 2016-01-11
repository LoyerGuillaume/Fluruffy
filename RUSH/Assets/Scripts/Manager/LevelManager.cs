using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Collections.Generic;

public class LevelManager : BaseManager<LevelManager>
{
    private const String PATH_LEVEL = "Levels/";
    private const String PATH_LEVEL_ACTION = "LevelActions/";
    private const String PATH_VISUAL = "Visual/";

    private GameObject currentLevel;

    public TextAsset actionXml;

    private bool solve;

    private int nTotalCubesShouldArrived;
    private int nCubeBySpawn;

    private Dictionary<string, Dictionary<string, int>> nActionPerLevel;
    private Dictionary<string, List<GameObject>> listCurrentAction;

    //private Dictionary<string, List<GameObject>> listActions;

    //public Dictionary<string, List<GameObject>> ListActions
    //{
    //    get
    //    {
    //        return listActions;
    //    }
    //}


    //private List<LevelActionTeleport> teleports;

    #region Events
    public event Action onAllCubesArrived;
    public event Action onCubeCollidedWithDeathZone;
    public event Action onLevelReady;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        InitListActionsLevels();
        print("LEVEL MANAGER - XML : " + nActionPerLevel);
    }

    protected override IEnumerator CoroutineStart()
    {
        IsReady = true;
        yield return null;
    }

    public void LoadLevel(String nameLevel)
    {
        print("LEVELMANAGER : LoadLevel");
        LoadAndStartLevel(nameLevel);
        InitListCurrentAction(nameLevel);
        StartCoroutine(InitLevelWhenIsReadyCoroutine());
    }

    IEnumerator InitLevelWhenIsReadyCoroutine()
    {
        while (currentLevel == null)
        {
            yield return null;
        }

        InitLevel();
        if (onLevelReady != null) onLevelReady();
        //GameManager.manager.LevelIsReady();
    }

    private void LoadAndStartLevel(String nameLevel)
    {
        currentLevel = Instantiate(Resources.Load(PATH_LEVEL+ nameLevel)) as GameObject;
        currentLevel.transform.SetParent(transform);
    }

    private void InitLevel()
    {
        Level level = currentLevel.GetComponent<Level>();

        nTotalCubesShouldArrived = 0;
        BroadcastMessage("SetFrequenceCreationCube", level.FrequenceCreationCubes);

        nCubeBySpawn = level.NumberOfSpawnCube;
        BroadcastMessage("SetNumberOfSpawnCube", nCubeBySpawn);
    }

    private void InitListCurrentAction(String levelName)
    {
        listCurrentAction = new Dictionary<string, List<GameObject>>();
        foreach (string key in nActionPerLevel[levelName].Keys)
        {
            List<GameObject> listGameObjectAction = new List<GameObject>();
            String nameInstantiateAction = key;
            String actionRotation = "";
            if (nameInstantiateAction.Split("_"[0])[0] == "Arrow")
            {
                actionRotation = nameInstantiateAction.Split("_"[0])[1];
                nameInstantiateAction = "Arrow";
            }

            for (int i = 0; i < nActionPerLevel[levelName][key]; i++)
            {
                GameObject action = Instantiate(Resources.Load(PATH_LEVEL_ACTION + "LevelAction" + nameInstantiateAction)) as GameObject;
                listGameObjectAction.Add(action);
                if (actionRotation != "")
                {
                    action.transform.Rotate(new Vector3(0, int.Parse(actionRotation), 0));
                }
                action.SetActive(false);
                action.transform.SetParent(currentLevel.transform.FindChild("Actions").transform);
            }

            listCurrentAction.Add(key, listGameObjectAction);
        }
    }


    //TO DO
    private void DestroyListCurrentAction()
    {
        if (listCurrentAction.Count != 0)
        {

        }
    }
    

    /// <summary>
    /// Initialize by xml "ACTION_LIST.xml" a dictionary with all Action 
    /// </summary>
    private void InitListActionsLevels()
    {
        //print("TEST : " + "arrow_0".Split("_"[0])[0]);
        //print("TEST 2 : " + "arrow0".Split("_"[0])[0]);
        nActionPerLevel = new Dictionary<string, Dictionary<string, int>>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(actionXml.text);

        foreach (XmlNode nodeLevel in xmlDoc.GetElementsByTagName("LEVEL"))
        {
            Dictionary<string, int> listActions = new Dictionary<string, int>();
            if (nodeLevel.NodeType != XmlNodeType.Comment)
            {
                foreach (XmlNode nodeAction in nodeLevel.ChildNodes)
                {
                    /* Retourne tous les noeuds même ceux des autres levels
                    nodeLevel.OwnerDocument.GetElementsByTagName("ACTION")
                    */

                    //List<String> list = new List<String>();

                    //for (int i = 0; i < int.Parse(levelNode.Attributes["number"].Value); i++)
                    //{
                    //    //GameObject action = Instantiate(Resources.Load(PATH_LEVEL_ACTION + "LevelAction" + levelNode.Attributes["name"].Value)) as GameObject;
                    //    list.Add(levelNode.Attributes["name"].Value);
                    //}
                    listActions.Add(nodeAction.Attributes["name"].Value, int.Parse(nodeAction.Attributes["number"].Value));
                }
                nActionPerLevel.Add(nodeLevel.Attributes["name"].Value, listActions);
            }
        }
    }

    public void RestartLevel()
    {
        nTotalCubesShouldArrived = 0;
        BroadcastMessage("ResetLevel");
        //InitLevel();
    }

    protected override void Play(params object[] prms)
    {
        print("LEVELMANAGER : PLAY");
    }

    public void CubeCollidedWithDeathZone(Cube cube)
    {
        CreateExclamation(cube.transform.position + Vector3.up);
        if (onCubeCollidedWithDeathZone != null) onCubeCollidedWithDeathZone();
    }

    private void CreateExclamation(Vector3 position)
    {
        GameObject exclamation = Instantiate(Resources.Load(PATH_VISUAL + "VisualExclamation")) as GameObject;
        exclamation.transform.SetParent(currentLevel.transform);
        exclamation.GetComponent<VisualExclamation>().InitializePosition(position);
    }

    private void SpawnCube(Spawner spawner)
    {
        GameObject cube = Instantiate(spawner.Cube, spawner.transform.position, spawner.transform.rotation) as GameObject;
        cube.transform.SetParent(currentLevel.transform);
    }

    private void AddSpawner()
    {
        nTotalCubesShouldArrived += nCubeBySpawn;
    }

    //private void AddTeleport(LevelActionTeleport teleport)
    //{
    //    print("TELEPORT " + teleport);
    //    teleports.Add(teleport);
    //}

    private void CubeArrived(Cube cube)
    {
        nTotalCubesShouldArrived--;
        DestroyCube(cube);

        if (nTotalCubesShouldArrived == 0)
        {
            if (onAllCubesArrived != null) onAllCubesArrived();
        }
    }

    public void DestroyCube(Cube cube)
    {
        cube.OnDestroy();
        DestroyObject(cube.transform.gameObject);
    }

    public void DestroyExclamation(VisualExclamation exclamation)
    {
        print("DESTROY EXCLAMATION");
        exclamation.OnDestroy();
        DestroyObject(exclamation.transform.gameObject);
    }

    //private void CubeTeleport(Cube cube)
    //{
    //    foreach(LevelActionTeleport teleport in teleports)
    //    {
    //        if (teleport.color == cube.CurrentTeleportColor && !teleport.doTeleportCube)
    //        {
    //            cube.transform.position = teleport.transform.position + Vector3.up;
    //            cube.SetStatePop();
    //            teleport.doTeleportCube = false;
    //            return;
    //        }
    //    }
    //}

}
