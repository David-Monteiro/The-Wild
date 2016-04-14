using UnityEngine;
using System.Collections;

public class Carnivorous : Animal
{
    protected string[] Herbivorous;
    public bool PreySpotted;
    protected int SearchSteps;

    public GameObject Prey;

    public new void Start() {
        base.Start();
        Herbivorous = new[] { "deer", "moose", "elk" };
        PreySpotted = false;
        SearchSteps = 1;
    }
    public new void RayCasting() {
        base.RayCasting();

        PreySpotted = IsPreySpotted("herbivorous");
        

            // prey = RaycastHit.transform.gameObject : GameObject;

            //RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
            }
            
    

    }

    public bool IsPreySpotted(string animal)
    {
        //make this a void function, instead of returning a bool value it changes the var PreySpotted
        if (Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(animal))
            || Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(animal)))
        {
            if (Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd0.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd1.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if(Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd3.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd4.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd5.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd5.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd6.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd6.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd7.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd7.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            else if (Physics2D.Linecast(head.position, sightEnd8.position, 1 << LayerMask.NameToLayer(animal)).collider != null) {
                if (Prey == null)
                    Prey = Physics2D.Linecast(head.position, sightEnd8.position, LayerMask.NameToLayer(animal)).collider.gameObject;
                Debug.Log("XXXXXXXXXXXXX");
            }
            
            return true;
        }
       return false;
    }

    public void SearchPrey() {
        switch (SearchSteps) {
            case 1:
                if (LookAround()) SearchSteps++;
                break;
            case 2:
                if (BigMoveForward()) SearchSteps++;
                break;
            case 3:
                if (LookRight()) SearchSteps++;
                break;
            case 4:
                if (BigMoveForward()) SearchSteps++;
                break;
            case 5:
                if (LookLeft()) SearchSteps++;
                break;
            case 6:
                if (BigMoveForward()) SearchSteps++;
                break;
            case 7:
                if (LookRight()) SearchSteps++;
                break;
            default:
                SearchSteps = 1;
                break;
        }

    }

}
