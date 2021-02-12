using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreScene : MonoBehaviour
{
    private GameObject go;
    private Player gs;
    public Skin charSkin;
    public int index;

    public Image pedroImg;
    public Image ssImg;
    public Image choiImg;
    public Image kimImg;
    public Image booImg;

    public Text skinTitle;
    public Text skinDescrp;
    // public Text price;
    public Text coupsText;


    public Image pageCounter1;
    public Image pageCounter2;
    public Image pageCounter3;
    public Image pageCounter4;
    public Image pageCounter5;
    Color32 currentColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    Color32 notCurrentColor = new Color32(0x50, 0x50, 0x50, 0xFF);

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

        index = 0;

        DisplaySkins();
        coupsText.text = gs.coups.ToString();
        // CheckIfPurchased();
    }

    // Update is called once per frame
    void Update()
    {
        PageCounter();
    }

    public void OnBackClick()
    {
        Debug.Log("Back was clicked!");
        SceneManager.LoadScene(3);

    }

    public void OnNextClick()
    {
        index++;
        if (index < 5) {
            DisplaySkins();
        } else {
            index = 4;
            //make button non-interactable
        }

    }

    public void OnPrevClick()
    {
        index--;
        if (index >= 0) {
            DisplaySkins();
        } else {
            index = 0;
            //make button non-interactable
        }

    }

    public void CheckIfPurchased(){
        
    }

    public void DisplaySkins() {
        // Debug.Log(gs.skins[index].title);

        var currentSkin = gs.skins[index];

        //show image
        if (index == 0) {
            pedroImg.enabled = true;
            ssImg.enabled = false;
            choiImg.enabled = false;
            kimImg.enabled = false;
            booImg.enabled = false;
        } else if (index == 1) {
            pedroImg.enabled = false;
            ssImg.enabled = true;
            choiImg.enabled = false;
            kimImg.enabled = false;
            booImg.enabled = false;
        } else if (index == 2) {
            pedroImg.enabled = false;
            ssImg.enabled = false;
            choiImg.enabled = true;
            kimImg.enabled = false;
            booImg.enabled = false;
        } else if (index == 3) {
            pedroImg.enabled = false;
            ssImg.enabled = false;
            choiImg.enabled = false;
            kimImg.enabled = true;
            booImg.enabled = false;
        } else if (index == 4) {
            pedroImg.enabled = false;
            ssImg.enabled = false;
            choiImg.enabled = false;
            kimImg.enabled = false;
            booImg.enabled = true;
        }
        //show title
        skinTitle.text = currentSkin.title; 
        //show description
        skinDescrp.text = currentSkin.description; 



        //buy and select buttons
        if (!currentSkin.isBought) { //if item is not yet bought
            //button text set to "Buy"
            if (gs.coups >= currentSkin.price) {
                //button for buy shows since user can afford
                //show price
                //interactable = true
            } else {
                //show price
                //interactable = false
            }
        }  else if (currentSkin.isBought) {
            //button text set to "select"
            if (!currentSkin.isEquipped) {
                //button to select skin
                //interactable = true
            } else {
                //interactable = false
            }
        }
    }

    public void PurchaseSkin() {
        if (gs.coups > gs.skins[index].price) { //can be removed but for added security wag nalang lol
            gs.coups = gs.coups - gs.skins[index].price;
            gs.skins[index].isBought = true;
            Debug.Log(gs.skins[index].title + ": " + gs.skins[index].isBought);
        }         
    }

    public void SelectSkin() {
        
        foreach (var i in gs.skins) {
            if (i == gs.skins[index]) {
                i.isEquipped = true;
                gs.currentSkinID = index;
            } else {
                i.isEquipped = false;
            }
        }
    }

    public void PageCounter() {
        if (index == 0) {
            pageCounter1.color = currentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
            pageCounter5.color = notCurrentColor;
        } else if (index == 1) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = currentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
            pageCounter5.color = notCurrentColor;
        } else if (index == 2) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = currentColor;
            pageCounter4.color = notCurrentColor;
            pageCounter5.color = notCurrentColor;
        } else if (index == 3) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = currentColor;
            pageCounter5.color = notCurrentColor;
        } else if (index == 4) {
            pageCounter1.color = notCurrentColor;
            pageCounter2.color = notCurrentColor;
            pageCounter3.color = notCurrentColor;
            pageCounter4.color = notCurrentColor;
            pageCounter5.color = currentColor;
        } 
    }
}
