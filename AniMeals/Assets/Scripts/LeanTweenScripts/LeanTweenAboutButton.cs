using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenAboutButton : MonoBehaviour
{
    // public Transform rt;
    // Start is called before the first frame update
    void Start()
    {
        // rt = GetComponent<RectTransform>;
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one; 

        transform.LeanMoveLocalX(340, 0.8f).setEaseOutBack();
        // LeanTween.scale(gameObject, Vector3.one * 2, 3f).setEasePunch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
