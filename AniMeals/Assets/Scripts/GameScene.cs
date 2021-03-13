﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.IO;

public class GameScene : MonoBehaviour
{
    //Date & Time;
    public Text monthText;
        public Text greetingText;
        public Text playerNameText;
        TimeSpan morningstart = new TimeSpan(0, 0, 0); //00:00:00
        TimeSpan morningend = new TimeSpan(11, 59, 59); //11:59:59
        TimeSpan afternoonstart = new TimeSpan(12, 0, 0); //12 o'clock
        TimeSpan afternoonend = new TimeSpan(17, 59, 59); //5:59
        TimeSpan eveningstart = new TimeSpan(6, 0, 0); //6 o'clock
        TimeSpan eveningend = new TimeSpan(23, 59, 59); //11:59
        TimeSpan now = DateTime.Now.TimeOfDay;

    //Nutrients
    public Text TEAText;
        public Text CHOText;
        public Text PROText;
        public Text FATText;
        public Text TEAIntakeText;
        public Text CHOIntakeText;
        public Text PROIntakeText;
        public Text FATIntakeText;

    //Pedro
    public Text coupsText;
        public Text hungerText;
        public Text healthText;

        private Player gs;
        private GameObject go;
        private PopUp pop;

        public BarSlider calSlider;
        public BarSlider choSlider;
        public BarSlider proSlider;
        public BarSlider fatsSlider;

        public Text achieveText;
        public GameObject[] pedros;
        public GameObject msgnotif;
        Color32 errorcolor = new Color32(0xA8, 0x4B, 0x4B, 0xFF); 
        Color32 rewardcolor = new Color32(0x3F, 0xC8, 0x8D, 0xFF); 
        public Animator[] animator;
    
    // Start is called before the first frame update
    void Start()
    {
        //player instantiate
            go = GameObject.Find("Player");
            if (go == null) {
                Debug.LogError("No player gameobject");
                this.enabled = false;
                return;
            }
            gs = go.GetComponent<Player>();
        //pop up instantiate
            go = GameObject.Find("PopUp");
            if (go == null) {
                Debug.LogError("No pop up gameobject");
                this.enabled = false;
                return;
            }
            pop = go.GetComponent<PopUp>();
        //display
            Displays();

        if (gs.health == 0f) {
            DeadHealthPopUp();  
            gs.popped = true;
        }  

        CheckPedroSkin();

        if (gs.justAte) {
            animator[gs.currentSkinID].SetTrigger("JustAte");
            gs.justAte = false; 
        }
        
    }

    // Update is called once per frame
    void Update()
    { 
        Date_Time();
        ShowFoodIntake();
        UpdateHunger_Coups_Achievement();
        
        
        
        if (gs.hunger == 100f && gs.health != 0f) {
            DeadPopUp();  
            gs.popped = true;
        } else if (gs.hunger == 100f && gs.health == 0f) {

        } else {
            if (!gs.popped) {
                CheckForPopUp();
                gs.popped = true;
            }
        }

        msgnotif.SetActive(!gs.opened);       
            
    }


    public void OnAddFoodClick() {
        SceneManager.LoadScene(4);
    }

    public void CreditsClick() {
        Action action = () => {
            
        }; 
        pop.PopUpTrue("Credits", "Music: \n Tranquility by Dee Yan-Key \n\n Icons: ", "OK", rewardcolor, action);
    } 

    public void OnAchievementClick(){
        SceneManager.LoadScene(6);
    }
    
    public void OnStoreClick() {
        SceneManager.LoadScene(7);
    }

    public void OnProfileClick() {
        SceneManager.LoadScene(5);
    }

    public void OnMsgClick() {
        gs.opened = true;
        SceneManager.LoadScene(8);
    }

    public void OnLogClick() {
        SceneManager.LoadScene(9);
    }

    public void Date_Time() {
        string month = System.DateTime.Now.ToString("MMM dd");
        monthText.text = month;

        if (now >= morningstart && now <= morningend)
        {
            greetingText.text = "Good Morning,";
        } else if (now >= afternoonstart && now <= afternoonend)
        {
            greetingText.text = "Good Afternoon,";
        } else if (now >= eveningstart && now <= eveningend)
        {
            greetingText.text = "Good Evening,";
        }
    }

    public void ShowFoodIntake() {       
        TEAIntakeText.text = gs.TEAIntake.ToString() +"kcal";
        CHOIntakeText.text = gs.CHOIntake.ToString() +"g";
        PROIntakeText.text = gs.PROIntake.ToString() +"g";
        FATIntakeText.text = gs.FATIntake.ToString() +"g";
    }

    public void CheckPedroSkin() {
        for (int i=0; i < 5; i++) {
            if (gs.currentSkinID == i) {
                pedros[i].SetActive(true);
            } else {
                pedros[i].SetActive(false);
            }
        }
        
    }

