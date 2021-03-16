using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FoodLogPopUp : MonoBehaviour
{
    public GameObject popUp1;
    public Text popUpText1;
    public Text popUpTitleText1;
    public Button button1;
    public Text buttonText1;

    public void CenterPopUp(string title, string text, string btntext, Color32 titlecolor, Action action) {
        popUpTitleText1.text = title;
        popUpTitleText1.color = titlecolor;
        popUpText1.text = text;
        buttonText1.text = btntext;
        popUp1.SetActive(true);

        button1.onClick.AddListener(() => {
            action();
            // popUp1.SetActive(false);
        });
    }
}
