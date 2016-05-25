using UnityEngine;
using System.Collections;

public class Wolf : Carnivorous
{

    public new void Start(){
        base.Start();

        var coliiders = GameObject.FindGameObjectsWithTag("Wolf");
        foreach (var co in coliiders)
        {
            Physics2D.IgnoreCollision(co.GetComponent<UnityEngine.BoxCollider2D>(), GetComponent<UnityEngine.BoxCollider2D>());
            
        }

        Debug.Log("My attack: " + attr.GetAttackStats());
        Debug.Log("My defence: " + attr.GetDefenceStats());
        Debug.Log("My fitness :" + attr.GetFitness());
    }

    public new void Update()
    {
        base.Update();

        RayCasting();

        Animator.SetInteger("Animation_Rotation_State", (int) transform.rotation.eulerAngles.z);
        Animator.SetBool("Animation_Mov_State", isMoving_flag);

     
    }


}
