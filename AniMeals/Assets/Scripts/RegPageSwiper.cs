using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegPageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.3f;
    public int totalPages = 4;
    public int currentPage = 1;
    public Image pageCounter1;
    public Image pageCounter2;
    public Image pageCounter3;
    public Image pageCounter4;
    Color32 currentColor = new Color32(0x00, 0x00, 0x00, 0xFF);
    Color32 notCurrentColor = new Color32(0x93, 0x93, 0x93, 0xFF);



    // Start is called before the first frame update
    void Start()
    {
        panelLocation = transform.position;    
    }

    void Update()
    {
        if (currentPage == 1) {
            pageCounter1.color = currentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
        } else if (currentPage == 2) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = currentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
        } else if (currentPage == 3) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = currentColor;
            pageCounter4.color = notCurrentColor;
        } else if (currentPage == 4) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = currentColor;
        } 
    }

    public void OnDrag(PointerEventData data) {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data) {
        
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= percentThreshold) {
            Vector3 newLocation = panelLocation;
            if (percentage > 0 && currentPage < totalPages) {
                currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
            }else if(percentage < 0 && currentPage > 1) {
                currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        } else {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds) {
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
