using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Animal : BasicMovements
{
    private Attributes attributes = new Attributes();

    public float hungerSpeed = .1f;
    public float thirstSpeed = .1f;
    
    public float currentThirst = 0;
    public float currentHunger = 0;
    public float currentHealth = 0;

    protected RaycastHit2D hit;
    public Transform sightEnd0, sightEnd1, sightEnd2, sightEnd3, sightEnd4, sightEnd5, sightEnd6, sightEnd7, sightEnd8;

    public bool Spotted = false;

    private int _decisionNo;

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

    private void ControlledMov()
    {
        Movement();
        Rotation();
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

