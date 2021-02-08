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
        

        if (PlayerPrefs.GetInt("isRegisteredKeyName") == 1) {
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
        Debug.Log("Register was clicked!"); 
        // LevelLoader loader =        
        
        SceneManager.LoadScene(1);
    }

    public void OnAboutClick()
    {
        Debug.Log("About was clicked");
        SceneManager.LoadScene(2);
    }
}
