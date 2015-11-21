using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private GameObject cube;

    public GameObject Cube
    {
        get
        {
            return cube;
        }
    }

    private int numberOfTic;
    private int frequenceCreationCubes;

    private int numberOfSpawn;
    private int totalSpawn;

    // Use this for initialization
    void Start () {
        numberOfTic = 0;

        SendMessageUpwards("AddSpawner");

        if (Metronome.manager)
        {
            Metronome.manager.onTic += StartTic;
        }
    }

    private void StartTic()
    {
        //print("SPAWNER : StartTic");
        if (numberOfTic % frequenceCreationCubes == 0 && totalSpawn > numberOfSpawn)
        {
            SendMessageUpwards("SpawnCube", this);
            numberOfSpawn++;
        }

        numberOfTic++;
        //MonoBehaviour script = myCube.GetComponent<Cube>();

    }

    private void SetFrequenceCreationCube(int frequence)
    {
        frequenceCreationCubes = frequence;
    }

    private void SetNumberOfSpawnCube(int number)
    {
        totalSpawn = number;
    }
}
