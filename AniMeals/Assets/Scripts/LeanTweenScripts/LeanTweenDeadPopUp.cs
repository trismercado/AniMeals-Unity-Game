using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenDeadPopUp : MonoBehaviour
{
    public GameObject box;
    // Start is called before the first frame update
    private void OnEnable()
    {
        LeanTween.cancel(box);

        transform.localScale = Vector3.one; 

        LeanTween.scale(box, Vector3.one * 2, 3f).setEasePunch();
    }
}
