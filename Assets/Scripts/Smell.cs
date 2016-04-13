using UnityEngine;
using System.Collections;

public class Smell : MonoBehaviour
{


    protected Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Object.Destroy(gameObject, 1.0f);
    }

}
