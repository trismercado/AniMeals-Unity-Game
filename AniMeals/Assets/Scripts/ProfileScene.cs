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

    public Text actLevelDesc;
    public Text dBWDesc;

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
        weightText.text = PlayerPrefs.GetString("weightKeyName") + "kg";
        heightText.text = PlayerPrefs.GetString("heightKeyName") + "cm";
        DBWText.text = PlayerPrefs.GetString("DBWKeyName") + "kg";
        activityLevel = PlayerPrefs.GetString("activityLevelKeyName");

        if (float.Parse(activityLevel) == 1) {
            actLevelText.text = "Bed Rest But Mobile";
            actLevelDesc.text = "You are usually in bed.";
        } else if (float.Parse(activityLevel) == 2) {
            actLevelText.text = "Sedentary";
            actLevelDesc.text = "You spend most of your day sitting.";
        } else if (float.Parse(activityLevel) == 3) {
            actLevelText.text = "Light";
            actLevelDesc.text = "You usually spend a large part of your day on your feet.";
        } else if (float.Parse(activityLevel) == 4) {
            actLevelText.text = "Moderate";
            actLevelDesc.text = "You spend a good part of the day doing some physical activity";
        } else if (float.Parse(activityLevel) == 5) {
            actLevelText.text = "Very Active";
            actLevelDesc.text = "You spend most of the day doing heavy physical activity";
        }

        Display();
    }


    public void OnBackClick()
    {
        SceneManager.LoadScene(3);

    }

    public void OnEditClick()
    {
        SceneManager.LoadScene(10);

    }

    public void Display() {
        float weight = float.Parse(PlayerPrefs.GetString("weightKeyName"));
        float dbw = float.Parse(PlayerPrefs.GetString("DBWKeyName"));
        if (weight < (dbw-(0.1f*dbw))) {
            dBWDesc.text = "Your weight is below your ideal weight range.";
        } else if (weight > (dbw+(0.1f*dbw))) {
            dBWDesc.text = "Your weight is above your ideal weight range.";
        } else {
            dBWDesc.text = "Your weight is around your ideal weight range.";
        }

    }
}
