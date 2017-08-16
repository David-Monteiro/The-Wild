using System.Collections;
using System.Linq;
using UnityEngine;

public class Simulator : MonoBehaviour {

    public GameObject wolfPrefab;
    public GameObject bearPrefab;
    public GameObject moosePrefab;
    public GameObject dearPrefab;

    public GameObject map;


    private readonly string SENIOR_AGE = "Senior";
    private readonly string JUNIOR_AGE = "Junior";

    private GameObject[] animals;

    void Start() {
        
    }

    private void SetPosition()
    {
        animals = GameObject.FindGameObjectsWithTag("Wolf");

        Vector3[] positions = new Vector3[animals.Length];

        for (int i = 0; i < animals.Length; i++)
        {
            positions[i] = animals[i].transform.position;
        }
        Vector3 midpoint = GetMidPoint(positions);

        float distanceFromPlane = -(GetLongestDistance(midpoint, positions) * 2 / 5);
        
        if (distanceFromPlane < -1)
            transform.position = new Vector3(midpoint.x, midpoint.y, distanceFromPlane);
        else
            transform.position = new Vector3(midpoint.x, midpoint.y, -1);
    }

    private Vector3 GetMidPoint(Vector3 [] positions)
    {
        //My cluster algorithm
        if (positions.Length == 1)
        {
            return positions[0];
        }
        int insertion = 0;
        int newSize = positions.Length % 2 == 0 ? positions.Length / 2 : (positions.Length / 2) +1;

        Vector3[] iteration = new Vector3[newSize];
        
        for (int i = 0; i < positions.Length; i= i +2)
        {
            if (i + 1 < positions.Length)
                iteration[insertion] = positions[i] + (positions[i + 1] - positions[i])/2;
            else
                iteration[insertion] = positions[i];
            insertion++;
        }
        
        return GetMidPoint(iteration);
    }

    private float GetLongestDistance(Vector3 midpoint, Vector3[] positions)
    {
        return positions.Select(position => Vector3.Distance(position, midpoint)).Concat(new[] {0f}).Max();
    }

    void Update ()
    {
        SetPosition();
    }

    void SetFirstAnimals()
    {
        for (var i = 0; i < 6; i++)
        {
            var wolf = (GameObject)Instantiate(wolfPrefab, transform.position, Quaternion.identity);
            wolf.GetComponent<Wolf>().SetAge(i < 3 ? SENIOR_AGE : JUNIOR_AGE);
        }

        var moose0 = (GameObject)Instantiate(moosePrefab, transform.position, Quaternion.identity);
        moose0.GetComponent<Wolf>().SetAge(SENIOR_AGE);

        var moose1 = (GameObject)Instantiate(moosePrefab, transform.position, Quaternion.identity);
        moose1.GetComponent<Wolf>().SetAge(SENIOR_AGE);

    }
       


    void SetAnimals()
    {
        animals = GameObject.FindGameObjectsWithTag("Wolf");

        sortWolfPack();
        var newGeneration = animals;
        for (var i = 0; i < animals.Length/2; i++)
        {
            newGeneration[i+3].GetComponent<Wolf>().SetAttributes(mutation(animals[i].GetComponent<Wolf>().GetAttributes()
                , animals[i + 3].GetComponent<Wolf>().GetAttributes()));
            newGeneration[i].GetComponent<Wolf>().SetAttributes(animals[i+3].GetComponent<Wolf>().GetAttributes());
        }
        if (Random.Range(0, 50) < 25)
        {
            var moose0 = (GameObject) Instantiate(moosePrefab, transform.position, Quaternion.identity);
            moose0.GetComponent<Wolf>().SetAge(SENIOR_AGE);

            var moose1 = (GameObject) Instantiate(moosePrefab, transform.position, Quaternion.identity);
            moose1.GetComponent<Wolf>().SetAge(SENIOR_AGE);
        }
        else
        {
            var moose = (GameObject)Instantiate(moosePrefab, transform.position, Quaternion.identity);
            moose.GetComponent<Moose>().SetAge(SENIOR_AGE);

            var bear = (GameObject)Instantiate(bearPrefab, transform.position, Quaternion.identity);
            bear.GetComponent<Bear>().SetAge(SENIOR_AGE);
        }
        
    }

    protected void sortWolfPack()
    {
        animals = GameObject.FindGameObjectsWithTag("Wolf");
        for (var i = 0; i < animals.Length-1; i++)
        {
            for (var j = i+1; j < animals.Length; j++)
            {
            
                if(animals[i].GetComponent<Wolf>().GetAge().Equals(JUNIOR_AGE)
                    && animals[j].GetComponent<Wolf>().GetAge().Equals(SENIOR_AGE))
                {
                    var temp = animals[i];
                    animals[i] = animals[j];
                    animals[j] = temp;
                    break;
                }
            }
        }

    }

    protected float [] mutation(float [] attributesFromA, float[] attributesFromB)
    {
        var mutation = new float[attributesFromA.Length];
        for (var i = 0; i < mutation.Length; i++)
        {
            mutation[i] = Random.Range(0, 50) < 25 ? attributesFromA[i] : attributesFromB[i];
        }
        return mutation;
    }
/*
    IEnumerator GenerationTimer()
    {
        yield return new WaitForSeconds(300);
        foreach (var animal in _pack)
        {
            animal.GetComponent<Wolf>().Prey = null;
            animal.GetComponent<Wolf>().StopAction();
        }
    }*/
}
