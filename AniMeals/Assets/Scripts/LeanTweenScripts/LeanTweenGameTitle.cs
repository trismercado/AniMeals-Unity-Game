using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenGameTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one; 

        transform.LeanMoveLocal(new Vector2(0, 400), 2).setEaseOutBack();
        // LeanTween.scale(gameObject, Vector3.one * 2, 3f).setEasePunch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
