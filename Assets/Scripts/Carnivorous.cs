using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class Carnivorous : Animal
{
    protected string[] Herbivorous;
    public bool PreySpotted;
    protected int SearchSteps;

    public Vector3 LocationTarget = Vector3.zero;

    public GameObject Prey;

    public new void Start()
    {
        base.Start();
        Herbivorous = new[] {"Deer", "Moose", "Elk"};
        PreySpotted = false;
        SearchSteps = 1;
    }

    public new void RayCasting()
    {
        base.RayCasting();

        foreach (var prey in Herbivorous)
        {
            PreySpotted = IsPreySpotted(prey);
            if (PreySpotted) break;
        }
        


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
        if (Prey != null) return true;
        if (IsSpotted(animal))
        {
            if (Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(animal)).collider !=
                null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd0.position, 1 << LayerMask.NameToLayer(animal))
                            .collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(animal)).collider !=
                null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd1.position, 1 << LayerMask.NameToLayer(animal))
                            .collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(animal))
                    .collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd2.position, 1 << LayerMask.NameToLayer(animal))
                            .collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(animal))
                    .collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd3.position, 1 << LayerMask.NameToLayer(animal)).collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd4.position, 1 << LayerMask.NameToLayer(animal)).collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd4.position,
                            1 << LayerMask.NameToLayer(animal)).collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd5.position,
                    1 << LayerMask.NameToLayer(animal)).collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd5.position,
                            1 << LayerMask.NameToLayer(animal)).collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd6.position,
                    1 << LayerMask.NameToLayer(animal)).collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd6.position,
                            1 << LayerMask.NameToLayer(animal)).collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd7.position,
                    1 << LayerMask.NameToLayer(animal)).collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd7.position,
                            1 << LayerMask.NameToLayer(animal)).collider.gameObject;
            }
            else if (
                Physics2D.Linecast(head.position, sightEnd8.position,
                    1 << LayerMask.NameToLayer(animal)).collider != null)
            {
                if (Prey == null)
                    Prey =
                        Physics2D.Linecast(head.position, sightEnd8.position,
                            1 << LayerMask.NameToLayer(animal)).collider.gameObject;
            }
            return true;
        }
        return false;
    }

    public bool canSmellPrey()
    {
        foreach (var animal in Herbivorous)
        {
            if (CanSmell(animal))
            {
                Debug.Log(animal);
                return true;
            }
        }
        return false;
    }

    public void SearchPrey()
    {
        RandomMov();
    }

    public bool GoToUnseen(Vector3 targetPos)
    {

        bool unseenPath = !inPreyVisionArea(targetPos);

        if (!unseenPath)
        {
            if (LocationTarget == Vector3.zero)
            {
                var i = 1;
                Vector3 intersection1 = Vector3.one;
                Vector3 intersection2 = Vector3.one;
                while (i < 92 && !unseenPath)
                {

                    double b = Vector3.Distance(head.position, targetPos);
                    double c = b;

                    //double cosA = Mathf.Cos((float)(2 * (Mathf.PI / 180.0))) ;
                    double cosA = Mathf.Cos(i*Mathf.Deg2Rad);
                    double a = Mathf.Sqrt((float) (b*b + c*c - 2*b*c*cosA));

                    // Find e and h.
                    double e = b == 0 ? 0 : (c*c - a*a + b*b)/(2*b);
                    double h = Math.Sqrt(c*c - e*e);

                    // Find point between AB > D.
                    double cx2 = transform.position.x + a*(targetPos.x - transform.position.x)/b;
                    double cy2 = transform.position.y + a*(targetPos.y - transform.position.y)/b;

                    // Get the points P3.
                    intersection1 = new Vector3(
                        (float) (cx2 + h*(transform.position.y - targetPos.y)/b),
                        (float) (cy2 - h*(transform.position.x - targetPos.x)/b));
                    intersection2 = new Vector3(
                        (float) (cx2 - h*(transform.position.y - targetPos.y)/b),
                        (float) (cy2 + h*(transform.position.x - targetPos.x)/b));

                    intersection1 = intersection1 + (targetPos - transform.position);
                    intersection2 = intersection2 + (targetPos - transform.position);

                    if (!inPreyVisionArea(intersection1))
                    {
                        unseenPath = true;
                        LocationTarget = intersection1;
                    }
                    if (!inPreyVisionArea(intersection2))
                    {
                        unseenPath = true;
                        LocationTarget = intersection2;
                    }
                    i += 10;
                }
                /*
                        if direct path is through prey field of vision
                        Need to find an unseen path 
                        Then move towards unseen path 
                        Check again
                        if unseen go to target location
                    */
                if (!unseenPath)
                {
                    var temp = new Vector3(2, 0, 0);
                    if (ObstacleBetweenTarget(temp))
                        temp.x = -temp.x;
                    LocationTarget = transform.TransformPoint(temp);

                }

            }

        }

        else if (LocationTarget != targetPos)
        {
            StopAction();
            LocationTarget = targetPos;
        }
        if (GoToLocation(LocationTarget))
        {

            if (LocationTarget != targetPos)
            {
                StopAction();
                LocationTarget = Vector3.zero;
            }
            else
            {
                StopAction();
                LocationTarget = Vector3.zero;
                return true;
            }
        }

        return false;
    }

    public bool inPreyVisionArea(Vector3 point)
    {

        var hits = Physics2D.LinecastAll(transform.position, point,
            1 << LayerMask.NameToLayer("field_of_vision"));

        var collided = new GameObject[hits.Length];
        for (var i = 0; i < hits.Length; i++)
        {
            collided[i] = hits[i].collider.gameObject;
        }

        foreach (var obj in collided)
        {
            if (obj.transform.parent.gameObject != null)
                if (obj.transform.parent.gameObject.Equals(Prey))
                    return true;
        }

        return false;
    }

    protected bool ObstacleBetweenTarget(Vector3 target)
    {
        return Physics2D.Linecast(transform.position, target, 1 << LayerMask.NameToLayer(OBSTACLE))
               || Physics2D.Linecast(transform.position, target, 1 << LayerMask.NameToLayer(WORLD_END));
    }

    protected new void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.gameObject.tag.Equals("Meat"))
        {
            //ScoreAndHealthSystem sh = (Player)ScoreAndHealthSystem.GetComponent("ScoreAndHealthSystem");
            attr.CurrentHunger -= Time.deltaTime * 50f;
            if (attr.CurrentHunger < 0)
                attr.CurrentHunger = 0;
        }

        /*if (!other.gameObject.tag.Equals(tag) && other.gameObject.tag.Equals("Bear"))
        {
        }

        if (!other.gameObject.tag.Equals(tag) && other.gameObject.tag.Equals("Wolf"))
        {
        }

        if (other.gameObject.tag.Equals("Moose"))
        {
        }*/

        if (other.gameObject.tag.Equals(Herbivorous[0]) 
            || other.gameObject.tag.Equals(Herbivorous[1]) 
            || other.gameObject.tag.Equals(Herbivorous[2]))
        {
            attr.GetAttackStats();

        }
        /*foreach (var animal in Herbivorous.Where(animal => other.gameObject.tag.Equals(animal)))
        {

        }*/

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
                if (IsSpotted(MEAT))
                {
                    StopAction();
                    GetLocation(MEAT);
                    _steps++;
                }
                break;
        }
        return false;
    }

    public void Attack(Vector3 animal)
    {
        switch (_steps)
        {
            case 1:
                if (SmallMoveBackward()) _steps--;
                break;
            default:
                if (!GoToLocation(animal))
                {
                    if (Vector3.Distance(transform.position, FoodLocation) < 0.4)
                    {
                        StopAction();
                        _steps++;
                    }
                }
                break;
        }

    }
}
