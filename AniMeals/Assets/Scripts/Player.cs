using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class Player : MonoBehaviour
{
 
    public float hunger;
        public float health;
        public int exp;
        public int level{ get; set; }
        public int coups;
        public List<string> achieve;
        // public List<Items> inventory;
        public List<FoodLog> foodIntake;
        public List<DailyIntake> dailyFoodIntake;
        public DailyIntake dayLog;
        // current skin the pet is using

        //Computations
        public string weight;
        public string height; 
        public string activityLevel;
        public float BMI;
        public float TEA;
        public float CHO;
        public float PRO;
        public float FAT;
        public float DBW;

        //User Intake
        public float TEAIntake;
        public float CHOIntake;
        public float PROIntake;
        public float FATIntake;

        //
        public string assess;
        public string pedromsg1;
        public string pedromsg2;
        public bool isPedroDead;
        public bool receiveReward;
        public int streak;

        public bool popped;

        static Player isUnique;
        private string savePath;


    void Awake() {
        Computations();

        savePath = Application.persistentDataPath + "/gamesave.anmls";

        isNewUser();
        isNewDay();

        if(isUnique != null) {
            Destroy(this.gameObject);
            Debug.Log("Player GameObject was destroyed");
            return;
        }
        isUnique = this;
        DontDestroyOnLoad(this.gameObject);        
    }


    void Update() {
        IncreaseHunger(Time.deltaTime * 0.069444444444f / 60f);
    }

    public void AddToIntake(float cal, float carbs, float protein, float fat, FoodLog food){
        TEAIntake += cal;
        CHOIntake += carbs;
        PROIntake += protein;
        FATIntake += fat;
        foodIntake.Add(food); ////baka pwede to ireplace para diretso na sa daily foodlog
        DecreaseHunger((cal/TEA)*100);
        InsertInFoodLogs();
    }

    public void IncreaseHunger(float amt) {
        hunger += amt;
        if (hunger >= 100f) {
            hunger = 100f;
            isPedroDead = true;
        }
    } 

    public void DecreaseHunger(float amt) {
        hunger -= amt;
        if (hunger < 0f) {
            hunger = 0f;
        }
    } 

    public void Heal(float amt) {
        health += amt;
        if (health >= 100f) {
            health = 100f;
            pedromsg1 = "Thanks for eating well! Do I look strong and healthy? So do you! Let’s keep this going!";
        } else {
            pedromsg1 = "Thanks for eating well! I'm feeling better today!";
        }
        exp += 15;
    }

    public void Damage(float amt) {
        health -= amt;
        if (health <= 0f) {
            health = 0f;
            isPedroDead = true;
        }
    }

    public void Revive(){
        hunger = 0f;
        health = 100f;
        coups -= 5;
        isPedroDead = false;
        streak = 0;
        // assess = "Hey! I am back to life! I missed you!";
        pedromsg1 = "Hey! I am back to life! I missed you!";
        pedromsg2 = "...Don't let me die again ok?";
    }

    public void Reset(){
        foodIntake.Clear();
        TEAIntake=0;
        CHOIntake=0;
        PROIntake=0;
        FATIntake=0;
            
    }
    
    //Round Off is a bit tricky under unity
    public void Computations() {
            
        //Preliminaries
        weight = PlayerPrefs.GetString("weightKeyName");
        height = PlayerPrefs.GetString("heightKeyName");
        activityLevel = PlayerPrefs.GetString("activityLevelKeyName");

        //Body Mass Index
        BMI = float.Parse(weight)/ Mathf.Pow((float.Parse(height)/100), 2f);
        BMI = Mathf.Ceil(BMI * 10f) / 10f;

        // //Assessment
        // if (BMI < 18.5) {
        //     Debug.Log("Underweight");
        // } else if (BMI >= 18.5 && BMI <= 24.9) {
        //     Debug.Log("Normal");
        // } else if (BMI >= 25 && BMI <= 29.9) {
        //     Debug.Log("Overweight");
        // } else if (BMI >= 30) {
        //     Debug.Log("Obese");
        // }
        
        float actLevel = 1f;
        if (float.Parse(activityLevel) == 1) {
            actLevel = 27.5f;
        } else if (float.Parse(activityLevel) == 2) {
            actLevel = 30f;
        } else if (float.Parse(activityLevel) == 3) {
            actLevel = 35f;
        } else if (float.Parse(activityLevel) == 4) {
            actLevel = 40f;
        } else if (float.Parse(activityLevel) == 5) {
            actLevel = 45f;
        }   

        //Total Energy Allowance
        TEA = float.Parse(weight)*actLevel;
        TEA = Mathf.Round(TEA / 50f) * 50f;


        //Carbohydrates
        CHO =  TEA * 0.65f;
        CHO = CHO / 4f;
        CHO = Mathf.Round(CHO / 5f) * 5f;

        //Protein
        PRO = TEA * 0.15f;
        PRO = PRO / 4f;
        PRO = Mathf.Round(PRO / 5f) * 5f;
        
        //Fat
        FAT = TEA * 0.2f;
        FAT = FAT / 9f;
        FAT = Mathf.Round(FAT / 5f) * 5f;


        DBW = (float.Parse(height)-100f)-((float.Parse(height)-100f)*0.1f);
        PlayerPrefs.SetString("DBWKeyName", DBW.ToString());

    }
    
    //Reset intake for new day
    public void isNewDay() {
        string day = System.DateTime.Now.ToString("dd");
        if (PlayerPrefs.HasKey("lastDate")){
            if (day != Convert.ToDateTime(PlayerPrefs.GetString("lastDate")).ToString("dd")){
                checkIntake();
                Reset();
            } else {
                receiveReward = false;
            }
        }
    }

    //checks if the user is new
    public void isNewUser() {
        if (File.Exists(savePath)) { 
            LoadData();
            fromLastAccess(); //updates hunger
        }
        else {
            health=100f;
            exp=10;
            coups=5;
            pedromsg1 = "My name is Pedro!" + "\n\n" + "만나서 반가워요!";
        }
    }

    //updates the hunger level from last time accessed
    public void fromLastAccess() {
        if (PlayerPrefs.HasKey("lastDate")){
            DateTime today = DateTime.Now;
            DateTime lastAccess = Convert.ToDateTime(PlayerPrefs.GetString("lastDate"));
            
            long elapsedTicks = today.Ticks - lastAccess.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            float p = Convert.ToSingle(elapsedSpan.TotalSeconds);
            // Debug.Log("Seconds since last access: "+ p);
            
            IncreaseHunger(p * 0.069444444444f / 60f);
        }
    }

    public void SaveData() {
        var save = new Save()
        {
            health = this.health,
            hunger = this.hunger,
            exp = this.exp,
            level = this.level,
            coups = this.coups,
            assess = this.assess,
            pedromsg1 = this.pedromsg1,
            pedromsg2 = this.pedromsg2,
            achieve = this.achieve,
            streak = this.streak,
            foodIntake = this.foodIntake,
            dailyFoodIntake = this.dailyFoodIntake,
            TEAIntake = this.TEAIntake,
            CHOIntake = this.CHOIntake,
            PROIntake = this.PROIntake,
            FATIntake = this.FATIntake
        };
 
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
        
        string date = System.DateTime.Now.ToString();
        PlayerPrefs.SetString("lastDate", date);
        
        Debug.Log("Data Saved");
    }

    public void LoadData() {
        if (File.Exists(savePath))
        {
            Save save;
 
            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            health = save.health;
            hunger = save.hunger;
            exp = save.exp;
            level = save.level;
            coups = save.coups;
            assess = save.assess;
            pedromsg1 = save.pedromsg1;
            pedromsg2 = save.pedromsg2;
            achieve = save.achieve;
            streak = save.streak;
            foodIntake = save.foodIntake;
            dailyFoodIntake = save.dailyFoodIntake;
            TEAIntake = save.TEAIntake;
            CHOIntake = save.CHOIntake;
            PROIntake = save.PROIntake;
            FATIntake = save.FATIntake;

 
            Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save file doesn't exist.");
        }
    }

    public void DeleteData() {
        File.Delete(savePath);
    }

    void OnApplicationQuit() {
        SaveData();
    }

    void OnApplicationPause(bool isPaused) {
        
        if (isPaused) {
            SaveData();
        } else {
            isNewUser();
            isNewDay();            
        }
        
    }

    public void checkIntake(){
        assess = "";
        pedromsg1 = "";
        pedromsg2 = "";
        bool a = CheckCarbs();
        bool b = CheckFats();
        bool c = CheckPro();
        bool d = CheckCal();
        if (a && b && c && d) {
            Heal(20);            
            pedromsg2 = ""; 
            receiveReward = true;
            streak += 1;
        } if (!a && !b && !c && !d) {
            pedromsg1 = "Remember: " + pedromsg1;
            pedromsg2 = "You didn't reach all the goal nutrients yesterday... So when you were gone my tummy started feeling weird... Let's eat better today so we can heal fast, ok?"; 
            Damage(20);
            streak = 0; 
        } else if (!a || !b || !c || !d) {
            pedromsg1 = "Remember: " + pedromsg1;
            pedromsg2 = pedromsg2 + "\n" + "I'm sure there are plenty of good food for our body. Let's eat better today!";
            Damage(20);
            streak = 0; 
        }

        pedromsg1 = pedromsg1 + "\n\n" + pedromsg2;
    }

    public bool CheckCarbs() {
        if (CHOIntake < (TEA*0.55)/4) {
            // //  assess += "CHO not enough. ";
             pedromsg1 += "\n\t" + "Carbohydrates affect our blood sugar and energy.";
             pedromsg2 += " Also, we didn’t eat enough carbs yesterday, my head is starting to hurt huhu ";
             return false;
        } else if (CHOIntake > (TEA*0.70)/4) {
            //  assess += "CHO not within range. ";
             pedromsg1 += "\n\t" + "Carbohydrates affect our blood sugar and energy.";
             pedromsg2 += " And there were not enough calories yesterday, I’m feeling a little weak today… :( Hope you feel better than I do.";
             return false;
        } else {
            return true;
        }
    }

    public bool CheckPro() {
        if (PROIntake < (TEA*0.10)/4) {
            pedromsg1 += "\n\t" + "Protein is used to create the building blocks of the body.";
            pedromsg2 += " And, I’m feeling so weak today. I think it’s because you didn’t eat enough protein yesterday.";
            return false;
        } else if (PROIntake > PRO){
            pedromsg1 += "\n\t" + "Protein is used to create the building blocks of the body.";
            pedromsg2 += " Also, we ate too much protein yesterday, I feel nauseated and exhausted :(";
            return false;
        } else {
            return true;
        }
    }

    public bool CheckFats() {
        if (FATIntake < FAT) {
            pedromsg1 += "\n\t" + "Fat is our body’s fuel source, and is the major storage of energy in the body.";
            pedromsg2 += " And we didn’t eat enough fats. Fats are good too, you know… Now, I’m feeling a bit stressed. :(";
            return false;
        } else if (FATIntake > (TEA*0.30)/9) {
            pedromsg1 += "\n\t" + "Fat is our body’s fuel source, and is the major storage of energy in the body.";
            pedromsg2 += " Also, we had too much fats. I feel like my blood pressure is up there somewhere…";
            return false;
        } else {
            return true;
        }
    }

    public bool CheckCal () {
        if (TEAIntake < 1500) {
            pedromsg1 += "\n\t" + "Our body needs calories for energy.";
            pedromsg2 += " And there were not enough calories yesterday, I’m feeling a little weak today… :(";
            return false;
        } else if (TEAIntake > TEA) {
            pedromsg1 += "\n\t" + "Our body needs calories for energy.";
            pedromsg2 += " And, I’m feeling a bit tired today. Maybe it’s because we ate too much carbs yesterday :( ";
            return false;
        } else {
            return true;
        }
    }

    public void InsertInFoodLogs() {
        if (dailyFoodIntake.Count != 0) {
            if (dailyFoodIntake[dailyFoodIntake.Count - 1].dateLogged == DateTime.Today.ToString()) {
                dailyFoodIntake[dailyFoodIntake.Count - 1].foodLogsForTheDay = new List<FoodLog>(foodIntake);
                dailyFoodIntake[dailyFoodIntake.Count - 1].TEAIntake = TEAIntake;
                dailyFoodIntake[dailyFoodIntake.Count - 1].CHOIntake = CHOIntake;
                dailyFoodIntake[dailyFoodIntake.Count - 1].PROIntake = PROIntake;
                dailyFoodIntake[dailyFoodIntake.Count - 1].FATIntake = FATIntake;
            }
            else {
                dailyFoodIntake.Add(new DailyIntake(
                                        foodIntake, 
                                        DateTime.Today.ToString(),
                                        TEA,
                                        CHO,
                                        PRO,
                                        FAT,
                                        TEAIntake,
                                        CHOIntake,
                                        PROIntake,
                                        FATIntake
                                    ));
            }
        } else {
            dailyFoodIntake.Add(new DailyIntake(
                        foodIntake,
                        DateTime.Today.ToString(),
                        TEA,
                        CHO,
                        PRO,
                        FAT,
                        TEAIntake,
                        CHOIntake,
                        PROIntake,
                        FATIntake
                    ));
        }
        
        //check mo yung date today 
        //if the same date, rewrite mo yung dailyintake
        //if not, add to the list
    }







    
    

}
