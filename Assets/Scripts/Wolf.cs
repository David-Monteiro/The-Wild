using UnityEngine;
using System.Collections;

public class Wolf : Carnivorous
{
    new void Start(){
        base.Start();

    }
    new void Update()
    {
        base.Update();
        base.animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
        base.animator.SetBool("Animation_Mov_State", isMoving_flag);
    }
}
