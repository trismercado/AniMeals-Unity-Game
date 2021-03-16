using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class AchievementPopUp : MonoBehaviour
{   
    
    public GameObject popUp;
    public Text popUpText;
    public Text popUpTitleText;
    public Button button;
    public Text buttonText;

    public void PopUpTrue(string title, string text, string btntext, Color32 titlecolor, Action action) {
        popUpTitleText.text = title;
        popUpTitleText.color = titlecolor;
        popUpText.text = text;
        buttonText.text = btntext;
        popUp.SetActive(true);

        button.onClick.AddListener(() => {
            action();
            // popUp.SetActive(false);
        });
    }
}
