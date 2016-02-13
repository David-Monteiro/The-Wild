using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed;
    public float rotationSpeed;
    public Vector2 maxVelocity = new Vector2(3, 5);

    public Transform sightStart0, sightEnd0;
    public Transform sightStart1, sightEnd1;
    public Transform sightStart2, sightEnd2;

    public bool spotted = false;
    //int? anglePos = null;
    float anglePos;
    bool isRotating = false;
    public bool leftRot = false;
    public bool rightRot = false;
    void Update()
    {
     
        RayCasting();
        //controlledMov();
        //if (Input.GetKey(KeyCode.W)){ smallMoveForward(); }
        //if (Input.GetKey(KeyCode.UpArrow)) { bigMoveForward(); }
        //if (Input.GetKey(KeyCode.U)) { uTurnMove(anglePos.Value); }
        //if (Input.GetKey(KeyCode.LeftArrow)) { lookLeft(); }
        //if (Input.GetKey(KeyCode.O)) { lookAround(); }

        /*if (!anglePos.HasValue){
            anglePos = (int)transform.eulerAngles.z;
            Debug.Log(anglePos.Value);
        }*/
        getAnglePos();
        //lookAround(anglePos);
        isRotating = lookRight(anglePos);
        //uTurnMove(anglePos.Value);

        Debug.Log(Random.Range(0, 2));
    }

    void randomMov() {
        //Here I will create random generated actions
        //I will probabily do it in turns of moving forward/ backwards and rotating left/right
    }
    void controlledMov() {
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
    bool uTurnMove(float initialPos) {
        //if (Random.Range(0, 2) == 1) {
        //    int myTarget = (int)((initialPos + (360 - 90)) % 360);
        //    rotZAnglesRight(myTarget);
        //}
        //else{
            int myTarget = (int)((initialPos + 180) % 360);
            rotZAnglesLeft(myTarget);
        //}
        return true;
    }

    bool lookLeft(float initialPos) {
        int myTarget = (int)((initialPos + 90) % 360);


        //need to change implementation to know if the player is rotating
        //and to check whether the player has finished rotating
        if (leftRot == false)
            rotZAnglesLeft(myTarget);
        else if (rightRot == false && leftRot == true)
            rotZAnglesRight(initialPos);
        return true;
    }
    void rotZAnglesLeft(float target){
        float finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        //while player is rotating, it will keep setting leftRot true
        //need to update his part
        leftRot = transform.rotation.eulerAngles.z == target;
    }

    bool lookRight(float initialPos) {
        int myTarget = (int)((initialPos + (360 - 90)) % 360);

        if (rightRot == false) {
            rotZAnglesRight(myTarget);
        } else if (rightRot == true && leftRot == false) {
            rotZAnglesLeft(initialPos);
        }
        return true;
    }
    void rotZAnglesRight(float target) {
   
        float finalAngle;
        finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        rightRot = (int)transform.rotation.eulerAngles.z == (int)target;
        //add a second float with the initial position minus final angle 
        //
        // if (transform.rotation.eulerAngles.z == toZ) {
        //     transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, -1), rotationSpeed * Time.deltaTime);
        // }
        // else if(transform.rotation.eulerAngles.z == fromZ) {
        //     transform.RotateAround(new Vector3(X, Y, Z), new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
        //     lookRight0(fromZ, toZ); Debug.Log(toZ);
        // } 
    }
    
    bool lookAround(float initialPos) {
        int myTarget = (int)((initialPos + 90) % 360);
        if (leftRot == false)
            rotZAnglesRight(myTarget);
        else if (rightRot == false && leftRot == true)
            rotZAnglesLeft((int)((initialPos + (360 - 90)) % 360));
        return true;
    }
    void goToLocation(Vector3 loc) { }


    void RayCasting() {
        if (Physics2D.Linecast(sightStart0.position, sightEnd0.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(sightStart0.position, sightEnd1.position, 1 << LayerMask.NameToLayer("Block")) 
            || Physics2D.Linecast(sightStart0.position, sightEnd2.position, 1 << LayerMask.NameToLayer("Block")))
            spotted = true;
        else spotted = false;
        //Need to add line tof code to recognise the end_world blocks
        //either a bool variable or a float representing the distance between gameObject and wall
        Debug.DrawLine(sightStart0.position, sightEnd0.position, Color.green);
        Debug.DrawLine(sightStart0.position, sightEnd1.position, Color.green);
        Debug.DrawLine(sightStart0.position, sightEnd2.position, Color.green);
    }

    float getAnglePos() {
        if (isRotating == false) { anglePos = transform.eulerAngles.z;}
        return anglePos;
    }

}
