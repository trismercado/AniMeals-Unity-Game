using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Save {
    public float health;
    public float hunger;
    public int exp;
    public int level;
    public int coups;
    public int streak;
    public string assess;
    public string pedromsg1;
    public string pedromsg2;
    public List<string> achieve;
    // public List<Items> inventory;
    public List<FoodLog> foodIntake;
    public List<DailyIntake> dailyFoodIntake;
    // current skin the pet is using

    //Computations
    public float TEAIntake;
    public float CHOIntake;
    public float PROIntake;
    public float FATIntake;
}