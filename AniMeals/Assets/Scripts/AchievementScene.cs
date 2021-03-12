using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementScene : MonoBehaviour
{
    private GameObject go;
    private Player gs;
    private AchievementPopUp pop;
    
    public GameObject[] awards;

    Color32 rewardcolor = new Color32(0x3F, 0xC8, 0x8D, 0xFF); 

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

        go = GameObject.Find("AchievementPopUp");
            if (go == null) {
                Debug.LogError("No pop up gameobject");
                this.enabled = false;
                return;
            }
        pop = go.GetComponent<AchievementPopUp>();

        CheckNoTrophies();
    }

    public void Update() {
        CheckIfAchieved();
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(3);

    }

    public void CheckIfAchieved() {
        if (gs.achieve.Count != 0) {
            foreach (string i in gs.achieve) {
                if (i.Equals("Third Time is A Charm")) {
                    awards[0].SetActive(true);
                } else if (i.Equals("Weak? Not Me!")) {
                    awards[1].SetActive(true);
                } else if (i.Equals("Ten is Mightier than the Sword")) {
                    awards[2].SetActive(true);
                } else if (i.Equals("It's Twice as Nice")) {
                    awards[3].SetActive(true);
                } else if (i.Equals("You Got Style!")) {
                    awards[4].SetActive(true);
                } else if (i.Equals("A Whole Closet!")) {
                    awards[5].SetActive(true);
                } else if (i.Equals("Shopping Spree")) {
                    awards[6].SetActive(true);
                } else if (i.Equals("Health is Wealth!")) {
                    awards[7].SetActive(true);
                } else if (i.Equals("Health Nut!")) {
                    awards[8].SetActive(true);
                } else if (i.Equals("Look at You! Keep it Up!")) {
                    awards[9].SetActive(true);
                } else if (i.Equals("You're Doing Great")) {
                    awards[10].SetActive(true);
                } else if (i.Equals("Pros And Bronze")) {
                    awards[11].SetActive(true);
                } else if (i.Equals("Silver Lining")) {
                    awards[12].SetActive(true);
                } else if (i.Equals("Winner Winner Chicken Dinner!")) {
                    awards[13].SetActive(true);
                }
            }
        } else {
            foreach (var i in awards) {
                 i.SetActive(false);
            } 
        } 
    }

    public void CheckNoTrophies() {
        if (gs.achieve.Count != 0) {
            if (gs.achieve.Count >= 3 && gs.achieve.Count < 6 && !gs.achieve.Contains("Pros And Bronze")) {
                Action action = () => {
                        gs.achieve.Add("Pros And Bronze"); 
                        gs.coups += 5; 
                };
                string saysmtng = "An additional achievement for reaching unlocking 3 trophies! \n\n You're getting: +5 Coups ";
                pop.PopUpTrue("That's great!", saysmtng, "Receive", rewardcolor, action);
                
            } else if (gs.achieve.Count >= 6 && gs.achieve.Count < 10 && !gs.achieve.Contains("Silver Lining")) {
                Action action = () => {
                        gs.achieve.Add("Silver Lining"); 
                        if (!gs.achieve.Contains("Pros And Bronze")) {
                            gs.achieve.Add("Pros And Bronze"); 
                            gs.coups += 5; 
                        }
                        gs.coups += 7; 
                };
                string saysmtng = "6 trophies? Your progress is showing! \n\n You're getting: +7 Coups ";
                pop.PopUpTrue("What a Pro!", saysmtng, "Receive", rewardcolor, action);                
            } else if (gs.achieve.Count >= 10 && !gs.achieve.Contains("Winner Winner Chicken Dinner!")) {
                Action action = () => {
                    gs.achieve.Add("Winner Winner Chicken Dinner!"); 
                    if (!gs.achieve.Contains("Pros And Bronze")) {
                        gs.achieve.Add("Pros And Bronze"); 
                        gs.coups += 5; 
                    }
                    if (!gs.achieve.Contains("Silver Lining")) {
                        gs.achieve.Add("Silver Lining"); 
                        gs.coups += 7; 
                    }
                    gs.coups += 9; 
                };
                string saysmtng = " You're trophy shelf looks pretty cool! You deserve it! \n\n You're getting: +9 Coups";
                pop.PopUpTrue("You're a Superstar!", saysmtng, "Receive", rewardcolor, action);  
            } 
        }
        
    }
}
