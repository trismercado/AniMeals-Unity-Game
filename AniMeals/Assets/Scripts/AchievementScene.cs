using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementScene : MonoBehaviour
{
    private GameObject go;
    private Player gs;
    
    public GameObject streak3;
    public GameObject streak7;

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("Player");
        if (go == null) {
            Debug.LogError("No player gameobject");
            this.enabled = false;
            return;
        }
        gs = go.GetComponent<Player>();

        CheckIfAchieved();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackClick()
    {
        Debug.Log("Back was clicked!");
        SceneManager.LoadScene(3);

    }

    public void CheckIfAchieved(){
        if (gs.achieve.Count != 0) {
            foreach (string i in gs.achieve) {
                if (i.Equals("streak3")) {
                    streak3.SetActive(true);
                } else if (i.Equals("streak7")) {
                    streak7.SetActive(true);
                }
            }
            
        } else {
            streak3.SetActive(false);
            streak7.SetActive(false);
        }
            
            
    }
}
