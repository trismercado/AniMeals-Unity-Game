using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenGameScene : MonoBehaviour
{
    Vector3 offScreenLeftPosition;
    Vector3 offScreenRightPosition;
    Vector3 centerPosition;
    RectTransform rt;
    
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
        LeanTween.move(rt, centerPosition, 0.5f).setEase(LeanTweenType.easeInOutQuad);      
    }
    
    // public void hide(Action onHidden)
    // {
    //     LeanTween.cancel(gameObject);
    //     LeanTween.move(rt, offScreenRightPosition, 0.3f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(delegate() { onHidden(); });
    // }
}
