using UnityEngine;
using System.Collections;

public class Smell : MonoBehaviour
{
    private string _parentName = "";

    protected Animator Animator;

    public void Start()
    {
        Animator = GetComponent<Animator>();
        Destroy(gameObject, 3.0f);
    }

    public void SetParentName(string name0)
    {
        _parentName = name0;
    }

    public string GetParentName()
    {
        return _parentName;
    }

}
