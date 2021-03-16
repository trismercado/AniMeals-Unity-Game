using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenFoodLogScene : MonoBehaviour
{
    Vector3 offScreenLeftPosition;
    Vector3 offScreenRightPosition;
    Vector3 centerPosition;
    RectTransform rt;
    public RectTransform rt1;
    
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
        LeanTween.move(rt1, centerPosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);      
        LeanTween.move(rt, centerPosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);      
    }
}
