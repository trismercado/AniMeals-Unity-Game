using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenMessageScene : MonoBehaviour
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
    
        rt.localPosition = offScreenRightPosition;
    }
    
    public void Start()
    {
        LeanTween.cancel(gameObject);      
        rt.localPosition = offScreenRightPosition;
        LeanTween.move(rt, centerPosition, 0.3f).setEase(LeanTweenType.easeInOutExpo);      
    }
}
