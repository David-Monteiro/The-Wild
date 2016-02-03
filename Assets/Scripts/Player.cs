using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed;
    public float rotationSpeed;
    public Vector2 maxVelocity = new Vector2(3, 5);

    public Transform sightStart0, sightEnd0;
    public Transform sightStart1, sightEnd1;
    public Transform sightStart2, sightEnd2;

    public bool spotted = false;
    int i = 0;
    void Update()
    {

        movement();
        rotation();
        RayCasting();

        //if (Input.GetKey(KeyCode.W)){ smallMoveForward(); }
        //if (Input.GetKey(KeyCode.UpArrow)) { bigMoveForward(); }
        //if (Input.GetKey(KeyCode.U)) { uTurnMove(); }
        //if (Input.GetKey(KeyCode.LeftArrow)) { lookLeft(); }
        //if (i==0) lookRight(); 
        //        i++;
        
        //if (Input.GetKey(KeyCode.O)) { lookAround(); }

        //bool cond = (int)transform.rotation.eulerAngles.z == (int)((0 + 450) % 360);
        //Debug.Log((int)((0 + 450) % 360));
        //Debug.Log((int)transform.rotation.eulerAngles.z);
        //Debug.Log(cond);
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

    void smallMoveForward() {
        int i = 0;
        while (i < 4)
        {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
            i++;
        }
    }
    void bigMoveForward() {
        int i = 0;
        while (i < 8) {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
            i++;
        }
    }
    void uTurnMove() {
        var X = 0f;
        var Y = 0f;
        float angle = 180;

        transform.Rotate(new Vector3(X, Y, 360) * Time.deltaTime * rotationSpeed);
    }
    void lookLeft() {
        var X = 0f;
        var Y = 0f;
        float angle = 180;

        transform.Rotate(new Vector3(X, Y, angle) * Time.deltaTime * rotationSpeed);
    }

    bool lookRight() {
        lookRight0(transform.rotation.eulerAngles.z, (transform.rotation.eulerAngles.z + 90) % 360);
        return true;
    }
    void lookRight0(float fromZ, float toZ) {

        float X = transform.position.x;
        float Y = transform.position.y;
        float Z = transform.position.z;

        //Try to add recursion to terminate this method
        
    
        if (transform.rotation.eulerAngles.z == toZ) {
            transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, -1), 90 * Time.deltaTime);
        }
        else if(transform.rotation.eulerAngles.z == fromZ)
        {
            transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, 1), 90 * Time.deltaTime);
            lookRight0(fromZ, toZ); Debug.Log(toZ);
            //if (transform.rotation.eulerAngles.z != fromZ)
            //    transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, -1), 90 * Time.deltaTime);
        }

        //transform.Rotate(new Vector3(X, Y, -angle) * Time.deltaTime * rotationSpeed);
        // transform.Rotate(new Vector3(X, Y, angle) * Time.deltaTime * rotationSpeed);




    }
    void lookAround() {
        lookRight();
        lookLeft();
    }
    void goToLocation(Vector3 loc) { }


    void RayCasting() {
        if (Physics2D.Linecast(sightStart0.position, sightEnd0.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(sightStart0.position, sightEnd1.position, 1 << LayerMask.NameToLayer("Block")) 
            || Physics2D.Linecast(sightStart0.position, sightEnd2.position, 1 << LayerMask.NameToLayer("Block")))
            spotted = true;
        else spotted = false;
        Debug.DrawLine(sightStart0.position, sightEnd0.position, Color.green);
        Debug.DrawLine(sightStart0.position, sightEnd1.position, Color.green);
        Debug.DrawLine(sightStart0.position, sightEnd2.position, Color.green);
    }

}
