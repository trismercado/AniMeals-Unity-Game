using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public int id;
    public string title;
    public string description;
    public int price;
    public bool isBought;
    public bool isEquipped;
    //add animations that comes with

    public Skin(int id, string title, string description, int price, bool isBought, bool isEquipped)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.price = price;
        this.isBought = isBought;
        this.isEquipped = isEquipped;
    }
}

