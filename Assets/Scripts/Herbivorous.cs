using UnityEngine;
using System.Collections;

public class Herbivorous : Animal{


    protected new void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.gameObject.tag == "Grass")
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");
            currentHunger -= Time.deltaTime * 50f;
            if (currentHunger < 0)
                currentHunger = 0;
            Debug.Log("test2");

        }

    }
}
