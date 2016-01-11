using UnityEngine;
using System.Collections;

public class LevelActionSplit : LevelAction {


    [SerializeField]
    private bool rotationLeft = false;

    public bool RotationLeft
    {
        get
        {
            return rotationLeft;
        }
    }

    public void ChangeRotation()
    {
        rotationLeft = !rotationLeft;
    }

    public void ResetLevel()
    {
        ResetRotation();
    }
   
    private void ResetRotation ()
    {
        rotationLeft = false;
    }

}
