using UnityEngine;
using System.Collections;

public class DeadBody : MonoBehaviour {


    public void Start()
    {
        Destroy(gameObject, 15.0f);
    }

}
	


