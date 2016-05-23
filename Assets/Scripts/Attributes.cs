using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using Random = UnityEngine.Random;

public class Attributes 
{
    private readonly string WOLF = "Wolf";
    private readonly string BEAR = "Bear";
    private readonly string MOOSE = "Moose";

    public bool DisplayAttributes;

    public float CurrentThirst;
    public float CurrentHunger;
    public float CurrentHealth;

    public float Strenght;
    public float Agility;
    public float Speed;
    public float AttackSpeed;
    public float DefenceSpeed;
    public float Aggression;
    public float Vision;
    public float Height;
    public float Weight;

    public readonly float RegenerationSpeed;
    public readonly float HungerSpeed;
    public readonly float ThirstSpeed;

    public Attributes()
    {
        DisplayAttributes = false;

        HungerSpeed = .1f;
        ThirstSpeed = .1f;
        RegenerationSpeed = 0.09f;
    }   

    /*public void OnGUI()
    {
        /*Console.WriteLine("strenght: " + values["strenght"]);
        Console.WriteLine("agility: " + values["agility"]);
        Console.WriteLine("stamina: " + values["stamina"]);
        Console.WriteLine("speed: " + values["strenspeedght"]);
        Console.WriteLine("attackSpeed: " + values["attackSpeed"]);
        Console.WriteLine("defenceSpeed: " + values["defenceSpeed"]);
        Console.WriteLine("regeneration: " + values["regeneration"]);
        Console.WriteLine("aggression: " + values["aggresion"]);
        Console.WriteLine("bravery: " + values["bravery"]);
        Console.WriteLine("vision: " + values["vision"]);
        Console.WriteLine("height: " + values["height"]);
        Console.WriteLine("weight: " + values["weight"]);

        Console.WriteLine("health:" + values["health"]);
        Console.WriteLine("hunger:" + values["hunger"]);
        Console.WriteLine("thirst:" + values["thirst"]);* /

        if (DisplayAttributes)
        { 

            var w = Screen.width;
            var h = Screen.height;
            GUI.Box(new Rect(w - 150, h - 100, 150, 100), "");

            GUI.Label(new Rect(w - 140, h - 90, 100, 30), "Health");
            GUI.Label(new Rect(w - 50, h - 90, 100, 30), CurrentHealth.ToString());
            GUI.Label(new Rect(w - 140, h - 70, 100, 30), "Hunger");
            GUI.Label(new Rect(w - 50, h - 70, 100, 30), CurrentHunger.ToString("0"));
            GUI.Label(new Rect(w - 140, h - 50, 100, 30), "Thirst");
            GUI.Label(new Rect(w - 50, h - 50, 100, 30), CurrentThirst.ToString("0"));

        }
    }*/


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
                    Aggression = attributesValue[i];
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
            CurrentThirst = Random.Range(0, 50);
            CurrentHunger = Random.Range(0, 50);
            CurrentHealth = Random.Range(50, 100);
        }
    }

    
    public void SetAttributes(string animal)
    {
        switch (animal)
        {
            case "Wolf":
                Height = Random.Range(80, 85);
                Weight = Random.Range(30, 80);
                Strenght = Random.Range(1, 11);
                Speed = Random.Range(3.25f, 3.5f); 
                Aggression = Random.Range(5, 11);
                break;
            case "Bear":
                Height = Random.Range(70, 150);
                Weight = Random.Range(100, 500);
                Strenght = Random.Range(1, 11);
                Speed = Random.Range(2.75f, 3f); 
                Aggression = Random.Range(5, 11);
                break;
            case "Moose":
                Height = Random.Range(140, 210);
                Weight = Random.Range(380, 600);
                Strenght = Random.Range(1, 11);
                Speed = Random.Range(2.5f, 2.85f);
                Aggression = Random.Range(3, 8);
                break;
            default:
                Height = Random.Range(80, 150);
                Weight = Random.Range(100, 300);
                Strenght = Random.Range(2, 7);
                Speed = Random.Range(3f, 4f);
                Aggression = Random.Range(3, 8);
                break;
        }
        Agility = Random.Range(((Weight * 35) / 600) + 23, 36);
        AttackSpeed = Random.Range(1, 21);
        DefenceSpeed = Random.Range(1, 11);
        Vision = Random.Range(0.5f, 1f); //raycast, from 0.5 to 1

        CurrentThirst = Random.Range(0, 50);
        CurrentHunger = Random.Range(0, 50);
        CurrentHealth = Random.Range(50, 100);
    }

    public float [] GetAttributes()
    {
        var attr = new float[9];

        for (var i = 0; i < attr.Length; i++)
        {
            switch (i)
            {
                case 1:
                    attr[i] = Agility;
                    break;
                case 2:
                    attr[i] = Speed;
                    break;
                case 3:
                    attr[i] = AttackSpeed;
                    break;
                case 4:
                    attr[i] = DefenceSpeed;
                    break;
                case 5:
                    attr[i] = Aggression;
                    break;
                case 6:
                    attr[i] = Vision;
                    break;
                case 7:
                    attr[i] = Height;
                    break;
                case 8:
                    attr[i] = Weight;
                    break;
                default:
                    attr[i] = Strenght;
                    break;
            }
        }
        return attr;
    }

    public float GetFitness()
    {
        return ((Height/5)*Weight)/(Strenght*Aggression);
    }

    public float GetAttackStats()
    {
        return GetFitness() * AttackSpeed;
    }

    public float GetDefenceStats()
    {
        return GetFitness() * DefenceSpeed;
    }

}
