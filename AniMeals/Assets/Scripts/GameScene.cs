using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

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
        public Image happyimg;
        public Image sadimg;
        public Image deadimg;

        private Player gs;
        private GameObject go;
        private PopUp pop;

        public BarSlider calSlider;
        public BarSlider choSlider;
        public BarSlider proSlider;
        public BarSlider fatsSlider;


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
        Debug.Log("Add Food was clicked!");
        SceneManager.LoadScene(4);
    }

    public void OnAchievementClick(){
        // Debug.Log("Achievement was clicked!");
        SceneManager.LoadScene(6);
    }
    
    public void OnStoreClick() {
        Debug.Log("Store was clicked!");
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
            sadimg.enabled = false;
            happyimg.enabled = false;
            deadimg.enabled = true;
            //dying
        } else if ((gs.health <= 80 && gs.health >= 60) || (gs.hunger > 70)) {
            sadimg.enabled = true;
            happyimg.enabled = false;
            deadimg.enabled = false;
            //pedro sad
        } else {
            //healthy
            sadimg.enabled = false;
            happyimg.enabled = true;
            deadimg.enabled = false;
        }
    }

    public void UpdateHunger_Coups() {
        hungerText.text = Convert.ToInt32(gs.hunger).ToString() +"%";
        healthText.text = gs.health.ToString()+"%";
        // hungerText.text = gs.hunger.ToString();
        coupsText.text = gs.coups.ToString();
    }

    public void CheckForPopUp() {
        if (gs.isPedroDead) {
            Action action = () => {
                gs.Revive();
            }; 
            pop.PopUpTrue("Oops!", "It seems like you left Pedro to die... You might have to pay the hospital fee for this...", "Revive",errorcolor, action);
        } else if (gs.receiveReward) {
            if (gs.streak == 4) { //3day streak
                Action action = () => {
                    gs.exp += 15;
                    gs.coups += 10;
                    gs.receiveReward = false;

                    if (!gs.achieve.Contains("streak3")) {
                        gs.achieve.Add("streak3");  
                    }
                };
                pop.PopUpTrue("Hey good job!", "Your 3-day streak was a success! For this, I'll give you some extra coups!", "Receive", rewardcolor, action);
            } else if (gs.streak == 8) { //7day streak
                Action action = () => {
                    gs.exp += 35;
                    gs.coups += 25;
                    gs.receiveReward = false;
                    if (!gs.achieve.Contains("streak7")) {
                        gs.achieve.Add("streak7");  
                    }
                };
                pop.PopUpTrue("Wow!", "Your 7-day streak was a success! Pedro is lucky to have you! Some extra coups is in order!", "Receive", rewardcolor, action);
            } else { //no streak
                Action action = () => {
                    gs.exp += 10;
                    gs.coups += 5;
                    gs.receiveReward = false;
                };
                pop.PopUpTrue("Congratulations!", "You reached yesterday's goal. Here are some coups!", "Receive", rewardcolor, action);
                // gs.dailyFoodIntake.RemoveAt(dailyFoodIntake.Count-1);
            }
        } else {
            string day = System.DateTime.Now.ToString("dd");
            if (PlayerPrefs.HasKey("lastDate")){
                if (day != Convert.ToDateTime(PlayerPrefs.GetString("lastDate")).ToString("dd")){ //new day, daily reward (consolation)
                    Action action = () => {
                        gs.exp += 7;
                        gs.coups += 2;
                    }; 
                    pop.PopUpTrue("Hi!", "Thank you for checking up on me! Here's a couple of coups. Let's spend this day well!", "OK", rewardcolor, action);
                } 
            } else { //new user
                // Action action = () => {
                // }; 
                // pop.CenterPopUp("Hi, I'm Pedro!", "Please take care of me... no, I mean us... is that right? Hmm, basically I eat what you eat... so eat well and let's stay healthy!", "OK", rewardcolor, action);
                gs.assess = "Please take care of me... no, I mean us... is that right? Hmm, basically I eat what you eat... so eat well and let's stay healthy!";
            }
        } 
        gs.popped = true;
    }

    public void Assess() {
        Action action = () => {
        };
        string saysmtng = "Say something interesting...";
        pop.CenterPopUp("Pedro:", saysmtng, "OK", rewardcolor, action);
    }




    

    




}
