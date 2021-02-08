using System;
using System.Collections.Generic;

[System.Serializable]
public class DailyIntake
{
    public List<FoodLog> foodLogsForTheDay; 
    // = new List<FoodLog>{new FoodLog()};
    public string dateLogged;
    public float TEA;
    public float CHO;
    public float PRO;
    public float FAT;
    public float TEAIntake; 
    public float CHOIntake;
    public float PROIntake;
    public float FATIntake;
    
    public DailyIntake(List<FoodLog> log, string day, float t, float c, float p, float f, float TI, float CI, float PI, float FI) {
        foodLogsForTheDay = new List<FoodLog>(log);
        dateLogged = day;
        TEA = t;
        CHO = c;
        PRO = p;
        FAT = f;
        TEAIntake = TI; 
        CHOIntake = CI;
        PROIntake = PI;
        FATIntake = FI;
    }
}

