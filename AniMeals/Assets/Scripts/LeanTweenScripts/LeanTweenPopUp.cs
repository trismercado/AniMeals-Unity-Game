using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenPopUp : MonoBehaviour
{
    public Transform box;
    // Start is called before the first frame update
    private void OnEnable()
    {
        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void ClosePopUp() {
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(Done);
    }
    // Update is called once per frame
    void Done() {
        gameObject.SetActive(false);
    }
}
