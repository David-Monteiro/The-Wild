using UnityEngine;
using System.Collections;

public class Bear : Carnivorous
{
    
    public bool rule_flag;
    public int _ruleNo;

    public Vector3 LocationTarget = Vector3.zero;
    

    public new void Start()
    {
        base.Start();

        _ruleNo = 0;
        rule_flag = false;
    }

    public new void Update()
    {
        base.Update();

        RayCasting();

        Animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
        Animator.SetBool("Animation_Mov_State", isMoving_flag);

        if (!rule_flag)
        { 
            SetRules();
        }
        rule_flag = !MakeRule();

    }

   

    

    protected void SetRules()
    {
        if (PreySpotted)
        {
            _ruleNo = 3;
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
                if (UTurnMove())
                    return (BigMoveForward());

                return false;
            case 2:
                return GetWater();
            case 3:
                return GetFood();
            default:
                RandomMov();
                return true;
        }
    }
}
