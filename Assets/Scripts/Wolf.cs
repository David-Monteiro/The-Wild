using UnityEngine;
using System.Collections;

public class Wolf : Carnivorous
{


    public new void Start(){
         base.Start();
        Animator = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(0, 0, 0);

    }
    /*public Wolf(){
        isMoving_flag = false;
        isRotating_flag = false;
        leftRotDone_flag = false;
        rightRotDone_flag = false;     
    }*/
    public new void Update()
    {
        /*
            base.Update();
            base.animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
            base.animator.SetBool("Animation_Mov_State", isMoving_flag);
        */
    }

    public void Mov() {
        base.Update();
        Animator.SetInteger("Animation_Rotation_State", (int)transform.rotation.eulerAngles.z);
        Animator.SetBool("Animation_Mov_State", isMoving_flag);
    }
}
