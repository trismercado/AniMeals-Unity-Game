using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    static DontDestroyAudio isUnique;
    // Start is called before the first frame update
    void Awake() {
        if(isUnique != null) {
            Destroy(this.gameObject);
            Debug.Log("Audio GameObject was destroyed");
            return;
        }
        isUnique = this;
        DontDestroyOnLoad(transform.gameObject);
    }
}
