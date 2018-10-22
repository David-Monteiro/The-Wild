using UnityEngine;
using System.Collections;
using System.Linq;

public class Herbivorous : Animal
{
    protected string[] Carnivorous;
    public GameObject Predator;
    public bool PredatorSpotted;
    public bool rule_flag;
    public int _ruleNo;

    public Vector3 LocationTarget = Vector3.zero;
    

    public new void Start()
    {
        base.Start();
        Carnivorous = new [] {"Bear", "Wolf"};
        _ruleNo = 0;
        rule_flag = false;
    }

    public new void Update()
    {
        if (!rule_flag)
        { 
            SetRules();
        }
<<<<<<< HEAD
        //rule_flag = !MakeRule();
=======
       // rule_flag = !MakeRule();
>>>>>>> parent of 6beaad9... The Wild
    }

    protected new void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.gameObject.tag .Equals(HAY))
        {

            attr.CurrentHunger -= Time.deltaTime * 50f;
            if (attr.CurrentHunger < 0)
                attr.CurrentHunger = 0;
        }
        

        foreach (var animal in Carnivorous.Where(animal => other.gameObject.tag.Equals(animal)))
        {
            attr.CurrentHealth -= Time.deltaTime * (50f - (attr.GetDefenceStats() / 400000));
        }

    }

    public bool GetFood()
    {
        if (attr.CurrentHunger < 20)
        {
            StopAction();
            _steps = 0;
            return true;
        }
        switch (_steps)
        {
            case 1:
                if (!GoToLocation(FoodLocation))
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
                RandomMov();
                if (IsSpotted(HAY))
                {
                    StopAction();
                    GetLocation(HAY);
                    _steps++;
                }
                break;
        }
        return false;
    }

    protected void SetRules()
    {
        if (PredatorSpotted)
        {
            _ruleNo = 1;
        }
        else if (attr.CurrentThirst > 70)
        { 
            _ruleNo = 2;
        }
        else if (attr.CurrentHunger > 70)
        { 
            _ruleNo = 3;
        }
        else if (attr.CurrentHealth < 20)
        {
            _ruleNo = 1;
        }
        else
        {
            _ruleNo = 0;
        }
        StopAction();

    }

    protected bool MakeRule()
    {
        switch (_ruleNo)
        {
            case 1:
                if (UTurnMove()) { 
                    Debug.Log("Runnign away");
                    return (BigMoveForward());
                }
                return false;
            case 2:
                Debug.Log("Getting water");
                return GetWater();
            case 3:
                Debug.Log("Getting food");
                return GetFood();
            default:
                Debug.Log("Random movements");
                RandomMov();
                return true;
        }
    }
}
