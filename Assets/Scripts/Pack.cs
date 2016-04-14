using UnityEngine;
using System.Collections;

public class Pack : MonoBehaviour {

    //This class serves as the controller of all my deployed wolves

    // public Vector2 vector1;
    //public Vector2 vector2;
    // public Vector3 vector3;
    //public Vector3 vector4;

    private GameObject Alpha;
    private GameObject Beta;
    private GameObject Gamma;
    private GameObject Delta1;
    private GameObject Delta2;
    private GameObject Delta3;

    private GameObject Prey;

    //Instead of an array of gameObjects, make a list of wolfs and their status in the pack.

    private int _searchAgent;
    //this int will tell which wolf is the one that fount the prey


    public int _huntingSteps;
    private GameObject [] _pack;
    public void Start ()
    {
        _huntingSteps = 0;
        _pack = GameObject.FindGameObjectsWithTag("Wolf");
        /*  foreach(GameObject wolf in pack) {

              wolf.GetComponent<Wolf>().Start();

          }*/

        for (int i = 0; i < _pack.Length; i++)
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
    public void Update () {
     /*   foreach(GameObject wolf in pack) {

        }*/
   
         
        for (int i = 0; i < _pack.Length; i++) {
            if (_pack.Length - i == 1)
            {
                _pack[i].GetComponent<Wolf>().RayCasting();
                Hunt(_pack[i]);
            }
            
        }
    }

    public void Hunt(GameObject wolf){
        //First hunt

        switch (_huntingSteps) {    
            case 1:
                //Set the position the first wolf who spotted a prey
                if(wolf.GetComponent<Wolf>().GoToLocation(Prey.transform.position))
                    _huntingSteps++;
                break;
            case 2:
                //call pack seniors
                // Lock first wolf to angle 180 of prey while maintaining a minimum distance of 2f
                break;
            case 3:
                //adjust position for senior wolves so they also stalk the prey in the rear area of the prey line of sight
                break;
            case 4:
                //alpha wolf analyses the prey and decides whether or not to produce the hunt
                break;
            case 5:
                //if alpha decides to hunt
                //call the junior members of the pack and keep stalking until the rest of the members arrive
                break;
            case 8:
                //has they arrive adjust positions so all members can surround the prey without being noticed by the prey
                break;
            case 7:
                //hunt the prey
                break;
            default:
                //Search for prey
                wolf.GetComponent<Wolf>().SearchPrey();
                if (wolf.GetComponent<Wolf>().PreySpotted)
                {
                    Prey = wolf.GetComponent<Wolf>().Prey;
                    wolf.GetComponent<Wolf>().StopAction();
                    _huntingSteps++;
                }
                break;
        }
    }
}