    public void UpdateHunger_Coups_Achievement() {
        hungerText.text = Convert.ToInt32(gs.hunger).ToString() +"%";
        healthText.text = gs.health.ToString()+"%";
        coupsText.text = gs.coups.ToString();
        if (gs.achieve.Count != 0) {
            achieveText.text = gs.achieve[(gs.achieve.Count - 1)];
        } else {
            achieveText.text = "No achievements yet";
        }


        

        if ((gs.hunger >= 90 && gs.hunger <= 100) || (gs.health == 20 || gs.health == 40)) {
            animator[gs.currentSkinID].SetBool("isHappy", false);
            animator[gs.currentSkinID].SetBool("isSad", false);
            animator[gs.currentSkinID].SetBool("isSick", true);
        } else if ((gs.hunger >= 70 && gs.hunger <= 90) || (gs.health == 60 || gs.health == 80)) {
            animator[gs.currentSkinID].SetBool("isHappy", false);
           animator[gs.currentSkinID].SetBool("isSad", true);
            animator[gs.currentSkinID].SetBool("isSick", false);
        } else {
            animator[gs.currentSkinID].SetBool("isHappy", true);
            animator[gs.currentSkinID].SetBool("isSad", false);
            animator[gs.currentSkinID].SetBool("isSick", false);
        }
    }

    public void CheckForPopUp() {
        if (gs.receiveReward && !gs.isPedroDead) {
            if (gs.streak == 4) { //3day streak
                Action action = () => {
                    if (!gs.achieve.Contains("Health is Wealth!")) {
                        gs.achieve.Add("Health is Wealth!");
                        if (!gs.achieve.Contains("Third Time is A Charm")) { gs.achieve.Add("Third Time is A Charm"); }
                    }
                    gs.receiveReward = false;
                    gs.coups += 6;
                };
                pop.PopUpTrue("Hey good job!","We hit our 3rd healthy streak! That's 2 achievements at once! \n\n You're getting: +6 Coups", "Receive", rewardcolor, action);
            } else if (gs.streak == 8) { //7day streak
                Action action = () => {
                    gs.receiveReward = false;
                    if (!gs.achieve.Contains("Health Nut!")) {
                        gs.achieve.Add("Health Nut!"); 
                        if (!gs.achieve.Contains("Weak? Not Me!")) 
                            gs.achieve.Add("Weak? Not Me!");
                    } 
                    gs.coups += 10;
                };
                pop.PopUpTrue("Wow!", "A week of healthy eating! I'm so lucky to have you! \n\n You're getting: +10 Coups", "Receive", rewardcolor, action);
            } else if (gs.streak == 11) { //10day streak
                Action action = () => {
                    gs.receiveReward = false;
                    if (!gs.achieve.Contains("Look at You! Keep it Up!")) {
                        gs.achieve.Add("Look at You! Keep it Up!"); 
                        if (!gs.achieve.Contains("Ten is Mightier than the Sword!")) 
                            gs.achieve.Add("Ten is Mightier than the Sword!");
                    }
                    gs.coups += 14;
                };
                pop.PopUpTrue("Amazing!", "I have been healthy for 10 days! Now that's an achievement! \n\n You're getting: +14 Coups", "Receive", rewardcolor, action);
            } else if (gs.streak == 15) { //14day streak
                Action action = () => {
                    gs.receiveReward = false;
                    if (!gs.achieve.Contains("You're Doing Great!")) {
                        gs.achieve.Add("You're Doing Great!"); 
                        if (!gs.achieve.Contains("It's Twice as Nice")) 
                            gs.achieve.Add("It's Twice as Nice");
                    }
                    gs.coups += 18;
                };
                pop.PopUpTrue("That's crazy!", "You're a natural! You must be feeling as great as I do! \n\n You're getting: +18 Coups", "Receive", rewardcolor, action);
            } else { //no streak
                Action action = () => {
                    gs.coups += 2;
                    gs.receiveReward = false;

                };
                pop.PopUpTrue("Congratulations!", "We reached yesterday's goal. Here are some coups! \n\n You're getting: +2 Coups", "Receive", rewardcolor, action);
            }
        } else if (!gs.isPedroDead) {
            if (File.Exists(Application.persistentDataPath + "/gamesave.anmls")){
                if (gs.softstreak == 4) {
                    Action action = () => {
                        if (!gs.achieve.Contains("Third Time is A Charm")) { //3day streak
                            gs.achieve.Add("Third Time is A Charm");
                        }
                        gs.coups += 3;
                    };                 
                    pop.PopUpTrue("Hi there!", "Thanks for putting in the effort for the past 3 days. \n\n You're getting: +3 Coups", "Receive", rewardcolor, action);
                } else if (gs.softstreak == 8) { //7day streak
                    Action action = () => {
                        if (!gs.achieve.Contains("Weak? Not Me!")) { //3day streak
                            gs.achieve.Add("Weak? Not Me!");
                        }
                        gs.coups += 5;
                    };
                    pop.PopUpTrue("That's cool!", "It's been a week and we are not giving up just yet! \n\n You're getting: +5 Coups", "Receive", rewardcolor, action);
                } else if (gs.softstreak == 11) { //10day streak
                    Action action = () => {
                        if (!gs.achieve.Contains("Ten is Mightier than the Sword!")) { //3day streak
                            gs.achieve.Add("Ten is Mightier than the Sword!");
                        }
                        gs.coups += 7;
                    };
                    pop.PopUpTrue("Let's keep going!", "I know you've been trying and I'm very grateful! \n\n You're getting: +7 Coups", "Receive", rewardcolor, action);
                } else if (gs.softstreak == 15) { //14day streak
                    Action action = () => {
                        if (!gs.achieve.Contains("It's Twice as Nice")) { //3day streak
                            gs.achieve.Add("It's Twice as Nice");
                        } 
                        gs.coups += 9;

                    };
                    pop.PopUpTrue("ᕙ(`▿´)ᕗ", "I haven't been hungry for the past 2 weeks. Thanks for being consistent! \n\n You're getting: +9 Coups", "Receive", rewardcolor, action);
                } else {
                    Action action = () => {
                        gs.coups += 1;
                    }; 
                    pop.PopUpTrue("Hi!", "Thank you for checking up on me! Let's spend this day well! \n\n You're getting: +1 Coup", "OK", rewardcolor, action);
                }
            } else { //new user
                Action action = () => {
                    gs.coups += 5;

                }; 
                pop.CenterPopUp("Welcome!", "It's nice to meet you. Thanks for joining! For that, I shall start you off with 5 coups! Oh, and coups is short for coupons! You can check out Achievements to see how you can get more coups. Collect as much as you can to get some cool stuff at the store!", "Receive", rewardcolor, action);
            }    
        } 
        
    }

