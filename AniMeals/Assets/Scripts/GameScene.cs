using System.Collections;
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

        //replace with animations
        public Image happyimg;
        public Image sadimg;
        public Image deadimg;

        public Image happyssimg;

        private Player gs;
        private GameObject go;
        private PopUp pop;

        public BarSlider calSlider;
        public BarSlider choSlider;
        public BarSlider proSlider;
        public BarSlider fatsSlider;

        public Text achieveText;


        Color32 errorcolor = new Color32(0xA8, 0x4B, 0x4B, 0xFF); 
        Color32 rewardcolor = new Color32(0x3F, 0xC8, 0x8D, 0xFF); 
    
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
        //check for pop up
        if (!gs.popped)
            CheckForPopUp();
    }

    // Update is called once per frame
    void Update()
    { 
        Date_Time();
        ShowFoodIntake();
        CheckHealth();
        UpdateHunger_Coups();
    }

    public void OnAddFoodClick() {
        SceneManager.LoadScene(4);
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

    public void CheckHealth() {
        if (gs.health < 60 || gs.hunger >= 90) {
            if (gs.currentSkinID == 0) {
                sadimg.enabled = false;
                happyimg.enabled = false;
                deadimg.enabled = true;
                happyssimg.enabled = false;
                
            }            
            //dying
        } else if ((gs.health <= 80 && gs.health >= 60) || (gs.hunger > 70)) {
            if (gs.currentSkinID == 0) {
                sadimg.enabled = true;
                happyimg.enabled = false;
                deadimg.enabled = false;
                happyssimg.enabled = false;
            }
            //pedro sad
        } else {
            //healthy
            if (gs.currentSkinID == 0) {
                sadimg.enabled = false;
                happyimg.enabled = true;
                deadimg.enabled = false;
                happyssimg.enabled = false;
            } else if (gs.currentSkinID == 1) {
                happyssimg.enabled = true;
                happyimg.enabled = false;
                deadimg.enabled = false;
                sadimg.enabled = false;

            }
            
            //for changing sprites
            //access the id of current skin and set display here
        }
    }

    public void UpdateHunger_Coups() {
        hungerText.text = Convert.ToInt32(gs.hunger).ToString() +"%";
        healthText.text = gs.health.ToString()+"%";
        coupsText.text = gs.coups.ToString();
    }

    public void CheckForPopUp() {
        if (gs.isPedroDead) {
            Action action = () => {
                gs.Revive();
                gs.popped = true;
            }; 
            pop.PopUpTrue("Oops!", "It seems like you left Pedro to die... You might have to pay the hospital fee for this...", "Revive",errorcolor, action);
        } else if (gs.receiveReward) {
            if (gs.streak == 4) { //3day streak
                Action action = () => {
                    gs.exp += 15;
                    gs.coups += 10;
                    gs.receiveReward = false;
                    if (!gs.achieve.Contains("3-day healthy streak")) {
                        gs.achieve.Add("3-day healthy streak");  
                    }
                    gs.popped = true;
                };
                pop.PopUpTrue("Hey good job!", "Our 3-day streak was a success! For this, I'll give you some extra coups!", "Receive", rewardcolor, action);
            } else if (gs.streak == 8) { //7day streak
                Action action = () => {
                    gs.exp += 35;
                    gs.coups += 25;
                    gs.receiveReward = false;
                    if (!gs.achieve.Contains("7-day healthy streak")) {
                        gs.achieve.Add("7-day healthy streak");  
                    }
                    gs.popped = true;
                };
                pop.PopUpTrue("Wow!", "Our 7-day streak was a success! Pedro is lucky to have you! Some extra coups is in order!", "Receive", rewardcolor, action);
            } else { //no streak
                Action action = () => {
                    gs.exp += 10;
                    gs.coups += 5;
                    gs.receiveReward = false;
                    gs.popped = true;
                };
                pop.PopUpTrue("Congratulations!", "We reached yesterday's goal. Here are some coups!", "Receive", rewardcolor, action);
                // gs.dailyFoodIntake.RemoveAt(dailyFoodIntake.Count-1);
            }
        } else {
            if (File.Exists(Application.persistentDataPath + "/gamesave.anmls")){
                //check here if softstreak > 2, softsreak 4, ss > 8
                Action action = () => {
                    gs.exp += 7;
                    gs.coups += 2;
                    gs.popped = true;
                }; 
                pop.PopUpTrue("Hi!", "Thank you for checking up on me! Here's a couple of coups. Let's spend this day well!", "OK", rewardcolor, action);
            } else { //new user
                Action action = () => {
                    gs.coups += 5;
                    gs.popped = true;
                }; 
                pop.CenterPopUp("Hi, I'm Pedro!", "Thank you for coming in this journey with me! Here, have 5 coups, you might need them later... I also calculated your body mass index and ideal body weight. You can check your profile for that. It's right there at the top left corner! :)", "OK", rewardcolor, action);
            }
        } 
        
    }

    public void Assess() {
        Action action = () => {
        };
        string saysmtng = "Say something interesting...";
        pop.CenterPopUp("Pedro:", saysmtng, "OK", rewardcolor, action);
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

        if (gs.achieve.Count != 0) {
            achieveText.text = gs.achieve[(gs.achieve.Count - 1)];
        } else {
            achieveText.text = "No achievements yet";
        }
    }






    

    




}
