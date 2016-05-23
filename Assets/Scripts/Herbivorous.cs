using UnityEngine;
using System.Collections;

public class Herbivorous : Animal
{


    protected new void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.gameObject.tag .Equals(HAY))
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");
            attr.CurrentHunger -= Time.deltaTime * 50f;
            if (attr.CurrentHunger < 0)
                attr.CurrentHunger = 0;
            Debug.Log("test2");

        }




    }


}
