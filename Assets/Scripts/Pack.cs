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

    private readonly string SENIOR_AGE = "Senior";
    private readonly string JUNIOR_AGE = "Junior";

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
        SetPackHierarchy();

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
        switch (_huntingSteps)
        {
            case 1:
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }

                if (StalkPrey())
                {
                    _huntingSteps++;
                }

                break;
            case 2:
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }
                if (CallSeniors())
                {
                    _huntingSteps++;
                    StopActions();
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
                }
                break;
            case 4:
                break;
            case 5:
                if (!PreyInSight())
                {
                    _huntingSteps = 0;
                    break;
                }
                if (CallJuniors())
                {
                    _huntingSteps++;
                    StopActions();
                }
                break;
            case 6:
                if (!PreyInSight())
                {
                    _huntingSteps++;
                    break;
                }
                if (Besiese())
                {
                    _huntingSteps++;
                    StopActions();
                }
                break;
            case 7:
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
                if (SearchPrey())
                {
                    _huntingSteps++;
                    StopActions();
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

        var count = new int[_pack.Length];

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
        if (
            _pack[_searchAgent].GetComponent<Wolf>()
                .GoToUnseen(Prey.transform.TransformPoint(StalkPos[StalkPos.Length - 1])))
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
            if (_pack[i - 1].GetComponent<Wolf>().GoToUnseen(Prey.transform.TransformPoint(StalkPos[_pack.Length - i])))
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

    public bool SearchPrey()
    {

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
        return count >= _pack.Length - 2;
    }

    public float GetDistance(Vector3 origin, Vector3 rayDir, Vector3 avoidanceArea)
    {
        float distance = Vector3.Distance(origin, avoidanceArea);
        float angle = Vector3.Angle(rayDir, avoidanceArea - origin);
        return (distance*Mathf.Sin(angle*Mathf.Deg2Rad));
    }

    protected bool PreyInSight()
    {
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

    protected void SetPackHierarchy()
    {
        for (var i = 0; i < _pack.Length - 1; i++)
        {
            for (var j = i + 1; j < _pack.Length; j++)
            {

                if (_pack[i].GetComponent<Wolf>().GetAge().Equals(JUNIOR_AGE)
                    && _pack[j].GetComponent<Wolf>().GetAge().Equals(SENIOR_AGE))
                {
                    var temp = _pack[i];
                    _pack[i] = _pack[j];
                    _pack[j] = temp;
                    break;
                }
            }
        }
        for (var i = 0; i < 2; i++)
        {
            for (var j = i + 1; j < 3; j++)
            {
                if (_pack[i].GetComponent<Wolf>().GetFitnessLevel()
                    < _pack[j].GetComponent<Wolf>().GetFitnessLevel())
                {
                    var temp = _pack[i];
                    _pack[i] = _pack[j];
                    _pack[j] = temp;
                    break;
                }
            }
        }
    }
}