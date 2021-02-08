using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterScene : MonoBehaviour
{
    
    public InputField inputName;
    new string name;

    public InputField inputWeight;
    string weight;

    public InputField inputHeight;
    string height;

    // public InputField inputActivityLevel;
    string activityLevel;

    public Dropdown drop;


    // Start is called before the first frame update
    void Start()
    {
        name = PlayerPrefs.GetString("nameKeyName");
        inputName.text = name;
        weight = PlayerPrefs.GetString("weightKeyName");
        inputWeight.text = weight;
        height = PlayerPrefs.GetString("heightKeyName");
        inputHeight.text = height;

        drop.value = 0;
    } 

    // Update is called once per frame
    void Update()
    {
        name = inputName.text;
        weight = inputWeight.text;
        height = inputHeight.text;
    }

    public void OnBackClick()
    {
        Debug.Log("Back was clicked!");
        SceneManager.LoadScene(0);

    }

    public void Q4DropDown(int val) {
        val = val + 1;
        activityLevel = val.ToString();
        Debug.Log(val);
    }

    public void OnSaveClick()
    {
        // Save Info
        Debug.Log("Save was clicked!");

        //Saving Info
        if (CheckEntries()) {
            PlayerPrefs.SetString("nameKeyName", name);
            PlayerPrefs.SetString("weightKeyName", weight);
            PlayerPrefs.SetString("heightKeyName", height);
            PlayerPrefs.SetString("activityLevelKeyName", activityLevel);

            //Register once only
            PlayerPrefs.SetInt("isRegisteredKeyName", 1);
            SceneManager.LoadScene(3);
        }       

    }

    public bool CheckEntries() {
        if (float.Parse(weight) > 0f && float.Parse(height) > 0f && name != "" && int.Parse(activityLevel) > 0 && int.Parse(activityLevel) < 6) {
            return true;
        } else  {
            return false;
        }
    }
}
