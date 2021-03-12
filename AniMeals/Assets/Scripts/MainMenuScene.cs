using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuScene : MonoBehaviour
{
    // public GameObject 
    private string savePath;


    void Awake(){
        

        if (PlayerPrefs.HasKey("isRegisteredKeyName")) {
            SceneManager.LoadScene(3);
        } else {
            savePath = Application.persistentDataPath + "/gamesave.anmls";
            File.Delete(savePath);
            Debug.Log("Saved Game deleted");

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegisterClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnAboutClick()
    {
        SceneManager.LoadScene(2);
    }
}
