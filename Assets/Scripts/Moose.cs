﻿using UnityEngine;
using System.Collections;

public class Moose : Herbivorous
{

    public new void Start()
    {
        base.Start();
        
    }

    public new void Update()
    {
        base.Update();
    
        RayCasting();

        //GetWater();

        Animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
        Animator.SetBool("Animation_Mov_State", isMoving_flag);
        ControlledMov();
        Debug.Log("Health " + attr.CurrentHealth);

       // RandomMov();
    }


}
