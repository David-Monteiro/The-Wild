using UnityEngine;
using System.Collections;
using System.Linq;

public class Pack : MonoBehaviour
{

    //This class serves as the controller of all my deployed wolves

    public Vector3 searchArea;

    readonly Vector3[] StalkPos =
    {
        new Vector3(-2, -0.75f, 0), //
        new Vector3(1, -1.25f, 0), //
        new Vector3(-1, -1.25f, 0),
        new Vector3(1, -1.75f, 0),
        new Vector3(-1, -1.75f, 0),
        new Vector3(0, -2, 0)
    };

    public GameObject Prey;

    //Instead of an array of gameObjects, make a list of wolfs and their status in the pack.

    private int _searchAgent;
    //this int will tell which wolf is the one that fount the prey


    public int _huntingSteps;
    private GameObject[] _pack;

    public void Start()
    {
        _huntingSteps = 0;
        _pack = GameObject.FindGameObjectsWithTag("Wolf");



        foreach (var wolf in _pack) {

            //wolf.GetComponent<Wolf>().Start();

        }

       /* for (var i = 0; i < _pack.Length; i++)
        {
            if (_pack.Length - i == 1)
            {
                _pack[i].GetComponent<Wolf>().Start();
            }
        }*/


    }


    public void Update()
    {
       // RayCast();
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
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }
                    
                if (StalkPrey()) {
                    _huntingSteps++;
                }
                
                break;
            case 2:
                //call pack seniors 
                // Lock first wolf to angle 180 of prey while maintaining a minimum distance of 2f
                Debug.Log("Calling Seniors");
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }
                if (CallSeniors()) {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Seniors where called");
                }
                break;
            case 3:
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }
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
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }
                if (CallJuniors())
                {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Juniors where called");
                }
                break;
            case 6:
                //has they arrive adjust positions so all members can surround the prey without being noticed by the prey
                if (!PreyInSight())
                {
                    _huntingSteps++;
                    break;
                }
                if (Besiese())
                {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Pack is looking at target");
                }
                break;
            case 7:
                //besiese behaviour
                if (Feed())
                {
                    break;
                }
                break;
            case 8:
                if (GoNearLocation(searchArea))
                {
                    StopActions();
                    _huntingSteps = 0;
                }
                break;
            default:
                //Search for prey

                //Here each wolf will look for a prey
                if(SearchPrey())
                {
                    _huntingSteps++;
                    StopActions();
                    Debug.Log("Prey found");
                    StartCoroutine(PreyTimer());
                    break;
                }
                if (CanSmellPrey())
                {
                    _huntingSteps = 8;
                    StopActions();
                }
                break;
        }
    }

    public bool Feed()
    {

        var count =  new int[_pack.Length] ;

        for (var i = 0; i < count.Length; i++)
        {
            if (_pack[i].GetComponent<Wolf>().GetFood())
                count[i] = 1;
        }
        return count.All(i => i != 0);
    }

    public bool Besiese()
    {
        if ((Prey == null)) return true;
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().Attack(Prey.transform.position);
        }
        return false;
    }

    public bool StalkPrey()
    {
        /*foreach (var animal in _pack)
        {
            //Set the position of the first wolf who spotted a prey
            if (animal.Equals(_pack[_searchAgent]))
            {*/
                if (_pack[_searchAgent].GetComponent<Wolf>().GoToUnseen(Prey.transform.TransformPoint(StalkPos[StalkPos.Length-1])))
                    if (_pack[_searchAgent].GetComponent<Wolf>().LookAtTarget(Prey.transform.position))
                        return true;
            /*}
            else
            {
                animal.GetComponent<Wolf>().RandomMov1();
            }
        }*/
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
        var count = 0;
        for (var i = 1; i < 4; i++)
        {
            if (_pack[i-1].GetComponent<Wolf>().GoToUnseen(Prey.transform.TransformPoint(StalkPos[_pack.Length - i])))
                count++;
        }
        return count >= 3;
    }

    public void StopActions()
    {
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().StopAction();
        }
    }

    public bool SearchPrey() {

        for (var i = 0; i < 3; i++)
        {
            _pack[i].GetComponent<Wolf>().SearchPrey();
            if (_pack[i].GetComponent<Wolf>().PreySpotted)
            {
                Prey = _pack[i].GetComponent<Wolf>().Prey;
                SetPrey();
                _searchAgent = i;
                return true;
            }
        }
        return false;

    }

    public bool CallJuniors()
    {
        var count = 0;
        for (var i = 0; i < _pack.Length; i++)
        {
            if (_pack[i].GetComponent<Wolf>().GoToUnseen(Prey.transform.TransformPoint(StalkPos[i])))
                count++;
        }
        return count >= _pack.Length-2;
    }

    public float GetDistance(Vector3 origin, Vector3 rayDir, Vector3 avoidanceArea)
    {
        float distance = Vector3.Distance(origin, avoidanceArea);
        float angle = Vector3.Angle(rayDir, avoidanceArea - origin);
        return (distance * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    protected bool PreyInSight() {
        if (Prey != null) return true;

        foreach (GameObject animal in _pack)
        {
            if (animal.GetComponent<Wolf>().PreySpotted)
            {
                Prey = animal.GetComponent<Wolf>().Prey;
                SetPrey();
                return true;
            }
        }
        return false;
    }

    protected void SetPrey()
    {
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().Prey = Prey;
        }
    }

    protected bool GoNearLocation(Vector3 pos)
    {
        var count = new int[3];
        for (var i = 0; i < 3; i++)
        {
            _pack[i].GetComponent<Wolf>().GoToLocation(pos);
            if (Vector3.Distance(_pack[i].GetComponent<Wolf>().transform.position, pos) < 1)
                    count[i] = 1;
        }
        return count.All(i => i != 0);
    }

    protected bool CanSmellPrey()
    {
        foreach (var animal in _pack)
        {
            if (animal.GetComponent<Wolf>().canSmellPrey())
            {
                searchArea = animal.transform.position;
                return true;
            }
        }
        return false;
    }

    IEnumerator PreyTimer()
    {
        yield return new WaitForSeconds(20);
        Prey = null;
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().Prey = null;
            animal.GetComponent<Wolf>().StopAction();
        }
    }
}