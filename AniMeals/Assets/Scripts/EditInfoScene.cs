using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditInfoScene : MonoBehaviour
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

    public Button saveBtn;

    private GameObject go;
    private Player gs;

    void Start()
    {
        go = GameObject.Find("Player");
        if (go == null) {
            Debug.LogError("No player gameobject");
            this.enabled = false;
            return;
        }
        gs = go.GetComponent<Player>();

        name = PlayerPrefs.GetString("nameKeyName");
        inputName.text = name;
        weight = PlayerPrefs.GetString("weightKeyName");
        inputWeight.text = weight;
        height = PlayerPrefs.GetString("heightKeyName");
        inputHeight.text = height;

        // var t = PlayerPrefs.GetString("activityLevelKeyName", activityLevel);
        drop.value = int.Parse(PlayerPrefs.GetString("activityLevelKeyName", activityLevel)) - 1;
    }

    // Update is called once per frame
    void Update()
    {
        name = inputName.text;
        weight = inputWeight.text;
        height = inputHeight.text;
        
        saveBtn.interactable = CheckEntries();
    }

    public void OnBackClick() {
        SceneManager.LoadScene(5);
    }

    public void Q4DropDown(int val) {
        val = val + 1;
        activityLevel = val.ToString();

    }

    public void OnSaveClick()
    {
        // Save Info
        // Debug.Log("Save was clicked!");

        //Saving Info
        if (CheckEntries()) {
            PlayerPrefs.SetString("nameKeyName", name);
            PlayerPrefs.SetString("weightKeyName", weight);
            PlayerPrefs.SetString("heightKeyName", height);
            PlayerPrefs.SetString("activityLevelKeyName", activityLevel);

            //Register once only
            gs.Computations();

            SceneManager.LoadScene(5);
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
