using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutScene : MonoBehaviour
{
    public void OnBackClick()
    {
        Debug.Log("Back was clicked!");
        SceneManager.LoadScene(0);

    }
}
