using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed;
    public float rotationSpeed;
    public Vector2 maxVelocity = new Vector2(3, 5);

    public Transform sightStart0, sightEnd0;
    public Transform sightStart1, sightEnd1;
    public Transform sightStart2, sightEnd2;

    public bool spotted = false;
    void Update () {

        movement();
        rotation();
        RayCasting();
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

    void smallMoveForward() { }
    void bigMoveForward() { }
    void uTurnMove() { }
    void lookLeft() { }
    void lookRight() { }
    void lookAround() {
        lookRight();
        lookLeft();
    }
    void goToLocation(Vector3 loc) { }



    void RayCasting() {
        Debug.DrawLine(sightStart0.position, sightEnd0.position, Color.green);
        spotted = Physics2D.Linecast(sightStart0.position, sightEnd0.position, 1 << LayerMask.NameToLayer("Block"));
        transform.Translate(-Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime, 0, 0);

        Debug.DrawLine(sightStart1.position, sightEnd1.position, Color.green);
        spotted = Physics2D.Linecast(sightStart1.position, sightEnd1.position, 1 << LayerMask.NameToLayer("Block"));

        Debug.DrawLine(sightStart2.position, sightEnd2.position, Color.green);
        spotted = Physics2D.Linecast(sightStart2.position, sightEnd2.position, 1 << LayerMask.NameToLayer("Block"));
    }

}
