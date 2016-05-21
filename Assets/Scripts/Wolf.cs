using UnityEngine;
using System.Collections;

public class Wolf : Carnivorous
{

    public new void Start(){
      base.Start();

        var coliiders = GameObject.FindGameObjectsWithTag("Wolf");
        foreach (var co in coliiders)
        {
            Physics2D.IgnoreCollision(co.GetComponent<UnityEngine.BoxCollider2D>(), GetComponent<UnityEngine.BoxCollider2D>());
            
        }
    }

    public new void Update()
    {
        base.Update();
        Animator.SetInteger("Animation_Rotation_State", (int) transform.rotation.eulerAngles.z);
        Animator.SetBool("Animation_Mov_State", isMoving_flag);

        ControlledMov();
        //if (cond == false)
        // cond = GetFood();

        if (canSmellPrey())
            Debug.Log("Prey smelled");
        
        //float dist = DistancePointLine(transform.position, enemy.GetComponent<Animal>().backPointC.position,
        // enemy.transform.position);

        //if (cond == false)
        //  cond = GoToUnseen(enemy.GetComponent<Animal>().backPointC.position);




        //Debug.Log(inPreyVisionArea(Prey.GetComponent<Animal>().backPointC.position));
    }




    public float GetDistance(Vector3 origin, Vector3 rayDir, Vector3 avoidanceArea)
    {
        float distance = Vector3.Distance(origin, avoidanceArea);
        float angle = Vector3.Angle(rayDir, avoidanceArea - origin);
        return (distance * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    public static float DistancePointLine(Vector3 avoidanceArea, Vector3 lineStart, Vector3 lineEnd)
    {
        return Vector3.Magnitude(ProjectPointLine(avoidanceArea, lineStart, lineEnd) - avoidanceArea);
    }

    public static Vector3 ProjectPointLine(Vector3 lineStart, Vector3 lineEnd, Vector3 avoidanceArea)
    {
        Vector3 rhs = avoidanceArea - lineStart;
        Vector3 vector2 = lineEnd - lineStart;
        float magnitude = vector2.magnitude;
        Vector3 lhs = vector2;
        if (magnitude > 1E-06f)
        {
            lhs = (Vector3)(lhs / magnitude);
        }
        float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
        return (lineStart + ((Vector3)(lhs * num2)));
    }

}
