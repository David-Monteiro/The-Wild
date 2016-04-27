using System.Linq;
using UnityEngine;

public class Simulator : MonoBehaviour {
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
        //GetComponent<Pack>().Update();
    }

    
}
