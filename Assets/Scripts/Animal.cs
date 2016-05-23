using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;

public class Animal : BasicMovements
{
    protected readonly string WATER = "Water";
    protected readonly string MEAT = "Meat";
    protected readonly string HAY = "Hay";


    public GameObject deadBody;

    protected RaycastHit2D hit;
    public Transform sightEnd0, sightEnd1, sightEnd2, sightEnd3, sightEnd4, sightEnd5, sightEnd6, sightEnd7, sightEnd8;

    //private Attributes attributes = new Attributes();

   // public float hungerSpeed = .1f;
    //public float thirstSpeed = .1f;

    //public float currentThirst = 0;
    //public float currentHunger = 0;
    //public float currentHealth = 0;

    public bool Spotted = false;
    public Vector3 WaterLocation = new Vector3(-1, -1, -1);
    public Vector3 FoodLocation = new Vector3(-1, -1, -1);
    public bool AnimalSmell;

    protected int _steps;
    public int _decisionNo;

    public GameObject enemy;
    protected Animator Animator;
    
    public bool cond;

    protected void Start()
    {

        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;
        _decisionNo = 0;

        _steps = 0;

        attr.SetAttributes(tag);
        cond = false;
        enemy = GameObject.Find("Turtle").gameObject;
        Animator = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(0, 0, 0);
        

        //rotationSpeed = attributes.GetAttribute("agility");
        //movementSpeed = attributes.GetAttribute("speed");
        //currentHunger = attributes.GetAttribute("hunger");
        //currentThirst = attributes.GetAttribute("thirst");

    }

    protected void Update()
    {
        

        HealthHandler();
        

        //attr.OnGUI();

        //RandomMov1();

        // Debug.Log(enemy.GetComponent<Animal>().backPointC.position);
        //if(cond == false)
        //   cond = GetWater();

        //Debug.Log(enemy.GetComponent<Animal>().backPointC.position - transform.position);
        /*
        getAnglePos();
        if(transform.eulerAngles.z != 9) {
            turnLeft(25);
        }
        */

        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
        }*/

    }

    void RandomMov2()
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

    protected void RandomMov()
    {
        //Here I will create random generated actions
        //I will probabily do it in turns of moving forward/ backwards and rotating left/right
        if(MakeDecision())
            _decisionNo = _decisionNo > 4 ? Random.Range(0, 2) : Random.Range(8, 10);

        if (NearObstacle(""))
        {
            StopAction();
            _decisionNo = 7;
        }
            

    }

    protected void ControlledMov()
    {
        Movement();
        Rotation();
    }

    public void RayCasting()
    {
        Spotted = IsSpotted(OBSTACLE);
        if (!Spotted)
        {
            Spotted = IsSpotted(WORLD_END);
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

    public bool CanSmell(string animal)
    {
        GameObject collided = null;
        if( Physics2D.Linecast(head.position, transform.TransformPoint(0, 0, 1), 1 << LayerMask.NameToLayer("smell")).collider != null) { 
            collided = Physics2D.Linecast(head.position, transform.TransformPoint(0, 0, 1)
                , 1 << LayerMask.NameToLayer("smell")).collider.gameObject;
        }
        if (collided == null) return false;
        return collided.gameObject.GetComponent<Smell>().GetParentName().Equals(animal);
    }

    public bool IsSpotted(string layerName)
    {
        return Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(layerName))
               || Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(layerName));
    }

    public float GetFitnessLevel()
    {
        return attr.GetFitness();
    }

    /*public bool GoAround()
    {
        if(!Physics2D.OverlapPoint(sightEnd4.position, 1 << LayerMask.NameToLayer("Block")).gameObject.tag.Equals("Block")  
         || !Physics2D.OverlapPoint(sightEnd4.position, 1 << LayerMask.NameToLayer("Block")).gameObject.tag.Equals("wall_block"))
            if(Physics2D.Linecast(transform.position, sightEnd0.position, 1 << LayerMask.NameToLayer("Block")).collider.gameObject != null
             || Physics2D.Linecast(transform.position, sightEnd0.position, 1 << LayerMask.NameToLayer("wall_block")).collider.gameObject != null)
                GoToLocation(Vector3 targetPos)


        Physics2D.LinecastAll(transform.position, sightEnd8.position, 1 << LayerMask.NameToLayer("wall_block"));

        return true;
    }*/

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
                    _target = Random.Range(30, 80);
                    //Debug.Log("turn left");
                    //Debug.Log(anglePos + " > " + (anglePos + targetPoint) %360 + " XXXXXXXXXXXXXXXXXXXXX");
                }
                return (TurnLeft(_target));
                break;
            case 9:
                if (!isRotating_flag)
                {
                    _target = Random.Range(30, 80);
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

        if(other.gameObject.tag.Equals(WATER))
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");
            attr.CurrentThirst -= Time.deltaTime * 50f;
            if (attr.CurrentThirst < 0)
                attr.CurrentThirst = 0;
            Debug.Log("test2");
        }

    }

    public bool GetWater()
    {
        if (attr.CurrentThirst < 20)
        {
            StopAction();
            _steps = 0;
            return true;
        }
        switch (_steps)
        {
            case 1:
                if (!GoToLocation(WaterLocation))
                {
                    if (Vector3.Distance(transform.position, FoodLocation) < 0.4)
                    {
                        StopAction();
                        _steps++;
                    }
                }
                break;
            case 2:
                if (SmallMoveBackward()) _steps--;
                break;
            default:
                if (WaterLocation != new Vector3(-1, -1, -1))
                {
                    _steps++;
                }
                RandomMov();
                if (IsSpotted(WATER))
                {
                    StopAction();
                    GetLocation(WATER);
                    _steps++;
                }
                break;
        }
        return false;
    }

    public void GetLocation(string type)
    {
        if (Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }
        else if (Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(type)).collider != null)
        {
            if (type.Equals(WATER))
                WaterLocation = Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(MEAT))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
            else if (type.Equals(HAY))
                FoodLocation = Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(type))
                        .collider.transform.position;
        }

    }


    protected void DieNow()
    {
        Instantiate(deadBody, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void HealthHandler()
    {
        attr.CurrentHealth += Time.deltaTime*attr.RegenerationSpeed;
        attr.CurrentHunger += Time.deltaTime * attr.HungerSpeed;
        attr.CurrentThirst += Time.deltaTime * attr.ThirstSpeed;

        if (attr.CurrentHunger >= 100)
        {
            attr.CurrentHunger = 100;
        }
        if (attr.CurrentThirst >= 100)
        {
            attr.CurrentThirst = 100;
        }
        if (attr.CurrentHealth <= 0)
        {
            attr.CurrentHealth = 0;
        }
        else if (attr.CurrentHealth > 100)
        {
            attr.CurrentHealth = 100;
        }

        if (attr.CurrentThirst >= 100
            || attr.CurrentHunger >= 100
            || attr.CurrentHealth <= 0)
        {
            DieNow();
        }
    }


}