    public void DeadPopUp() {
        Action action = () => {
            gs.streak = 0;
            gs.softstreak = 0;
            gs.pedromsg1 = "Hey! I am back! I missed you! \n \n ...Don't let me starve again ok?";
        }; 
        pop.CenterDeadPopUp("Oops!", "It seems like you left Pedro to starve... Log the food you ate today to get him back...", "Log Meal",errorcolor, action);

    }

    public void DeadHealthPopUp(){
        Action action = () => {
            gs.streak = 0;
            gs.softstreak = 0;
            gs.health = 100f;
            gs.hunger = 0f;
            gs.coups -= 2;
            gs.pedromsg1 = "Hey! I feel good as new! How have you been? \n \n ...Don't let me die again ok?";
        }; 
        pop.CenterPopUp("Oh no!", "Pedro felt very ill... His current health is at 0. You will have to pay the hospital bills for this.", "Pay Hospital",errorcolor, action);

    }  

    public void Assess() {
        string[] quotes = new string[] {
            "I know you'll do great today!",
            "If you're tired. It's okay to get some rest and refuel!",
            "I wonder what you're up to today...",
            "Ooohh hi!",
            ":)",
            "roses are red, violets are not purple, the way you eat today will determine my mood tomorrow! :))))",
            "Small victories are essential to big changes!",
            "I write you letters regarding your intake every day... I hope you take note of them!",
            "The percentage distribution of these nutrients can differ from person to person because of various diet prescriptions and usual eating habits... So, I made sure to make the appropriate range for you.",
            "I don't usually die but when my health reaches 0, you'd have to pay 1 coup for my bills... :/",
            "How was your day?",
            "I know... I'm a bit awkward sometimes... :/",
            "If you were waiting for your sign, this is it!",
            "Hi! Thanks for visiting!",
            "Did you know? \n Our body needs calories for energy. \n Carbohydrates affect our blood sugar and energy. \n Protein is used to create the building blocks of the body. \n Fat is our body’s fuel source, and is the major storage of energy in the body.",
            "The more we become aware, the more that we can make healthy choices!",
            "Did you know? Protein, Carbohydrates, and Fats are called Macronutrients. In short, macros! On the other hand, Vitamins and Minerals are called Micronutrients!",
            "Eating healthy can be really hard... So don't forget to take it little by little!",
            "Don't skip your fruits and veggies!"
        };
        System.Random rnd = new System.Random();
        Action action = () => {
        };
        pop.CenterPopUp("Pedro:", quotes[rnd.Next(0, quotes.Length-1)], "OK", rewardcolor, action);
    }

    public void Displays() {
        playerNameText.text = PlayerPrefs.GetString("nameKeyName");
        TEAText.text = gs.TEA.ToString() +"kcal";
        CHOText.text = gs.CHO.ToString() +"g";
        PROText.text = gs.PRO.ToString() +"g";
        FATText.text = gs.FAT.ToString() +"g";
        calSlider.SetMaxValue(gs.TEA);
        calSlider.SetCurrentVal(gs.TEAIntake);
        choSlider.SetMaxValue(gs.CHO);
        choSlider.SetCurrentVal(gs.CHOIntake);
        proSlider.SetMaxValue(gs.PRO);
        proSlider.SetCurrentVal(gs.PROIntake);
        fatsSlider.SetMaxValue(gs.FAT);
        fatsSlider.SetCurrentVal(gs.FATIntake);

        
    }






    

    




}
