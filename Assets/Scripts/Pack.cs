using UnityEngine;
using System.Collections;
using System.Linq;

public class Pack : MonoBehaviour
{

    //This class serves as the controller of all my deployed wolves

    // public Vector2 vector1;
    //public Vector2 vector2;
    // public Vector3 vector3;
    //public Vector3 vector4;

    /*
        private Vector3 AlphaPos = new Vector3(0, -2, 0);
        private Vector3 BetaPos = new Vector3(1, -1.75f, 0);
        private Vector3 GammaPos = new Vector3(-1, -1.75f, 0);
        private Vector3 Delta1Pos = new Vector3(1, -1.25f, 0);
        private Vector3 Delta2Pos = new Vector3(-1, -1.25f, 0);
        private Vector3 Delta3Pos = new Vector3(2, -0.75f, 0);
        private Vector3 Delta4Pos = new Vector3(-2, -0.75f, 0);
    */
    Vector3[] StalkPos =
    {
        new Vector3(2, -0.75f, 0),
        new Vector3(1, -1.25f, 0),
        new Vector3(-1, -1.25f, 0),
        new Vector3(1, -1.75f, 0),
        new Vector3(-1, -1.75f, 0),
        new Vector3(0, -2, 0)
        
        
    };

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
        switch (_huntingSteps)
        {
            case 1:
                //Set the position the first wolf who spotted a prey
                if (StalkPrey()) {
                    _huntingSteps++;
                }
                break;
            case 2:
                //call pack seniors 
                // Lock first wolf to angle 180 of prey while maintaining a minimum distance of 2f
                Debug.Log("Calling Seniors");
                if (CallSeniors()) {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Call Seniors");
                }
                break;
            case 3:
                if (LookAtPrey2(2))
                {
                    _huntingSteps = 5;
                    Debug.Log("Seniors looking at prey");
                }
                //adjust position for senior wolves so they also stalk the prey in the rear area of the prey line of sight
                break;
            case 4:
                //alpha wolf analyses the prey and decides whether or not to produce the hunt
                break;
            case 5:
                //if alpha decides to hunt
                //call the junior members of the pack and keep stalking until the rest of the members arrive
                Debug.Log("Calling Juniors");
                if (CallJuniors())
                {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("CallJuniors");
                }
                break;
            case 6:
                //has they arrive adjust positions so all members can surround the prey without being noticed by the prey
                if (LookAtPrey2(1))
                {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Looking at target");
                }
                break;
            case 7:
                //besiese behaviour
                break;
            default:
                //Search for prey

                //Here each wolf will look for a prey
                if(SearchPrey())
                {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Prey found");
                }          
                break;
        }
    }

    public bool StalkPrey()
    {
        foreach (var animal in _pack)
        {
            //Set the position of the first wolf who spotted a prey
            if(!animal.Equals(_pack[_searchAgent]))
                animal.GetComponent<Wolf>().RandomMov1();
            else
                if (_pack[_searchAgent].GetComponent<Wolf>().GoToLocation(Prey.transform.TransformPoint(StalkPos[0])))
                    if (_pack[_searchAgent].GetComponent<Wolf>().LookAtTarget(Prey.transform.position))
                        return true;
        }
        return false;
    }

    public bool LookAtPrey2(int type)
    {
        
        var count = type == 1 ? new int[_pack.Length] : new int[3];

        for (var i = 0; i < count.Length; i++)
        {
            if (_pack[i].GetComponent<Wolf>().LookAtTarget(Prey.transform.position))
                count[i] = 1;
        }
        return count.All(i => i != 0);
    }

    public bool CallSeniors()
    {
        /*var count = type == 1 ? new int[3] : new int[_pack.Length];
      
        for (var i = 0; i < count.Length; i++)
        {
            if (_pack[i].GetComponent<Wolf>().LookAtTarget(Prey.transform.position))
                count[i] = 1;
        }
        return count.All(i => i != 0);*/


        var count = 0;
        for (var i = 1; i < 4; i++)
        {
            if (_pack[i].GetComponent<Wolf>().GoToUnseen(Prey.transform.TransformPoint(StalkPos[_pack.Length - i])))
                count++;
        }
        return count == 3;
    }

    public void StopActions()
    {
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().StopAction();
        }
    }

    public bool SearchPrey() {
        //for (int i = 0; i < _pack.Length; i++)
        for (int i = 0; i < 3; i++)
        {
            _pack[i].GetComponent<Wolf>().SearchPrey();

            if (_pack[i].GetComponent<Wolf>().PreySpotted)
            {
                Prey = _pack[i].GetComponent<Wolf>().Prey;
                foreach (var animal in _pack)
                {
                    animal.GetComponent<Wolf>().Prey = Prey;
                }
                _searchAgent = i;
                return true;
            }
        }
        return false;

    }

    public bool CallJuniors()
    {
        var temp = 0;
        for (var i = 0; i < _pack.Length; i++)
        {
            if (_pack[i].GetComponent<Wolf>().GoToUnseen(Prey.transform.TransformPoint(StalkPos[i])))
                temp++;
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