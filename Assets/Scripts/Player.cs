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
    int decisionNo = 0;
    bool isRotating = false;
    public bool leftRotDone = false;
    public bool rightRotDone = false;
    void Update()
    {
     
        RayCasting();
        //controlledMov();
        randomMov();
        /*if (!anglePos.HasValue){
            anglePos = (int)transform.eulerAngles.z;
            Debug.Log(anglePos.Value);
        }*/
        //if(decisionNo < 10) bigMoveForward();
        //decisionNo++;
        
    }

    void randomMov() {
        //Here I will create random generated actions
        //I will probabily do it in turns of moving forward/ backwards and rotating left/right
        if (!isRotating) {
            if (decisionNo > 4) decisionNo = Random.Range(0, 4);
            else {
                decisionNo = Random.Range(4, 10);
                getAnglePos();
            }
            makeDecision();
        }
        else if (isRotating){
            makeDecision();
        }
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
    void smallMoveBackward() {
        int i = 0;
        while (i < 4)
        {
            transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
            i++;
        }
    }

    void bigMoveForward() {
        //int i = 0;
        //while (i < 8) {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        //    i++;
        //}
    }
    void bigMoveBackward() {
        int i = 0;
        while (i < 8)
        {
            transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
            i++;
        }
    }

    bool lookLeft(float initialPos) {
        int myTarget = (int)((initialPos + 90) % 360);

        if (leftRotDone == false)
            rotZAnglesLeft(myTarget);
        else if (rightRotDone == false && leftRotDone == true)
            rotZAnglesRight(initialPos);
        isRotating = (rightRotDone && leftRotDone);
        return true;
    }
    void rotZAnglesLeft(float target){
        float finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        //while player is rotating, it will keep setting leftRot true
        //need to update his part
        leftRotDone = transform.rotation.eulerAngles.z == target;
    }

    bool lookRight(float initialPos) {
        int myTarget = (int)((initialPos + (360 - 90)) % 360);

        if (rightRotDone == false) {
            rotZAnglesRight(myTarget);
        } else if (rightRotDone == true && leftRotDone == false) {
            rotZAnglesLeft(initialPos);
        }
        isRotating = (rightRotDone && leftRotDone);
        return true;
    }
    void rotZAnglesRight(float target) {
   
        float finalAngle;
        finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, target, rotationSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, 0, finalAngle);
        rightRotDone = (int)transform.rotation.eulerAngles.z == (int)target;
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
        if (leftRotDone == false)
            rotZAnglesRight(myTarget);
        else if (rightRotDone == false && leftRotDone == true)
            rotZAnglesLeft((int)((initialPos + (360 - 90)) % 360));
        return true;
    }
    bool uTurnMove(float initialPos)
    {
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

    void makeDecision(){
        if (decisionNo == 0) {
            for (int i = 0; i < 10; i++){
                smallMoveForward();
            }
            Debug.Log("smallMoveForward");
        }
        else if (decisionNo == 1) {
            for (int i = 0; i < 10; i++){
                smallMoveBackward();
            }
            Debug.Log("smallMoveBackward");
        }
        else if (decisionNo == 2) {
            for (int i = 0; i < 25; i++){
                bigMoveForward();
            }
            Debug.Log("bigMoveForward");
        }
        else if (decisionNo == 3) {
            for (int i = 0; i < 25; i++){
                bigMoveBackward();
            }
            Debug.Log("bigMoveBackward");
        }
        else if (decisionNo == 4) {
            lookLeft(anglePos);
            Debug.Log("lookLeft");
        }
        else if (decisionNo == 5) {
            lookRight(anglePos);
            Debug.Log("lookRight");
        }
        else if (decisionNo == 6) {
            lookAround(anglePos);
            Debug.Log("lookAround");
        }
        else if (decisionNo == 7) {
            uTurnMove(anglePos);
            Debug.Log("uTurnMove");
        }
        else if (decisionNo == 8) { }
        else if (decisionNo == 9) { }
        else { }
    }
}
