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

    public GameObject[] skins;

    public Text skinTitle;
    public Text skinDescrp;
    public Text price;
    public Text coupsText;
    public Text BtnText;

    public Button Btn;
    public Button nxtBtn;
    public Button prvBtn;

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
    }

    void Update()
    {
        coupsText.text = gs.coups.ToString();
        DisplaySkins();
        PageCounter();

        if (index == 0) {
            prvBtn.gameObject.SetActive(false);
        } else if (index == 4) {
            nxtBtn.gameObject.SetActive(false);
        } else {
            prvBtn.gameObject.SetActive(true);
            nxtBtn.gameObject.SetActive(true);
        }
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
        } else {
            index = 4;
        }
    }

    public void OnPrevClick()
    {
        index--;
        if (index >= 0) {
        } else {
            index = 0;
        }
    }

    public void DisplaySkins() {

        var currentSkin = gs.skins[index];

        //show image, title, description, price
        for (int i = 0; i < 5; i++) {
            if (i == index) {
                skins[i].SetActive(true);
            } else {
                skins[i].SetActive(false);
            }
        }
        skinTitle.text = currentSkin.title; 
        skinDescrp.text = currentSkin.description; 
        price.text = currentSkin.price.ToString();

        //buy and select buttons
        if (!currentSkin.isBought) { 
            if (gs.coups >= currentSkin.price) {
                BtnText.text = "Buy";
                Btn.interactable = true;
            } else {
                BtnText.text = "Not enough coups";
                Btn.interactable = false;
            }
        }  else if (currentSkin.isBought) {
            if (!currentSkin.isEquipped) {
                BtnText.text = "Equip";
                Btn.interactable = true;
            } else {
                BtnText.text = "Equipped";
                Btn.interactable = false;

            }
        }
    }

    public void CheckIfPurchasedOrSelect(){
        if (!gs.skins[index].isBought) {
            PurchaseSkin();
        } else {
            SelectSkin();
        }
    }

    public void PurchaseSkin() {
        if (gs.coups > (int)gs.skins[index].price) { //can be removed but for added security wag nalang lol
            gs.coups = gs.coups - (int)gs.skins[index].price;
            gs.skins[index].isBought = true;
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
