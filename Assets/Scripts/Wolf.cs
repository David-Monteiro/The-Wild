using UnityEngine;
using System.Collections;

public class Wolf : Carnivorous
{

    //public new void Start(){
    //  base.Start();
    //
    //}

    public new void Update()
    {
        base.Update();
        Animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
        Animator.SetBool("Animation_Mov_State", isMoving_flag);
        
    }

}
