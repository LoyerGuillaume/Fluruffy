using UnityEngine;
using System.Collections;

public class DebugFPSTester : MonoBehaviour {

    public int numberOfLoop = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < numberOfLoop; i++)
        {
            float a = i / 80;
        }
    }
}
