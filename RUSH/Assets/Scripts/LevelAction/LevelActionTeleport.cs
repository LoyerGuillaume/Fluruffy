using UnityEngine;
using System.Collections;

public class LevelActionTeleport : LevelAction {

    public bool doTeleportCube = false;

    public string color; 


    void Start()
    {
        SendMessageUpwards("AddTeleport", this);
    }
}
