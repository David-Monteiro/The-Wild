using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed;
    public float rotationSpeed;
    public Vector2 maxVelocity = new Vector2(3, 5);

    public Transform sightStart0, sightEnd0;
    public Transform sightStart1, sightEnd1;
    public Transform sightStart2, sightEnd2;

    public bool spotted = false;
    int? anglePos = null;
    public bool leftRot = false;
    public bool rightRot = false;
    void Update()
    {

        //movement();
       // rotation();
        RayCasting();

        //if (Input.GetKey(KeyCode.W)){ smallMoveForward(); }
        //if (Input.GetKey(KeyCode.UpArrow)) { bigMoveForward(); }
        //if (Input.GetKey(KeyCode.U)) { uTurnMove(); }
        //if (Input.GetKey(KeyCode.LeftArrow)) { lookLeft(); }
        //if (Input.GetKey(KeyCode.O)) { lookAround(); }

        if (!anglePos.HasValue){
            anglePos = (int)transform.eulerAngles.z;
            Debug.Log(anglePos.Value);
        }
        lookRight(anglePos.Value);

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
    bool lookLeft(float initialPos) {
        int myTarget = (int)((initialPos - 90) % 360);
        if (!leftRot)
            lookLeft0(myTarget);
        else if (!rightRot && leftRot)
            lookRight0(initialPos);
        return true;
    }
    void lookLeft0(float target){
        float finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        leftRot = transform.rotation.eulerAngles.z == target;
    }

    bool lookRight(float initialPos) {
        //lookRight0(transform.rotation.eulerAngles.z, (transform.rotation.eulerAngles.z + 90) % 360);

        int myTarget = (int)((initialPos - 90) % 360);
        Debug.Log(myTarget);

        //if (!rightRot)
        lookRight0(myTarget);
        //else if (rightRot && !leftRot)
        //    lookLeft0(initialPos);
        return true;
    }
    void lookRight0(float target) {
        float finalAngle;

        finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, -(rotationSpeed * Time.deltaTime));

        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        rightRot = transform.rotation.eulerAngles.z == target;
        Debug.Log(finalAngle);

            //add a second float with the initial position minus final angle 
            //
            //
            //
            // if (transform.rotation.eulerAngles.z == toZ) {
            //     transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, -1), rotationSpeed * Time.deltaTime);
            // }
            // else if(transform.rotation.eulerAngles.z == fromZ) {
            //     transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
            //     lookRight0(fromZ, toZ); Debug.Log(toZ);
            // } 
    }
    
    void lookAround() {
        //lookRight();
        //lookLeft();
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
