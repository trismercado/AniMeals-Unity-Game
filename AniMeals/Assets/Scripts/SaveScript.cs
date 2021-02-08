using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(Player))]
public class SaveScript : MonoBehaviour {
 
    private Player playerData;
    private string savePath;
 
    void Start () {
        playerData = GetComponent<Player>();
        savePath = Application.persistentDataPath + "/gamesave.anmls";
    }
 
    public void SaveData()
    {
        var save = new Save()
        {
            health = playerData.health,
            exp = playerData.exp,
            level = playerData.level,
            coups = playerData.coups,
            foodIntake = playerData.foodIntake,
            TEAIntake = playerData.TEAIntake,
            CHOIntake = playerData.CHOIntake,
            PROIntake = playerData.PROIntake,
            FATIntake = playerData.FATIntake
        };
 
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
 
        Debug.Log("Data Saved");
    }
 
    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            Save save;
 
            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            playerData.health = save.health;
            playerData.exp = save.exp;
            playerData.level = save.level;
            playerData.coups = save.coups;
            playerData.foodIntake = save.foodIntake;
            playerData.TEAIntake = save.TEAIntake;
            playerData.CHOIntake = save.CHOIntake;
            playerData.PROIntake = save.PROIntake;
            playerData.FATIntake = save.FATIntake;

 
            Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save file doesn't exist.");
        }
    }
}