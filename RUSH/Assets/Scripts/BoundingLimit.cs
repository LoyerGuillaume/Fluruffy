using UnityEngine;
using System.Collections;

public class BoundingLimit : MonoBehaviour {
    
    void OnTriggerEnter(Collider colliderItem)
    {
        print("BOUDING LIMIT - Collision with cube");
    }
}
