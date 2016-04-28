using UnityEngine;
using System.Collections;

public class Pack : MonoBehaviour
{

    //This class serves as the controller of all my deployed wolves

    // public Vector2 vector1;
    //public Vector2 vector2;
    // public Vector3 vector3;
    //public Vector3 vector4;

    private Vector3 AlphaPos = new Vector3(0, -2, 0);
    private Vector3 BetaPos = new Vector3(1, -1.75f, 0);
    private Vector3 GammaPos = new Vector3(-1, -1.75f, 0);
    private Vector3 Delta1Pos = new Vector3(1, -1.25f, 0);
    private Vector3 Delta2Pos = new Vector3(-1, -1.25f, 0);
    private Vector3 Delta3Pos = new Vector3(2, -0.75f, 0);
    private Vector3 Delta43Pos = new Vector3(-2, -0.75f, 0);

    private GameObject Prey;

    //Instead of an array of gameObjects, make a list of wolfs and their status in the pack.

    private int _searchAgent;
    //this int will tell which wolf is the one that fount the prey


    public int _huntingSteps;
    private GameObject[] _pack;

    public void Start()
    {
        _huntingSteps = 0;
        _pack = GameObject.FindGameObjectsWithTag("Wolf");
        /*  foreach(GameObject wolf in pack) {

              wolf.GetComponent<Wolf>().Start();

          }*/

        for (var i = 0; i < _pack.Length; i++)
        {
            if (_pack.Length - i == 1)
            {
                _pack[i].GetComponent<Wolf>().Start();
            }
        }


    }

    /* Vector2 vectorResult = new Vector2();


     // vectorResult is equal to (65,100)
     float dotResult = Vector2.Dot(vector2, vector1);
     Debug.Log("dot product: " + dotResult);
     Vector2 product = vector1 * 4;
     // product = product - 2;
     //Debug.Log("product: " + product);
     Vector3 crossResult = Vector3.Cross(vector3, vector4);
     Debug.Log("cross product: " + vector3 * (int)System.Math.Sqrt(11));
     //Debug.Log("cross product: " + (new Vector3(1, 1, 0) * 2));*/




    /*
    If wolf spots prey and its not seem
        Stay  to float points behind the preu.
        Stay an angle 180 of where prey is looking
        Call rest of pack(mostly the senior member)
        Alpha stays at 180 degrees of prey
        Other members stays around, never in sight of vision
        if  alpha sees a potential hunt, he calls the rest of the wolf pack
        else    retreat
    if wolf is in line of sight of prey, mantain distance and stalk prey


    */

    public void Update()
    {
        RayCast();
        Hunt();
    }

    public void RayCast()
    {
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().RayCasting();
        }
    }

    public void Hunt()
    {
        //A STEP TO STOP ALL WOLVES MOVEMENTS
        //First hunt
        for (int i = 0; i < _pack.Length; i++)
        {
            if (_pack.Length - i == 1)
            {
                switch (_huntingSteps)
                {
                    case 1:
                        //Set the position the first wolf who spotted a prey
                        if (StalkPrey())
                            _huntingSteps=7;
                        break;
                    case 3:
                        //call pack seniors 
                        // Lock first wolf to angle 180 of prey while maintaining a minimum distance of 2f
                        break;
                    case 4:
                        //adjust position for senior wolves so they also stalk the prey in the rear area of the prey line of sight
                        break;
                    case 5:
                        //alpha wolf analyses the prey and decides whether or not to produce the hunt
                        break;
                    case 6:
                        //if alpha decides to hunt
                        //call the junior members of the pack and keep stalking until the rest of the members arrive
                        break;
                    case 7:
                        //has they arrive adjust positions so all members can surround the prey without being noticed by the prey
                        if(EnCircle(_pack[i]))
                            _huntingSteps++;
                        break;
                    case 8:
                        //besiese behaviour
                        break;
                    default:
                        //Search for prey

                        //Here each wolf will look for a prey
                        if(SearchPrey(_pack[i]))
                        {
                            _huntingSteps++;
                            _searchAgent = i;
                            StopActions();
                        }
                            
                        break;
                }
            }
        }
    }

    public bool StalkPrey()
    {
        //Set the position of the first wolf who spotted a prey
        if (_pack[_searchAgent].GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(AlphaPos)))
            if (_pack[_searchAgent].GetComponent<Wolf>().LookAtTarget(Prey.transform.position))
                return true;
        
        return false;
    }

    public void StopActions()
    {
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().StopAction();
        }
    }

    public bool SearchPrey(GameObject wolf) {
        wolf.GetComponent<Wolf>().SearchPrey();

        if (wolf.GetComponent<Wolf>().PreySpotted)
        {
            Prey = wolf.GetComponent<Wolf>().Prey;
            return true;
        }
        return false;
    }

    public bool EnCircle(GameObject wolf)
    {
        var temp = 0;
        for (var i = 0; i < _pack.Length; i++)
        {
            switch (i)
            {
                case 1:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(BetaPos)))
                        temp++;
                    break;
                case 2:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(GammaPos)))
                        temp++;
                    break;
                case 3:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(Delta1Pos)))
                        temp++;
                    break;
                case 4:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(Delta2Pos)))
                        temp++;
                    break;
                case 5:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(Delta3Pos)))
                        temp++;
                    break;
                case 6:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(Delta43Pos)))
                        temp++;
                    break;
                default:
                    if (wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(AlphaPos)))
                        temp++;
                    break;
            }

        }
        return temp == _pack.Length;
    }

    public float GetDistance(Vector3 origin, Vector3 rayDir, Vector3 avoidanceArea)
    {
        float distance = Vector3.Distance(origin, avoidanceArea);
        float angle = Vector3.Angle(rayDir, avoidanceArea - origin);
        return (distance * Mathf.Sin(angle * Mathf.Deg2Rad));
    }
}