using UnityEngine;
using System.Collections;

public class LevelActionTeleport : LevelAction {


    [SerializeField]
    private GameObject destinationTeleport;

    public GameObject DestinationTeleport
    {
        get
        {
            return destinationTeleport;
        }
        //set
        //{
        //    destinationTeleport = value;
        //}
    }

    public string color; 


    void Start()
    {
        SendMessageUpwards("AddTeleport", this);
    }
}
