using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Attributes attributes = new Attributes();

    public float movementSpeed;
    public float rotationSpeed;

    public Transform sightEnd0, sightEnd1, sightEnd2, sightEnd3, sightEnd4, sightEnd5, sightEnd6, sightEnd7, sightEnd8;

    public Transform head, tail;
    public Transform frontPointA, frontPointB;
    public Transform backPointA, backPointB;

    public bool spotted = false;

    private Vector3 locationTarget;

    private float anglePos;
    private int decisionNo = -1;
    private float target = 0;

    public bool isMoving_flag;
    public bool isRotating_flag;
    public bool leftRotDone_flag;
    public bool rightRotDone_flag;

    GameObject enemy;

    private Animator animator;

    void Start()
    {
        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;

        enemy = GameObject.Find("animal (1)");
        //animator = GetComponent<Animator>();

    }
    private bool cond = false;
    void Update()
    {
        RayCasting();
        //controlledMov();
        randomMov1();

        //animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
        //animator.SetBool("Animation_Mov_State", isMoving_flag);
       /*
       if(cond == false)
            goToLocation(enemy.transform.position);
        */

        /*
        getAnglePos();
        if(transform.eulerAngles.z != 9) {
            turnLeft(25);
        }
        */

    }

    void randomMov()
    {
        if (!isRotating_flag || !isMoving_flag)
        {
            if (decisionNo > 4) decisionNo = Random.Range(0, 4);
            else
            {
                decisionNo = Random.Range(4, 10);
                getAnglePos();
            }
            //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
            makeDecision();
        }
        else if (isRotating_flag)
        {
            makeDecision();
        }
        else if (isMoving_flag)
        {
            makeDecision();
        }
    }

    void randomMov1()
    {
        //Here I will create random generated actions
        //I will probabily do it in turns of moving forward/ backwards and rotating left/right
        if (!isRotating_flag && !isMoving_flag)
        {
            if (decisionNo > 4) decisionNo = Random.Range(0, 2);
            else
            {
                decisionNo = Random.Range(8, 10);
            }
            //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
            makeDecision();
        }
        else if (isRotating_flag || isMoving_flag)
        {
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

    bool smallMoveForward()
    {
        if (!isMoving_flag)
        {
            locationTarget = frontPointA.position;
        }
        isMoving_flag = true;
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (checkForObstacle("inFront"))   isMoving_flag = false;

        return !isMoving_flag;
    }
    bool smallMoveBackward()
    {
        if (!isMoving_flag)
        {
            locationTarget = backPointA.position;
        }
        isMoving_flag = true;
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (checkForObstacle("behind")) isMoving_flag = false;
        return !isMoving_flag;
    }

    bool bigMoveForward()
    {
        if (!isMoving_flag)
        {
            locationTarget = frontPointB.position;
        }
        isMoving_flag = true;
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (checkForObstacle("inFront")) isMoving_flag = false;
        return !isMoving_flag; 
    }
    bool bigMoveBackward()
    {
        if (!isMoving_flag)
        {
            locationTarget = backPointB.position;
        }
        isMoving_flag = true;
        moveTowardsPoint(locationTarget);

        if (locationTarget == transform.position) isMoving_flag = false;
        else if (checkForObstacle("behind")) isMoving_flag = false;
        return !isMoving_flag;
    }

    void moveToPoint(Vector3 point) {

        moveTowardsPoint(point);

        if (point == transform.position) isMoving_flag = false;
        else if (checkForObstacle("inFront")) isMoving_flag = false;
        else isMoving_flag = true;
    }
    
    bool turnLeft(float angleTurn)
    {
        getAnglePos();
        isRotating_flag = true;
        float angle = ((anglePos + angleTurn) % 360);

        if (leftRotDone_flag == false)
            rotZAngles(angle);

        //Here we finish this method and release the rotating flag if target as been met
        isRotating_flag = !((int)transform.rotation.eulerAngles.z == (int)angle);
        if (!isRotating_flag)
        {
            leftRotDone_flag = false;
        }
        return !isRotating_flag;

    }
    bool turnRight(float angleTurn)
    {
        getAnglePos();
        isRotating_flag = true;
        int angle = (int)((anglePos + (360 - angleTurn)) % 360);

        if (rightRotDone_flag == false)
            rotZAngles(angle);

        isRotating_flag = !((int)transform.rotation.eulerAngles.z == (int)angle);

        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
        }
        return !isRotating_flag;
    }
    bool uTurnMove()
    {
        getAnglePos();
        isRotating_flag = true;
        float myTarget = (int)((anglePos + 180) % 360);

        rotZAngles(myTarget);

        isRotating_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget; ;
        if (!isRotating_flag)
        {
            return true;
        }
        else return false;
    }


    //Scout Movements
    bool lookLeft()
    {
        getAnglePos();
        isRotating_flag = true;
        float myTarget = (int)((anglePos + 60) % 360);

        if (leftRotDone_flag == false)
        {
            rotZAngles(myTarget);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag == true)
        {
            rotZAngles(anglePos);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)anglePos;
        }
        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag);

        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        else return false;
    }
    bool lookRight()
    {
        getAnglePos();
        isRotating_flag = true;
        float myTarget = (int)((anglePos + (360 - 60)) % 360);

        if (rightRotDone_flag == false)
        {
            rotZAngles(myTarget);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == true && leftRotDone_flag == false)
        {
            rotZAngles(anglePos);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)anglePos;
        }

        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag);
        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        else return false;
    }
    bool lookAround()
    {
        getAnglePos();
        isRotating_flag = true;
        float myTarget;
        if (leftRotDone_flag == false)
        {
            myTarget = (int)((anglePos + 20) % 360);
            rotZAngles(myTarget);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag == true)
        {
            myTarget = (int)((anglePos + (360 - 20)) % 360);
            rotZAngles(myTarget);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else rotZAngles(anglePos);

        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag) && (transform.eulerAngles.z == anglePos);
        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        else return false;
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
            transform.eulerAngles = new Vector3(0, 0, finalAngle);
            //once in a while it will break 1/10
            Debug.Log("diff : " + (int)angleDiff(transform.eulerAngles.z, t0));
            if (-3 <= (int)(angleDiff(transform.eulerAngles.z, t0)) && (int)(angleDiff(transform.eulerAngles.z, t0)) <= 3)
            {
                transform.eulerAngles = new Vector3(0, 0, t0);
            }
        }
    }
    void moveTowardsPoint(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y, target.z), movementSpeed * Time.deltaTime);
    }

    bool goToLocation(Vector3 target_pos)
    {
        getAnglePos();
        target_pos.x = (target_pos.x - transform.position.x)%360;
        target_pos.y = (target_pos.y - transform.position.y)%360;
        float angle = Mathf.Atan2(target_pos.y, target_pos.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle: " + angle);
        if (angle > 180) turnRight(angle);
        else turnLeft(angle);
       // Debug.Log("anglepos: " + ((anglePos + angle) % 360));
        //Debug.Log("Angle: " + transform.rotation.eulerAngles.z);
        if (((anglePos + angle) % 360) == transform.rotation.eulerAngles.z) {
            if (!isMoving_flag) {
                locationTarget = target_pos;
            }
            moveToPoint(locationTarget);
        }
        return target_pos == transform.position;
    }

    void RayCasting()
    {
        if (Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer("Block"))
            || Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer("Block")))
            spotted = true;
        else spotted = false;
        Debug.DrawLine(head.position, sightEnd0.position, Color.green);
        Debug.DrawLine(head.position, sightEnd1.position, Color.green);
        Debug.DrawLine(head.position, sightEnd2.position, Color.green);
        Debug.DrawLine(head.position, sightEnd3.position, Color.green);
        Debug.DrawLine(head.position, sightEnd4.position, Color.green);
        Debug.DrawLine(head.position, sightEnd5.position, Color.green);
        Debug.DrawLine(head.position, sightEnd6.position, Color.green);
        Debug.DrawLine(head.position, sightEnd7.position, Color.green);
        Debug.DrawLine(head.position, sightEnd7.position, Color.green);
        Debug.DrawLine(head.position, sightEnd8.position, Color.green);
    }

    bool checkForObstacle(string str) {
        if (str.Equals("behind")) {
            return (Physics2D.Linecast(transform.position, tail.position, 1 << LayerMask.NameToLayer("Block"))
                || Physics2D.Linecast(transform.position, backPointA.position, 1 << LayerMask.NameToLayer("Block"))
                || Physics2D.Linecast(transform.position, tail.position, 1 << LayerMask.NameToLayer("wall_block"))
                || Physics2D.Linecast(transform.position, backPointA.position, 1 << LayerMask.NameToLayer("wall_block")));
        }
        else {
            return (Physics2D.Linecast(transform.position, head.position, 1 << LayerMask.NameToLayer("Block"))
                || Physics2D.Linecast(transform.position, frontPointA.position, 1 << LayerMask.NameToLayer("Block"))
                || Physics2D.Linecast(transform.position, head.position, 1 << LayerMask.NameToLayer("wall_block"))
                || Physics2D.Linecast(transform.position, frontPointA.position, 1 << LayerMask.NameToLayer("wall_block")));
        }
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
        if (decisionNo == 0) {
            smallMoveForward();
            //Debug.Log("smallMoveForward");
        }
        else if (decisionNo == 1) {
            bigMoveForward();
            //Debug.Log("bigMoveForward");
        }
        else if (decisionNo == 2)
        {
            smallMoveBackward();
            //Debug.Log("smallMoveBackward");
        }
        else if (decisionNo == 3) {
            bigMoveBackward();
            //Debug.Log("bigMoveBackward");
        }
        else if (decisionNo == 4)
        {
            lookLeft();
            //Debug.Log("lookLeft");
        }
        else if (decisionNo == 5)
        {
            lookRight();
            //Debug.Log("lookRight");
        }
        else if (decisionNo == 6)
        {
            lookAround();
            //Debug.Log("lookAround");
        }
        else if (decisionNo == 7)
        {
            uTurnMove();
            //Debug.Log("uTurnMove");
        }
        else if (decisionNo == 8)
        {
            if (!isRotating_flag)
            {
                target = Random.Range(4, 45);
                //Debug.Log("turn left");
                //Debug.Log(anglePos + " > " + (anglePos + target) %360 + " XXXXXXXXXXXXXXXXXXXXX");
            }
            turnLeft(target);
        }
        else if (decisionNo == 9) {
            if (!isRotating_flag)   
            {
                target = Random.Range(4, 45);
                //Debug.Log("turn right");
                //Debug.Log(anglePos + " > " + (anglePos- target) %360 + " XXXXXXXXXXXXXXXXXXXXX");
            }
            turnRight(target);    
        }
        else { }
    }


}
