using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class RegisterScene : MonoBehaviour
{
    
    public GameObject[] questions;
    public Button saveBtn;
    public Button prevBtn;
    public Button nxtBtn;
    public InputField inputName;
    new string name;

    public InputField inputWeight;
    string weight;

    public InputField inputHeight;
    string height;

    // public InputField inputActivityLevel;
    string activityLevel;

    int index;

    public Dropdown drop;
    public Image pageCounter1;
    public Image pageCounter2;
    public Image pageCounter3;
    public Image pageCounter4;

    Color32 currentColor = new Color32(0x00, 0x00, 0x00, 0xFF);
    Color32 notCurrentColor = new Color32(0x93, 0x93, 0x93, 0xFF);


    // Start is called before the first frame update
    void Start()
    {
        

        drop.value = 0;
        index = 0;
        DisplayQs();
    } 

    // Update is called once per frame
    void Update()
    {
        name = inputName.text;
        weight = inputWeight.text;
        height = inputHeight.text;

        saveBtn.interactable = CheckEntries();
        
        if (index == 0) {
            pageCounter1.color = currentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
            nxtBtn.gameObject.SetActive(true);
            prevBtn.gameObject.SetActive(false);
        } else if (index == 1) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = currentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
            nxtBtn.gameObject.SetActive(true);
            prevBtn.gameObject.SetActive(true);
        } else if (index == 2) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = currentColor;
            pageCounter4.color = notCurrentColor;
            nxtBtn.gameObject.SetActive(true);
            prevBtn.gameObject.SetActive(true);
        } else if (index == 3) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = currentColor;
            nxtBtn.gameObject.SetActive(false);
            prevBtn.gameObject.SetActive(true);
        } 

    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(0);

    }

    public void Q4DropDown(int val) {
        val = val + 1;
        activityLevel = val.ToString();

    }

    public void OnSaveClick()
    {
        //Saving Info
        if (CheckEntries()) {
            PlayerPrefs.SetString("nameKeyName", name);
            PlayerPrefs.SetString("weightKeyName", weight);
            PlayerPrefs.SetString("heightKeyName", height);
            PlayerPrefs.SetString("activityLevelKeyName", activityLevel);

            //Register once only
            PlayerPrefs.SetString("isRegisteredKeyName", DateTime.Today.ToString());

            SceneManager.LoadScene(11);
        }       

    }

    public bool CheckEntries() {
        if (weight != "" && height != "" && name != "") {
            if (float.Parse(weight) > 0f && float.Parse(height) > 0f && name != "" && int.Parse(activityLevel) > 0 && int.Parse(activityLevel) < 6) {
                return true;
            } else  {
                return false;
            }
        } else {
            return false;
        }
        
    }

    public void OnNextClick()
    {
        index++;
        if (index < 4) {
            DisplayQs();
        } else {
            index = 3;
        }

    }

    public void OnPrevClick()
    {
        index--;
        if (index >= 0) {
            DisplayQs();
        } else {
            index = 0;
        }

    }

    public void DisplayQs() {
        for (int i=0; i < 4; i++) {
            if (i == index) {
                questions[i].SetActive(true);
            } else {
                questions[i].SetActive(false);
            }
            

        }
    }


}
