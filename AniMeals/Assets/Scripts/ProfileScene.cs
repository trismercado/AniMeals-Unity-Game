using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileScene : MonoBehaviour
{

    // new string name;
    // string weight;
    // string height;
    string activityLevel;

    public Text playerNameText;
    public Text weightText;
    public Text heightText;
    public Text actLevelText;
    public Text DBWText;

    private Player gs;
    private GameObject go;

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

        playerNameText.text = PlayerPrefs.GetString("nameKeyName");
        weightText.text = PlayerPrefs.GetString("weightKeyName") + " kg";
        heightText.text = PlayerPrefs.GetString("heightKeyName") + " cm";
        DBWText.text = PlayerPrefs.GetString("DBWKeyName") + " kg";
        activityLevel = PlayerPrefs.GetString("activityLevelKeyName");

        if (float.Parse(activityLevel) == 1) {
            actLevelText.text = "Bed Rest But Mobile";
        } else if (float.Parse(activityLevel) == 2) {
            actLevelText.text = "Sedentary";
        } else if (float.Parse(activityLevel) == 3) {
            actLevelText.text = "Light";
        } else if (float.Parse(activityLevel) == 4) {
            actLevelText.text = "Moderate";
        } else if (float.Parse(activityLevel) == 5) {
            actLevelText.text = "Very Active";
        }
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

    public void OnEditClick()
    {
        Debug.Log("Edit was clicked!");
        // SceneManager.LoadScene(1);

    }
}
