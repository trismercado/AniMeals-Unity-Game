using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenProfileScene : MonoBehaviour
{
    Vector3 offScreenLeftPosition;
    Vector3 offScreenRightPosition;
    Vector3 centerPosition;
    RectTransform rt;
    
    void Awake()
    {      
        rt = GetComponent<RectTransform>();
    
        centerPosition = new Vector3(0, 0, 0);
        offScreenLeftPosition = new Vector3(-Screen.width, 0, 0);
        offScreenRightPosition = new Vector3(Screen.width, 0, 0);
    
        rt.localPosition = offScreenLeftPosition;
    }
    
    public void Start()
    {
        LeanTween.cancel(gameObject);      
        rt.localPosition = offScreenLeftPosition;
        LeanTween.move(rt, centerPosition, 0.3f).setEase(LeanTweenType.easeInOutExpo); 
          


    }

    // public void GotoScene(int SceneName)
    // {
    //     StartCoroutine(PlayTransition(SceneName));
    // }
 
    // IEnumerator PlayTransition(int sceneName)
    // {
    //     // LeanTween.cancel(gameObject);
    //     LeanTween.move(box1, offScreenLeftPosition*3, 0.3f).setEase(LeanTweenType.easeInOutExpo);      
    //     LeanTween.move(box2, offScreenLeftPosition*3, 0.3f).setEase(LeanTweenType.easeInOutExpo); 
    //     LeanTween.move(rt, offScreenLeftPosition*3, 0.3f).setEase(LeanTweenType.easeInOutExpo);
    //     yield return new WaitForSeconds(0.18f);
    //     SceneManager.LoadScene(sceneName);
    // }
}
