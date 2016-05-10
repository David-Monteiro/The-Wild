using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Animal : MonoBehaviour
{
    private Attributes attributes = new Attributes();

    public float movementSpeed;
    public float rotationSpeed;

    public float hungerSpeed = .1f;
    public float thirstSpeed = .1f;

    public float currentThirst = 0;
    public float currentHunger = 0;
    public float currentHealth = 0;

    protected RaycastHit2D hit;
    public Transform sightEnd0, sightEnd1, sightEnd2, sightEnd3, sightEnd4, sightEnd5, sightEnd6, sightEnd7, sightEnd8;

    public Transform head, tail;
    public Transform frontPointA, frontPointB;
    protected Transform backPointA, backPointB, backPointC;

    public bool Spotted = false;

    private Vector3 _locationTarget;

    private float _anglePos;
    private int _decisionNo;
    protected int _goingToLocationSteps = -1;
    private float _target = 0;

    public bool isMoving_flag;
    public bool isRotating_flag;
    private bool leftRotDone_flag;
    private bool rightRotDone_flag;

    public GameObject enemy;
    protected Animator Animator;
    //protected GameObject Smell;
    public bool cond;

    protected void Start()
    {
        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;
        _decisionNo = 0;

        attributes.SetAttributes();
        cond = false;
        enemy = GameObject.Find("Turtle").gameObject;
        Animator = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(0, 0, 0);
        //Smell = GameObject.Find("smell_mechanism");

        rotationSpeed = attributes.GetAttribute("agility");
        movementSpeed = attributes.GetAttribute("speed");
        currentHunger = attributes.GetAttribute("hunger");
        currentThirst = attributes.GetAttribute("thirst");

    }



    protected void Update()
    {
        RayCasting();

        currentHunger += Time.deltaTime * hungerSpeed;
        currentThirst += Time.deltaTime * thirstSpeed;
        ControlledMov();
        
        //RandomMov1();

        // Debug.Log(enemy.GetComponent<Animal>().backPointC.position);
        //if(cond == false)
        //   cond = GoToLocation(enemy.GetComponent<Animal>().backPointC.position);
        //GoToLocationTemp(enemy.GetComponent<Animal>().backPointC.position);

        //Debug.Log(enemy.GetComponent<Animal>().backPointC.position - transform.position);
        /*
        getAnglePos();
        if(transform.eulerAngles.z != 9) {
            turnLeft(25);
        }
        */

    }

    void RandomMov()
    {

        if (!isRotating_flag || !isMoving_flag)
        {
            if (_decisionNo > 4) _decisionNo = Random.Range(0, 4);
            else
            {
                _decisionNo = Random.Range(4, 10);
                GetAnglePos();
            }
            //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
            MakeDecision();

        }
        else if (isRotating_flag)
        {
            MakeDecision();
        }
        else if (isMoving_flag)
        {
            MakeDecision();
        }
    }

    protected void RandomMov1()
    {
        //Here I will create random generated actions
        //I will probabily do it in turns of moving forward/ backwards and rotating left/right
        if(MakeDecision())
            _decisionNo = _decisionNo > 4 ? Random.Range(0, 2) : Random.Range(8, 10);
        /*
        if (!isRotating_flag && !isMoving_flag)
        {
            _decisionNo = _decisionNo > 4 ? Random.Range(0, 2) : Random.Range(8, 10);
            //Debug.Log("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
            MakeDecision();
        }
        else if (isRotating_flag || isMoving_flag)
        {
            MakeDecision();
        }*/
    }

    void ControlledMov()
    {
        Movement();
        Rotation();
    }

    //Basic Movements
    void Movement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up*movementSpeed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down*movementSpeed*Time.deltaTime);
        }
    }

    void Rotation()
    {

        var forceX = 0f;
        var forceY = 0f;
        float tiltAngle = 25f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(forceX, forceY, tiltAngle)*Time.deltaTime*rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(forceX, forceY, -tiltAngle)*Time.deltaTime*rotationSpeed);
        }
    }

    protected bool SmallMoveForward()
    {
        if (!isMoving_flag)
        {
            _locationTarget = frontPointA.position;
        }
        isMoving_flag = true;
        MoveTowardsPoint(_locationTarget);

        if (_locationTarget == transform.position)
        {
            isMoving_flag = false;
            //Instantiate(Smell, new Vector3(backPointB.position.x, backPointB.position.y, -0.5f), Quaternion.identity);
        }
        else if (CheckForNearObstacle("inFront")) isMoving_flag = false;

        return !isMoving_flag;
    }

    protected bool SmallMoveBackward()
    {
        if (!isMoving_flag)
        {
            _locationTarget = backPointA.position;
        }
        isMoving_flag = true;
        MoveTowardsPoint(_locationTarget);

        if (_locationTarget == transform.position) isMoving_flag = false;
        else if (CheckForNearObstacle("behind")) isMoving_flag = false;
        return !isMoving_flag;
    }

    protected bool BigMoveForward()
    {
        if (!isMoving_flag)
        {
            _locationTarget = frontPointB.position;
        }
        isMoving_flag = true;
        MoveTowardsPoint(_locationTarget);

        if (_locationTarget == transform.position)
        {
            isMoving_flag = false;

            //Instantiate(Smell, new Vector3(backPointB.position.x, backPointB.position.y, -0.5f), Quaternion.identity);
            //Instantiate(Smell, new Vector3(backPointB.position.x, backPointB.position.y, -0.5f), Quaternion.identity);
        }
        else if (CheckForNearObstacle("inFront")) isMoving_flag = false;
        return !isMoving_flag;
    }

    protected bool BigMoveBackward()
    {
        if (!isMoving_flag)
        {
            _locationTarget = backPointB.position;
        }
        isMoving_flag = true;
        MoveTowardsPoint(_locationTarget);

        if (_locationTarget == transform.position)
        {
            isMoving_flag = false;
            //Instantiate(Smell, new Vector3(frontPointA.position.x, frontPointA.position.y, -0.5f), Quaternion.identity);
        }
        else if (CheckForNearObstacle("behind")) isMoving_flag = false;
        return !isMoving_flag;
    }

    protected bool MoveToPoint(Vector3 point)
    {

        if (!isMoving_flag)
        {
            _locationTarget = point;
        }
        isMoving_flag = true;
        MoveTowardsPoint(_locationTarget);

        if (_locationTarget == transform.position)
        {
            isMoving_flag = false;

            //Instantiate(Smell, new Vector3(backPointB.position.x, backPointB.position.y, -0.5f), Quaternion.identity);
            //Instantiate(Smell, new Vector3(backPointB.position.x, backPointB.position.y, -0.5f), Quaternion.identity);
        }
        else if (CheckForNearObstacle("inFront")) isMoving_flag = false;
        return !isMoving_flag;

    }

    protected bool TurnLeft(float angleTurn)
    {
        GetAnglePos();
        isRotating_flag = true;
        float angle = (_anglePos + angleTurn)%360;

        if (leftRotDone_flag == false)
            RotateTowardsAngleZ(angle);

        //Here we finish this method and release the rotating flag if targetPoint as been met
        isRotating_flag = (int) transform.rotation.eulerAngles.z != (int) angle;
        if (!isRotating_flag)
        {
            leftRotDone_flag = false;
        }
        return !isRotating_flag;

    }

    protected bool TurnRight(float angleTurn)
    {
        GetAnglePos();
        isRotating_flag = true;
        int angle = (int) ((_anglePos + (360 - angleTurn))%360);

        if (rightRotDone_flag == false)
            RotateTowardsAngleZ(angle);

        isRotating_flag = (int) transform.rotation.eulerAngles.z != (int) angle;

        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
        }
        return !isRotating_flag;
    }

    protected bool UTurnMove()
    {
        GetAnglePos();
        isRotating_flag = true;
        float myTarget = (int) ((_anglePos + 180)%360);

        RotateTowardsAngleZ(myTarget);

        isRotating_flag = (int) transform.rotation.eulerAngles.z == (int) myTarget;

        if (!isRotating_flag)
        {
            return true;
        }
        return false;
    }


    //Scout Movements
    protected bool LookLeft()
    {
        GetAnglePos();
        isRotating_flag = true;
        float myTarget = (int) ((_anglePos + 60)%360);

        if (leftRotDone_flag == false)
        {
            RotateTowardsAngleZ(myTarget);
            leftRotDone_flag = (int) transform.rotation.eulerAngles.z == (int) myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag)
        {
            RotateTowardsAngleZ(_anglePos);
            rightRotDone_flag = (int) transform.rotation.eulerAngles.z == (int) _anglePos;
        }
        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag);

        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        return false;
    }

    protected bool LookRight()
    {
        GetAnglePos();
        isRotating_flag = true;
        float myTarget = (int) ((_anglePos + (360 - 60))%360);

        if (rightRotDone_flag == false)
        {
            RotateTowardsAngleZ(myTarget);
            rightRotDone_flag = (int) transform.rotation.eulerAngles.z == (int) myTarget;
        }
        else if (rightRotDone_flag && leftRotDone_flag == false)
        {
            RotateTowardsAngleZ(_anglePos);
            leftRotDone_flag = (int) transform.rotation.eulerAngles.z == (int) _anglePos;
        }

        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag);
        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        return false;
    }

    protected bool LookAround()
    {
        GetAnglePos();
        isRotating_flag = true;
        float myTarget;
        if (leftRotDone_flag == false)
        {
            myTarget = (int) ((_anglePos + 20)%360);
            RotateTowardsAngleZ(myTarget);
            leftRotDone_flag = (int) transform.rotation.eulerAngles.z == (int) myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag == true)
        {
            myTarget = (int) ((_anglePos + (360 - 20))%360);
            RotateTowardsAngleZ(myTarget);
            rightRotDone_flag = (int) transform.rotation.eulerAngles.z == (int) myTarget;
        }
        else RotateTowardsAngleZ(_anglePos);

        isRotating_flag = !(rightRotDone_flag && leftRotDone_flag) && (transform.eulerAngles.z == _anglePos);
        if (!isRotating_flag)
        {
            rightRotDone_flag = false;
            leftRotDone_flag = false;
            return true;
        }
        else return false;
    }

    public bool LookAtTarget(Vector3 targetPos)
    {
        GetAnglePos();

        if (!isRotating_flag)
        {
            double angle;

            if (targetPos.x == transform.position.x)
            {
                angle = (targetPos.y < transform.position.y) ? 180 : 0;
            }
            else if (targetPos.y == transform.position.y)
            {
                angle = (targetPos.x < transform.position.x) ? 90 : 270;
            }
            else
            {
                float tangent = (targetPos.x - transform.position.x)/(targetPos.y - transform.position.y);

                angle = Mathf.Atan(tangent)*57.2958;

                if (targetPos.y - transform.position.y < 0) angle -= 180;

                angle = (transform.position.x < targetPos.x && transform.position.y > targetPos.y)
                    ? angle*-1
                    : 360 - angle;

            }

            _target = (float) angle;
        }

        isRotating_flag = true;

        //Debug.Log("Rotation towards: " + _target);

        RotateTowardsAngleZ(_target);

        isRotating_flag = (int) transform.rotation.eulerAngles.z != (int) _target%360;

        //Debug.Log(isRotating_flag);

        return !isRotating_flag;

    }


    //Core functions of movements and rotations
    void RotateTowardsAngleZ(float t0)
    {

        if (transform.eulerAngles.z - t0 == 1 || t0 - transform.eulerAngles.z == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, t0);
        }
        else
        {
            //t0 stands for targetPoint
            float finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, t0, rotationSpeed*Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, finalAngle);
            //once in a while it will break 1/10
            //Debug.Log("diff : " + (int)angleDiff(transform.eulerAngles.z, t0));
            if (-3 <= (int) (AngleDiff(transform.eulerAngles.z, t0)) &&
                (int) (AngleDiff(transform.eulerAngles.z, t0)) <= 3)
            {
                transform.eulerAngles = new Vector3(0, 0, t0);
            }
        }
    }

    void MoveTowardsPoint(Vector3 targetPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPoint.x, targetPoint.y, targetPoint.z),
            movementSpeed*Time.deltaTime);
    }

    //----------------------------------
    public bool GoToLocation(Vector3 targetPos)
    {
        if (targetPos == transform.position) return true;
        //Rotation is not working properly
        switch (_goingToLocationSteps)
        {
            case 1:
                if (MoveToPoint(targetPos))
                    _goingToLocationSteps++;
                //Debug.Log("step 2");
                break;
            default:
                if (LookAtTarget(targetPos))
                    _goingToLocationSteps = 1;
                //Debug.Log("step 1");
                break;
        }
        if (_goingToLocationSteps <= 1) return false;
        _goingToLocationSteps = -1;
        //Debug.Log("final step");
        return true;

    }

    private int TriangleType(int a, int b, int c)
    {
        /*equilateral==0
        *isosceles=1
        *scalene=2
        */
        if (a == b && a == c && b == c) return 0;
        if (a == b || a == c || c == b) return 1;
        return 2;
    }

    public void StopAction()
    {
        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;
        _goingToLocationSteps = -1;
    }

    public void RayCasting()
    {

        Spotted = IsObstacleSpotted("obstacle");
        if (!Spotted)
        {
            Spotted = IsObstacleSpotted("world_end");
        }

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

        /* test
        
        Debug.DrawRay(transform.position, Vector2.up, Color.blue);
        Debug.DrawRay(transform.position, new Vector3(Mathf.Sin(360 - transform.eulerAngles.z),Mathf.Cos(360 - transform.eulerAngles.z),0), Color.blue);
        Debug.Log(new Vector3(Mathf.Sin(transform.eulerAngles.z), Mathf.Cos(transform.eulerAngles.z), 0) + " " + transform.eulerAngles.z);
        */
    }

    bool CheckForNearObstacle(string str)
    {
        if (str.Equals("behind"))
        {
            return Physics2D.Linecast(transform.position, tail.position, 1 << LayerMask.NameToLayer("Block"))
                   || Physics2D.Linecast(transform.position, backPointA.position, 1 << LayerMask.NameToLayer("Block"))
                   || Physics2D.Linecast(transform.position, tail.position, 1 << LayerMask.NameToLayer("wall_block"))
                   ||
                   Physics2D.Linecast(transform.position, backPointA.position, 1 << LayerMask.NameToLayer("wall_block"));
        }

        return Physics2D.Linecast(transform.position, head.position, 1 << LayerMask.NameToLayer("Block"))
               || Physics2D.Linecast(transform.position, frontPointA.position, 1 << LayerMask.NameToLayer("Block"))
               || Physics2D.Linecast(transform.position, head.position, 1 << LayerMask.NameToLayer("wall_block"))
               || Physics2D.Linecast(transform.position, frontPointA.position, 1 << LayerMask.NameToLayer("wall_block"));
    }

    public bool IsObstacleSpotted(string obstacle)
    {
        if (Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(obstacle))
            || Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(obstacle)))
        {
            return true;
        }
        return false;
    }

    private float GetAnglePos()
    {
        if (isRotating_flag == false)
        {
            _anglePos = transform.eulerAngles.z;
        }
        return _anglePos;
    }

    private float AngleDiff(float angleA, float angleB)
    {
        float diff = angleA - angleB;
        while (diff < -180) diff += 360;
        while (diff > 180) diff -= 360;
        return diff;
    }

    protected bool MakeDecision()
    {
        switch (_decisionNo)
        {
            case 0:
                return (SmallMoveForward());
                //Debug.Log("smallMoveForward");
                break;
            case 1:
                return (BigMoveForward());
                //Debug.Log("bigMoveForward");
                break;
            case 2:
                return (SmallMoveBackward());
                //Debug.Log("smallMoveBackward");
                break;
            case 3:
                return (BigMoveBackward());
                //Debug.Log("bigMoveBackward");
                break;
            case 4:
                return (LookLeft());
                //Debug.Log("lookLeft");
                break;
            case 5:
                return (LookRight());
                //Debug.Log("lookRight");
                break;
            case 6:
                return (LookAround());
                //Debug.Log("lookAround");
                break;
            case 7:
                return (UTurnMove());
                //Debug.Log("uTurnMove");
                break;
            case 8:
                if (!isRotating_flag)
                {
                    _target = Random.Range(4, 45);
                    //Debug.Log("turn left");
                    //Debug.Log(anglePos + " > " + (anglePos + targetPoint) %360 + " XXXXXXXXXXXXXXXXXXXXX");
                }
                return (TurnLeft(_target));
                break;
            case 9:
                if (!isRotating_flag)
                {
                    _target = Random.Range(4, 45);
                    //Debug.Log("turn right");
                    //Debug.Log(anglePos + " > " + (anglePos- targetPoint) %360 + " XXXXXXXXXXXXXXXXXXXXX");
                }
                return (TurnRight(_target));
                break;
            default:
                return false;

        }


    }

    void setAttributes()
    {

    }

    /*public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("test1");
        if (other.tag == "Collectible")
        {
            //currentScore++;
            Destroy(other.gameObject);
        }

        if (other.tag.Equals("water"))
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");
            currentThirst += Time.deltaTime*5;
            Debug.Log("test2");

        }

        if (other.tag == "Meat" || other.tag == "Grass")
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");

        }

    }*/

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("test1");
        if (other.gameObject.tag == "Collectible")
        {
            //currentScore++;
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag.Equals("Water"))
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");
            currentThirst -= Time.deltaTime * 50f;
            if (currentThirst < 0)
                currentThirst = 0;
            Debug.Log("test2");
        }

    }
}

