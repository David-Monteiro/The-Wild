using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public float movementSpeed;
    public float rotationSpeed;
    private Vector2 maxVelocity = new Vector2(3, 5);

    public Transform sightStart0, sightEnd0;
    public Transform sightStart1, sightEnd1;
    public Transform sightStart2, sightEnd2;

    public Transform head, tail;
    public Transform frontPointA, frontPointB;
    public Transform backPointA, backPointB;

    public bool spotted = false;
    //int? anglePos = null;
    private Vector3 locationTarget;

    private float anglePos;
    private int decisionNo = -1;
    private float target = 0;

    public bool isMoving_flag;
    public bool isRotating_flag;
    public bool leftRotDone_flag;
    public bool rightRotDone_flag;

    //GameObject enemy;

    void Start()
    {
        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;

        //enemy = GameObject.Find("animal (1)");

        //Debug.Log(enemy.transform.position.x + "" + enemy.transform.position.y);
    }

    void Update()
    {
        RayCasting();
        //controlledMov();
        randomMov();

        //Debug.Log(head.position);

        //goToLocation(new Vector2(enemy.transform.position.x, enemy.transform.position.y));

        /*getAnglePos();
        if(transform.eulerAngles.z != 9) {
            turnLeft(25);
        }*/

        //lookLeft error
        //lookAround eroor

    }

    void randomMov()
    {
        //Here I will create random generated actions
        //I will probabily do it in turns of moving forward/ backwards and rotating left/right
        if (!isRotating_flag || !isMoving_flag)
        {
            if (decisionNo > 4) decisionNo = Random.Range(0, 4);
            else
            {
                decisionNo = Random.Range(4, 10);
                getAnglePos();
            }
            Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
            makeDecision();
            
        }
        else if (isRotating_flag)
        {
            makeDecision();
        }
        else if (isMoving_flag) {
            makeDecision();
        }
    }
    void controlledMov()
    {
        movement();
        rotation();
    }

    void movement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * movementSpeed * Time.deltaTime);
        }
    }
    void rotation()
    {

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
        if (!isMoving_flag) {
            locationTarget = frontPointA.position;
        }
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (Physics2D.Linecast(transform.position, head.position, 1 
            << LayerMask.NameToLayer("Block"))) isMoving_flag = false;
        else isMoving_flag = true;
    }
    void smallMoveBackward() {
        if (!isMoving_flag) {
            locationTarget = backPointA.position;
        }
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (Physics2D.Linecast(transform.position, tail.position, 1
            << LayerMask.NameToLayer("Block")))     isMoving_flag = false;
        else isMoving_flag = true;
    }

    void bigMoveForward() {
        if (!isMoving_flag) {
            locationTarget = frontPointB.position;
            //Debug.Log("From: " + transform.position + " , To: " + locationTarget);
        }
        moveTowardsPoint(locationTarget);

        if(locationTarget == transform.position)    isMoving_flag = false;
        else if (Physics2D.Linecast(transform.position, head.position, 1
            << LayerMask.NameToLayer("Block")))     isMoving_flag = false;
        else    isMoving_flag = true;
    }
    void bigMoveBackward() {
        if (!isMoving_flag)  {
            locationTarget = backPointB.position;
            //Debug.Log("From: " + transform.position + " , To: " + locationTarget);
        }
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (Physics2D.Linecast(transform.position, head.position, 1
            << LayerMask.NameToLayer("Block")))     isMoving_flag = false;
        else isMoving_flag = true;
    }


    //
    bool turnLeft(float angleTurn){
        float angle = ((anglePos + angleTurn) % 360);

        if (leftRotDone_flag == false)
            rotZAngles(angle);

        //Here we finish this method and release the rotating flag if target as been met
        isRotating_flag = !((int)transform.rotation.eulerAngles.z == (int)angle);
        Debug.Log(isRotating_flag);
        Debug.Log("Angle " + (int)transform.eulerAngles.z);
        if(!isRotating_flag){
            leftRotDone_flag = false;
        }
        return !isRotating_flag;

    }
    bool turnRight(float angleTurn){
        int angle = (int)((anglePos + (360 - angleTurn)) % 360);

        if (rightRotDone_flag == false)
            rotZAngles(angle);

        isRotating_flag = !((int)transform.rotation.eulerAngles.z == (int)angle);
        Debug.Log(isRotating_flag);
        Debug.Log("Angle " + (int)transform.eulerAngles.z);

        if (!isRotating_flag) {
            rightRotDone_flag = false;
        }
        return !isRotating_flag;
    }
    bool uTurnMove(float initialPos)
    {
        //xxxxxxxx
        float myTarget = (int)((initialPos + 180) % 360);

        rotZAngles(myTarget);

        isRotating_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget; ;
        if (!isRotating_flag)
        {
            return true;
        }
        return false;
    }


    //Scout Movements
    bool lookLeft(float initialPos)
    {
        float myTarget = (int)((initialPos + 60) % 360);

        if (leftRotDone_flag == false)
        {
            rotZAngles(myTarget);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag == true) {
            rotZAngles(initialPos);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)initialPos;
        }
        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag);

        if (!isRotating_flag) {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        return false;
    }
    bool lookRight(float initialPos)
    {
        float myTarget = (int)((initialPos + (360 - 60)) % 360);

        if (rightRotDone_flag == false) {
            rotZAngles(myTarget);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == true && leftRotDone_flag == false) {
            rotZAngles(initialPos);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)initialPos;
        }

        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag);
        if (!isRotating_flag) {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        return false;
    }
    bool lookAround(float initialPos)
    {
        float myTarget;
        if (leftRotDone_flag == false)
        {
            myTarget = (int)((initialPos + 20) % 360);
            rotZAngles(myTarget);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag == true)
        {
            myTarget = (int)((initialPos + (360 - 20)) % 360);
            rotZAngles(myTarget);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else rotZAngles(initialPos);

        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag) && (transform.eulerAngles.z == initialPos);
        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        return false;
    }


    //Core of my movements and rotations
    void rotZAngles(float t0)
    {

        if (transform.eulerAngles.z - t0 == 1 || t0 - transform.eulerAngles.z == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, t0);
        }
        else
        {
            //t0 stands for target
            float finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, t0, rotationSpeed * Time.deltaTime);

            if (!isRotating_flag)
                Debug.Log("Final Angle " + finalAngle + " angle " + transform.eulerAngles.z);

            transform.eulerAngles = new Vector3(0, 0, finalAngle);


            //once in a while it will break 1/10
            Debug.Log("diff : " + (int)angleDiff(transform.eulerAngles.z, t0));
            if (-3 <= (int)(angleDiff(transform.eulerAngles.z, t0)) && (int)(angleDiff(transform.eulerAngles.z, t0)) <= 3)
            {
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAA");
                transform.eulerAngles = new Vector3(0, 0, t0);
            }

            //while player is rotating, it will keep setting leftRot true
            //need to update his part????
        }
    }
    void moveTowardsPoint(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y, target.z), movementSpeed * Time.deltaTime);
    }

    void goToLocation(Vector2 target_pos) {
        target_pos.x = target_pos.x - transform.position.x;
        target_pos.y = target_pos.y - transform.position.y;
        float angle = Mathf.Atan2(target_pos.y, target_pos.x) * Mathf.Rad2Deg;
        angle = angle - 90;
        if (angle > 180) turnRight(angle);
        else turnLeft(angle);

        //I need to improve this function
        //Here the 

        Debug.Log(angle);
    }

    void RayCasting()
    {
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

    float getAnglePos()
    {
        if (isRotating_flag == false) { anglePos = transform.eulerAngles.z; }
        return anglePos;
    }

    private float angleDiff(float angleA, float angleB){
        float diff = angleA - angleB;
        while (diff < -180) diff += 360;
        while (diff > 180) diff -= 360;
        return diff;
    }

    void makeDecision()
    {
        if (decisionNo == 0)
        {

                smallMoveForward();

            Debug.Log("smallMoveForward");
        }
        else if (decisionNo == 1)
        {

                smallMoveBackward();

            Debug.Log("smallMoveBackward");
        }
        else if (decisionNo == 2)
        {

                bigMoveForward();

            Debug.Log("bigMoveForward");
        }
        else if (decisionNo == 3) {

                bigMoveBackward();

            Debug.Log("bigMoveBackward");
        }
        else if (decisionNo == 4)
        {
            lookLeft(anglePos);
            Debug.Log("lookLeft");
            Debug.Log(transform.rotation.eulerAngles.z);
        }
        else if (decisionNo == 5)
        {
            lookRight(anglePos);
            Debug.Log("lookRight");
            Debug.Log(transform.rotation.eulerAngles.z);
        }
        else if (decisionNo == 6)
        {
            lookAround(anglePos);
            Debug.Log("lookAround");
        }
        else if (decisionNo == 7)
        {
            uTurnMove(anglePos);
            Debug.Log("uTurnMove");
        }
        else if (decisionNo == 8)
        {
            if (!isRotating_flag)
            {
                target = Random.Range(4, 45);
                Debug.Log("turn left");
                Debug.Log(anglePos + " > " + (anglePos + target)%360 + " XXXXXXXXXXXXXXXXXXXXX");
            }
            turnLeft(target);
        }
        else if (decisionNo == 9)
        {
            if (!isRotating_flag)   
            {
                target = Random.Range(4, 45);
                Debug.Log("turn right");
                Debug.Log(anglePos + " > " + (anglePos-target)%360 + " XXXXXXXXXXXXXXXXXXXXX");
            }
            turnRight(target);    
        }
        else { }
    }

    void xSecondsPause() {
        StartCoroutine("Wait");
    }

    IEnumerator Wait() {
        Debug.Log("Pause from >>>>>>>>>> " + Time.time);
        yield return new WaitForSeconds(2);
    }

    Vector3 locationDecider() {
        Vector3 pointA = head.position;
        Vector3 pointB = tail.position;
        Debug.Log(new Vector3(pointA.x * pointB.x, pointA.y * pointB.y, 0));
        return new Vector3(pointA.x * pointB.x, pointA.y * pointB.y, 0);   
    }

}

class playerController : MonoBehaviour {
    
}
