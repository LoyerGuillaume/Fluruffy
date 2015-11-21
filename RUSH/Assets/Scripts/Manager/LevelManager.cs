using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LevelManager : BaseManager<LevelManager>
{
    private const String PATH_LEVEL = "Levels/";

    private GameObject currentLevel;
    
    private int numberTotalOfCubeShouldArrived;
    private int numberOfCubeArrived;
    private int numberOfCubeBySpawn;
    
    //private List<LevelActionTeleport> teleports;

    #region Events
    //public delegate void GameStepNotification(params object[] prms);

    //public event GameStepNotification onStartTic;
    #endregion


    protected override IEnumerator CoroutineStart()
    {
        IsReady = true;
        yield return null;
    }

    public void LoadLevel(String nameLevel)
    {
        print("LEVELMANAGER : LoadLevel");
        LoadAndStartLevel(nameLevel);
        StartCoroutine(InitLevelWhenIsReadyCoroutine());
    }

    IEnumerator InitLevelWhenIsReadyCoroutine()
    {
        while (currentLevel == null)
        {
            yield return null;
        }

        initLevel();
        GameManager.manager.LevelIsReady();
    }

    private void LoadAndStartLevel(String nameLevel)
    {
        currentLevel = Instantiate(Resources.Load(PATH_LEVEL+ nameLevel)) as GameObject;
        currentLevel.transform.SetParent(transform);
    }

    private void initLevel()
    {
        numberTotalOfCubeShouldArrived = 0;
        numberOfCubeArrived = 0;
        BroadcastMessage("SetFrequenceCreationCube", currentLevel.GetComponent<Level>().FrequenceCreationCubes);

        numberOfCubeBySpawn = currentLevel.GetComponent<Level>().NumberOfSpawnCube;
        BroadcastMessage("SetNumberOfSpawnCube", numberOfCubeBySpawn);

        //teleports = new List<LevelActionTeleport>();
    }

    protected override void Play(params object[] prms)
    {
        print("LEVELMANAGER : PLAY");
    }

    public void CubeCollidedWithDeathZone()
    {
        print("LEVELMANAGER - GAME OVER");
        GameManager.manager.GameOver();
    }

    private void SpawnCube(Spawner spawner)
    {
        GameObject cube = Instantiate(spawner.Cube, spawner.transform.position, spawner.transform.rotation) as GameObject;
        cube.transform.SetParent(currentLevel.transform);
    }

    private void AddSpawner()
    {
        numberTotalOfCubeShouldArrived += numberOfCubeBySpawn;
    }

    //private void AddTeleport(LevelActionTeleport teleport)
    //{
    //    print("TELEPORT " + teleport);
    //    teleports.Add(teleport);
    //}

    private void CubeArrived(Cube cube)
    {
        numberOfCubeArrived++;
        cube.OnDestroy();
        DestroyObject(cube.transform.gameObject);

        if (numberOfCubeArrived == numberTotalOfCubeShouldArrived)
        {
            GameManager.manager.GameWin();
        }
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
