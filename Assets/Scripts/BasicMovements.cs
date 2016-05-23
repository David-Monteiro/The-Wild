using UnityEngine;
using System.Collections;

public class BasicMovements : MonoBehaviour {

    //public float movementSpeed;
    //public float rotationSpeed;

    public Attributes attr = new Attributes();

    protected readonly string OBSTACLE = "Obstacle";
    protected readonly string WORLD_END = "World_End";

    public bool isMoving_flag;
    public bool isRotating_flag;
    protected bool leftRotDone_flag;
    protected bool rightRotDone_flag;

    protected Vector3 _locationTarget;
    protected float _anglePos;
    protected float _target = 0;
    protected int _goingToLocationSteps = -1;

    public Transform head, tail;
    public Transform frontPointA, frontPointB;
    public Transform backPointA, backPointB, backPointC;

    public GameObject smellPrefab;
    private int steps = 0;
    protected readonly Vector3 FrontStep = new Vector3(0, -0.5f, 0.5f);
    protected  readonly Vector3 BackStep = new Vector3(0, 0.5f, 0.5f);


    //Controlled Movements
    protected void Movement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * attr.Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * attr.Speed * Time.deltaTime);
        }
    }

    protected void Rotation()
    {

        const float forceX = 0f;
        const float forceY = 0f;
        const float tiltAngle = 25f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(forceX, forceY, tiltAngle) * Time.deltaTime * attr.Agility);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(forceX, forceY, -tiltAngle) * Time.deltaTime * attr.Agility);
        }
    }

    //Basic Movements
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
            _leaveSmellParticle(FrontStep);
        }
        else if (NearObstacle("inFront")) isMoving_flag = false;

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
        else if (NearObstacle("behind")) isMoving_flag = false;
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

            _leaveSmellParticle(FrontStep);
        }
        else if (NearObstacle("inFront")) isMoving_flag = false;
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
            _leaveSmellParticle(BackStep);
        }
        else if (NearObstacle("behind")) isMoving_flag = false;
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
        steps++;
        if (steps == 10)
        {
            steps = 0;
            _leaveSmellParticle(FrontStep);
        }
        if (_locationTarget == transform.position)
        {
            isMoving_flag = false;
        }
        else if (NearObstacle("inFront")) isMoving_flag = false;
        return !isMoving_flag;

    }

    protected bool TurnLeft(float angleTurn)
    {
        GetAnglePos();
        isRotating_flag = true;
        float angle = (_anglePos + angleTurn) % 360;

        if (leftRotDone_flag == false)
            RotateTowardsAngleZ(angle);

        //Here we finish this method and release the rotating flag if targetPoint as been met
        isRotating_flag = (int)transform.rotation.eulerAngles.z != (int)angle;
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
        var angle = (int)((_anglePos + (360 - angleTurn)) % 360);

        if (rightRotDone_flag == false)
            RotateTowardsAngleZ(angle);

        isRotating_flag = (int)transform.rotation.eulerAngles.z != (int)angle;

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
        float myTarget = (int)((_anglePos + 180) % 360);

        RotateTowardsAngleZ(myTarget);

        isRotating_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;

        if (!isRotating_flag)
        {
            return true;
        }
        return false;
    }


    //Complex Movements
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
                float tangent = (targetPos.x - transform.position.x) / (targetPos.y - transform.position.y);

                angle = Mathf.Atan(tangent) * 57.2958;

                if (targetPos.y - transform.position.y < 0) angle -= 180;

                angle = (transform.position.x < targetPos.x && transform.position.y > targetPos.y)
                    ? angle * -1
                    : 360 - angle;

            }

            _target = (float)angle;
        }

        isRotating_flag = true;

        //Debug.Log("Rotation towards: " + _target);

        RotateTowardsAngleZ(_target);

        isRotating_flag = (int)transform.rotation.eulerAngles.z != (int)_target % 360;

        //Debug.Log(isRotating_flag);

        return !isRotating_flag;

    }


    //Scout Movements
    protected bool LookLeft()
    {
        GetAnglePos();
        isRotating_flag = true;
        float myTarget = (int)((_anglePos + 60) % 360);

        if (leftRotDone_flag == false)
        {
            RotateTowardsAngleZ(myTarget);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag == false && leftRotDone_flag)
        {
            RotateTowardsAngleZ(_anglePos);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)_anglePos;
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
        float myTarget = (int)((_anglePos + (360 - 60)) % 360);

        if (rightRotDone_flag == false)
        {
            RotateTowardsAngleZ(myTarget);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (rightRotDone_flag && leftRotDone_flag == false)
        {
            RotateTowardsAngleZ(_anglePos);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)_anglePos;
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
            myTarget = (int)((_anglePos + 20) % 360);
            RotateTowardsAngleZ(myTarget);
            leftRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
        }
        else if (!rightRotDone_flag && leftRotDone_flag)
        {
            myTarget = (int)((_anglePos + (360 - 20)) % 360);
            RotateTowardsAngleZ(myTarget);
            rightRotDone_flag = (int)transform.rotation.eulerAngles.z == (int)myTarget;
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



    //Core functions of movements and rotations
    private void RotateTowardsAngleZ(float t0)
    {

        if (transform.eulerAngles.z - t0 == 1 || t0 - transform.eulerAngles.z == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, t0);
        }
        else
        {
            //t0 stands for targetPoint
            float finalAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, t0, attr.Agility * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, finalAngle);
            //once in a while it will break 1/10
            //Debug.Log("diff : " + (int)angleDiff(transform.eulerAngles.z, t0));
            if (-3 <= (int)(AngleDiff(transform.eulerAngles.z, t0)) &&
                (int)(AngleDiff(transform.eulerAngles.z, t0)) <= 3)
            {
                transform.eulerAngles = new Vector3(0, 0, t0);
            }
        }
    }

    private void MoveTowardsPoint(Vector3 targetPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(targetPoint.x, targetPoint.y, targetPoint.z),
            attr.Speed * Time.deltaTime);
    }

    
    //Obstacle check
    protected bool NearObstacle(string str)
    {
        if (str.Equals("behind"))
        {
            return Physics2D.Linecast(transform.position, tail.position, 1 << LayerMask.NameToLayer(OBSTACLE))
                   || Physics2D.Linecast(transform.position, backPointA.position, 1 << LayerMask.NameToLayer(OBSTACLE))
                   || Physics2D.Linecast(transform.position, tail.position, 1 << LayerMask.NameToLayer(WORLD_END))
                   || Physics2D.Linecast(transform.position, backPointA.position, 1 << LayerMask.NameToLayer(WORLD_END));
        }

        return Physics2D.Linecast(transform.position, head.position, 1 << LayerMask.NameToLayer(OBSTACLE))
               || Physics2D.Linecast(transform.position, frontPointA.position, 1 << LayerMask.NameToLayer(OBSTACLE))
               || Physics2D.Linecast(transform.position, head.position, 1 << LayerMask.NameToLayer(WORLD_END))
               || Physics2D.Linecast(transform.position, frontPointA.position, 1 << LayerMask.NameToLayer(WORLD_END));
    }


    //Tools
    public void StopAction()
    {
        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;
        _goingToLocationSteps = -1;
    }

    private void GetAnglePos()
    {
        if (isRotating_flag == false)
            _anglePos = transform.eulerAngles.z;
    }

    private float AngleDiff(float angleA, float angleB)
    {
        var diff = angleA - angleB;
        while (diff < -180) diff += 360;
        while (diff > 180) diff -= 360;
        return diff;
    }


    private void _leaveSmellParticle(Vector3 step)
    {
        var smell = (GameObject)Instantiate(smellPrefab, transform.TransformPoint(step), Quaternion.identity);
        smell.GetComponent<Smell>().SetParentName(tag);
    }
}
