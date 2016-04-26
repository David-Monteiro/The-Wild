using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Attributes 
{
    private readonly Dictionary<string, float> values = new Dictionary<string, float>();

    public Attributes()
    {
        values.Add("strenght", UnityEngine.Random.Range(1, 11));
        values.Add("agility", UnityEngine.Random.Range(1, 11));//rotation from 25 to 35
        values.Add("speed", UnityEngine.Random.Range(2f, 4f));//movement from 2 to 4
        values.Add("attackSpeed", UnityEngine.Random.Range(1, 11));
        values.Add("defenceSpeed", UnityEngine.Random.Range(1, 11));
        values.Add("aggresion", UnityEngine.Random.Range(1, 11));
        values.Add("vision", UnityEngine.Random.Range(0.5f, 1f));//raycast, from 0.5 to 1
        values.Add("height", UnityEngine.Random.Range(1500, 300));
        values.Add("weight", UnityEngine.Random.Range(20, 40));
    }

    public void PrintAttributes()
    {
        Console.WriteLine("strenght" + values["strenght"]);
        Console.WriteLine("agility" + values["agility"]);
        Console.WriteLine("stamina" + values["stamina"]);
        Console.WriteLine("speed" + values["strenspeedght"]);
        Console.WriteLine("attackSpeed" + values["attackSpeed"]);
        Console.WriteLine("defenceSpeed" + values["defenceSpeed"]);
        Console.WriteLine("regeneration" + values["regeneration"]);
        Console.WriteLine("aggresion" + values["aggresion"]);
        Console.WriteLine("bravery" + values["bravery"]);
        Console.WriteLine("vision" + values["vision"]);
        Console.WriteLine("height" + values["height"]);
        Console.WriteLine("weight" + values["weight"]);
    }
    public void SetAttributes(string key, float value)
    {
        values[key] = value;
    }

    public float GetAttribute(string attr_name)
    {
        float value;
        if (values.TryGetValue(attr_name, out value)) return value;
        return -1;
        //-1 stands for null value
    }
}
