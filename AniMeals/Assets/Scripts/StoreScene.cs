using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreScene : MonoBehaviour
{
    private GameObject go;
    private Player gs;

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

        // CheckIfPurchased();
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

    public void CheckIfPurchased(){
        // if (gs.achieve.Count != 0) {
        //     foreach (string i in gs.achieve) {
        //         if (i.Equals("streak3")) {
        //             streak3.SetActive(true);
        //             // continue;
        //         } else if (i.Equals("streak7")) {
        //             streak7.SetActive(true);
        //             // continue;
        //         }
        //     }
            
        // } else {
        //     streak3.SetActive(false);
        //     streak7.SetActive(false);
        // }
    }
}
