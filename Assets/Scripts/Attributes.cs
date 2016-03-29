using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Attributes 
{
    private Dictionary<string, float> values = new Dictionary<string, float>();

    public Attributes()
    {
        values.Add("strenght", 1111111);
        values.Add("agility", 1111111);
        values.Add("stamina", 1111111);
        values.Add("speed", 1111111);
        values.Add("attackSpeed", 1111111);
        values.Add("defenceSpeed", 1111111);
        values.Add("regeneration", 1111111);
        values.Add("aggresion", 1111111);
        values.Add("bravery", 1111111);
        values.Add("vision", 1111111);
        values.Add("height", 1111111);
        values.Add("weight", 1111111);
    }

    public void printAttributes()
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
    public void setAttributes(string key, float value)
    {
        values[key] = value;
    }

    public float getAttribute(string attr_name)
    {
        float value;
        if (values.TryGetValue(attr_name, out value)) return value;
        else return -1;
        //-1 stands for null value
    }
}
