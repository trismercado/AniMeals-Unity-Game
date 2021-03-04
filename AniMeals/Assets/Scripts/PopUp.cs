using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class PopUp : MonoBehaviour
{   
    
    public GameObject popUp;
    public Text popUpText;
    public Text popUpTitleText;
    public Button button;
    public Text buttonText;

    public GameObject popUp1;
    public Text popUpText1;
    public Text popUpTitleText1;
    public Button button1;
    public Text buttonText1;

    public GameObject popUp2;
    public Text popUpText2;
    public Text popUpTitleText2;
    public Button button2;
    public Text buttonText2;

    public void PopUpTrue(string title, string text, string btntext, Color32 titlecolor, Action action) {
        popUpTitleText.text = title;
        popUpTitleText.color = titlecolor;
        popUpText.text = text;
        buttonText.text = btntext;
        popUp.SetActive(true);

        button.onClick.AddListener(() => {
            action();
            popUp.SetActive(false);
        });
    }

    public void CenterPopUp(string title, string text, string btntext, Color32 titlecolor, Action action) {
        popUpTitleText1.text = title;
        popUpTitleText1.color = titlecolor;
        popUpText1.text = text;
        buttonText1.text = btntext;
        popUp1.SetActive(true);

        button1.onClick.AddListener(() => {
            action();
            popUp1.SetActive(false);
        });
    }

    public void CenterDeadPopUp(string title, string text, string btntext, Color32 titlecolor, Action action) {
        popUpTitleText2.text = title;
        popUpTitleText2.color = titlecolor;
        popUpText2.text = text;
        buttonText2.text = btntext;
        popUp2.SetActive(true);

        button2.onClick.AddListener(() => {
            action();  
            popUp2.SetActive(false);
                      
        });
    }
}
