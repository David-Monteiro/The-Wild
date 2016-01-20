using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
    public float speed = 10f;
    public Vector2 maxVelocity = new Vector2(3, 5);

	void Update () {
        var forceX = 0f;
        var forceY = 0f;

        var absVelX = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x;
        if(Input.GetKey("up")) {
            if (absVelX < maxVelocity.x) {
                forceX = speed;
            }
        } else if (Input.GetKey("down")){
            if (absVelX < maxVelocity.x){
                forceX = speed;
            }
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
    }
}
