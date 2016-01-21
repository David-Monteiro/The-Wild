using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float movementSpeed;
    public float rotationSpeed;
    public Vector2 maxVelocity = new Vector2(3, 5);

    void Update () {

        var forceX = 0f;
        var forceY = 0f;

        /* var absVelX = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x);
        if (Input.GetKey("up")) {
            if (absVelX < maxVelocity.x) {
                forceY = speed;
            }
            transform.localScale = new Vector3(1, 1, 1);
        } else if (Input.GetKey("down")) {
            if (absVelX < maxVelocity.x) {
                forceY = -speed;
            }
            transform.localScale = new Vector3(1, -1, 1);
        } else if (Input.GetKeyUp("up") || Input.GetKeyUp("down")) {
            forceX = 0f;
            forceY = 0f;
        }
        

        GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
        */


        movement();
        rotation();
    }

    void movement() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
        }
    }
    void rotation() {

        var forceX = 0f;
        var forceY = 0f;
        float tiltAngle = 25f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(forceX, forceY, tiltAngle) * Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(forceX, forceY, -tiltAngle) * Time.deltaTime * rotationSpeed);
        }
    }
}
