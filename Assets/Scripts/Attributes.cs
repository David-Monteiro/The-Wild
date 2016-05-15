using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Attributes 
{
    public float CurrentThirst;
    public float CurrentHunger;
    public float CurrentHealth;

    public float Strenght;
    public float Agility;
    public float Speed;
    public float AttackSpeed;
    public float DefenceSpeed;
    public float Aggresion;
    public float Vision;
    public float Height;
    public float Weight;

    public readonly float HungerSpeed = .1f;
    public readonly float ThirstSpeed = .1f;

    public Attributes()
    {
        Strenght = Random.Range(1, 11);
        Agility = Random.Range(25, 35);//rotation from 25 to 35
        Speed = Random.Range(2f, 4f);//movement from 2 to 4
        AttackSpeed = Random.Range(1, 11);
        DefenceSpeed = Random.Range(1, 11);
        Aggresion = Random.Range(1, 11);
        Vision = Random.Range(0.5f, 1f);//raycast, from 0.5 to 1
        Height = Random.Range(1500, 300);
        Weight = Random.Range(20, 40);

        CurrentThirst = Random.Range(0, 100);
        CurrentHunger = Random.Range(0, 100);
        CurrentHealth = 100;
    }   

    public void PrintAttributes()
    {
        /*Console.WriteLine("strenght: " + values["strenght"]);
        Console.WriteLine("agility: " + values["agility"]);
        Console.WriteLine("stamina: " + values["stamina"]);
        Console.WriteLine("speed: " + values["strenspeedght"]);
        Console.WriteLine("attackSpeed: " + values["attackSpeed"]);
        Console.WriteLine("defenceSpeed: " + values["defenceSpeed"]);
        Console.WriteLine("regeneration: " + values["regeneration"]);
        Console.WriteLine("aggresion: " + values["aggresion"]);
        Console.WriteLine("bravery: " + values["bravery"]);
        Console.WriteLine("vision: " + values["vision"]);
        Console.WriteLine("height: " + values["height"]);
        Console.WriteLine("weight: " + values["weight"]);

        Console.WriteLine("health:" + values["health"]);
        Console.WriteLine("hunger:" + values["hunger"]);
        Console.WriteLine("thirst:" + values["thirst"]);*/
    }

    public void SetAttributes(int[] attributesValue)
    {
        for (var i = 0; i < attributesValue.Length; i++)
        {
            switch (i)
            {
                case 1:
                    Agility = attributesValue[i];
                    break;
                case 2:
                    Speed = attributesValue[i];
                    break;
                case 3:
                    AttackSpeed = attributesValue[i];
                    break;
                case 4:
                    DefenceSpeed = attributesValue[i];
                    break;
                case 5:
                    Aggresion = attributesValue[i];
                    break;
                case 6:
                    Vision = attributesValue[i];
                    break;
                case 7:
                    Height = attributesValue[i];
                    break;
                case 8:
                    Weight = attributesValue[i];
                    break;
                default:
                    Strenght = attributesValue[i];
                    break;
            }
            if (i != attributesValue.Length - 1) continue;
            CurrentThirst = Random.Range(0, 100);
            CurrentHunger = Random.Range(0, 100);
            CurrentHealth = 100;
        }
    }



    public void SetAttributes()
    {

    }

    /*public float GetAttribute(string attr_name)
    {
        float value;
        if (values.TryGetValue(attr_name, out value)) return value;
        return -1;
        //-1 stands for null value
    }*/
}
