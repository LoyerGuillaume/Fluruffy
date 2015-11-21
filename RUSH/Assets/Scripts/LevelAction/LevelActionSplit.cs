using UnityEngine;
using System.Collections;

public class LevelActionSplit : LevelAction {


    [SerializeField]
    private bool splitTic = false;

    public bool SplitTic
    {
        get
        {
            return splitTic;
        }
    }

    public void ChangeTic()
    {
        splitTic = !splitTic;
    }

}
