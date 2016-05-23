using UnityEngine;
using System.Collections;

public class DeadBody : MonoBehaviour {

    // Use this for initialization
    public void Start()
    {
        Destroy(gameObject, 15.0f);
    }

}
	


