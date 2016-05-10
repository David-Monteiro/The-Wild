using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Attributes 
{
    private Dictionary<string, float> values = new Dictionary<string, float>();

    public Attributes()
    {
        values.Add("strenght", 0);
        values.Add("agility", 0);//rotation from 25 to 35
        values.Add("speed", 0);//movement from 2 to 4
        values.Add("attackSpeed", 0);
        values.Add("defenceSpeed", 0);
        values.Add("aggresion", 0);
        values.Add("vision", 0);//raycast, from 0.5 to 1
        values.Add("height", 0);
        values.Add("weight", 0);

        values.Add("health", 100);
        values.Add("hunger", 0);
        values.Add("thirst", 0);
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

        Console.WriteLine("health" + values["health"]);
        Console.WriteLine("hunger" + values["hunger"]);
        Console.WriteLine("thirst" + values["thirst"]);
    }
    public void SetAttributes(string key, float value)
    {
        values[key] = value;
    }

    public void SetAttributes()
    {
        values["strenght"] = Random.Range(1, 11);
        values["agility"] = Random.Range(25, 35);//rotation from 25 to 35
        values["speed"] = Random.Range(2f, 4f);//movement from 2 to 4
        values["attackSpeed"] = Random.Range(1, 11);
        values["defenceSpeed"] = Random.Range(1, 11);
        values["aggresion"] = Random.Range(1, 11);
        values["vision"] = Random.Range(0.5f, 1f);//raycast, from 0.5 to 1
        values["height"] = Random.Range(1500, 300);
        values["weight"] = Random.Range(20, 40);

        values["health"] = 100;
        values["hunger"] = 0;
        values["thirst"] = 0;
    }

    public float GetAttribute(string attr_name)
    {
        float value;
        if (values.TryGetValue(attr_name, out value)) return value;
        return -1;
        //-1 stands for null value
    }
}
