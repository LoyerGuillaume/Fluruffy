using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {


    [SerializeField]
    private int frequenceCreationCubes = 3;


    public int FrequenceCreationCubes
    {
        get
        {
            return frequenceCreationCubes <= 0 ? 1 : frequenceCreationCubes;
        }
    }


    [SerializeField]
    private int numberOfSpawnCube = 3;


    public int NumberOfSpawnCube
    {
        get
        {
            return numberOfSpawnCube <= 0 ? 1 : numberOfSpawnCube;
        }
    }

   
}
