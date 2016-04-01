using UnityEngine;

public class Simulator : MonoBehaviour {
    Vector3 asd;
    GameObject animal;
    void Start() {
        animal = GameObject.Find("grey_wolf");
    }
	void Update () {
        transform.position = new Vector3(animal.transform.position.x, animal.transform.position.y, -1);
    }
}
