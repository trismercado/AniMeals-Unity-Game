using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessagingScene : MonoBehaviour
{
    private Player gs;
    private GameObject go;

    public Text greeting;
    public Text firstPart;
    // public Text secondPart;

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("Player");
            if (go == null) {
                Debug.LogError("No player gameobject");
                this.enabled = false;
                return;
            }
        gs = go.GetComponent<Player>();

        
        // secondPart.text = gs.pedromsg2;
    }

    void Update() {
        greeting.text = "Dear " + PlayerPrefs.GetString("nameKeyName") + ",";
        firstPart.text = gs.pedromsg1;
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(3);
    }



}
