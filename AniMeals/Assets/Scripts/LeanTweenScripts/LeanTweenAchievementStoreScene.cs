using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenAchievementStoreScene : MonoBehaviour
{
    Vector3 offScreenLeftPosition;
    Vector3 offScreenRightPosition;
    Vector3 centerPosition;
    RectTransform rt;
    public RectTransform box1;
    
    void Awake()
    {      
        rt = GetComponent<RectTransform>();
    
        centerPosition = new Vector3(0, 0, 0);
        offScreenLeftPosition = new Vector3(0, -Screen.height, 0);
        offScreenRightPosition = new Vector3(0, Screen.height, 0);
    
        rt.localPosition = offScreenLeftPosition;
    }
    
    public void Start()
    {
        LeanTween.cancel(gameObject);      
        rt.localPosition = offScreenLeftPosition;
        LeanTween.move(rt, centerPosition, 0.3f).setEase(LeanTweenType.easeInOutExpo); 
        LeanTween.move(box1, centerPosition, 0.3f).setEase(LeanTweenType.easeInOutExpo);      

    }
}
