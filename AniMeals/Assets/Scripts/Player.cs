using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class Player : MonoBehaviour
{
    //pedro
    public float hunger;
        public float health;
        public int exp;
        public int level{ get; set; }
        public int coups;
        public List<string> achieve;
        public List<FoodLog> foodIntake;
        public List<DailyIntake> dailyFoodIntake;
        public int currentSkinID;

        //Computations
        string weight;
        string height; 
        string activityLevel;
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

        //messages and achievement
        public string assess;
        public string pedromsg1;
        public string pedromsg2;
        public bool isPedroDead;
        public bool receiveReward;
        public int streak;
        public int softstreak;
        public bool popped;
        public bool showPedroDead;
        // public bool isDiscounted;

        static Player isUnique;
        private string savePath;

        //store
        public List<Skin> skins; //includes pedro (default)


    void Awake() {
        Computations();

        savePath = Application.persistentDataPath + "/gamesave.anmls";

        isNewUser();
        

        if(isUnique != null) {
            Destroy(this.gameObject);
            Debug.Log("Player GameObject was destroyed");
            return;
        }
        isUnique = this;
        DontDestroyOnLoad(this.gameObject);  
        // SetDiscounts();
    }


    void Update() {
        isNewDay();
        
        IncreaseHunger(Time.deltaTime * 0.069444444444f / 60f);
    }

    public void AddToIntake(float cal, float carbs, float protein, float fat, FoodLog food){
        TEAIntake += cal;
        CHOIntake += carbs;
        PROIntake += protein;
        FATIntake += fat;
        // foodIntake.Add(food); 
        // DecreaseHunger((cal/TEA)*100);
        if (TEAIntake < TEA) DecreaseHunger((cal/(TEA-TEAIntake))*100); 
        //the logged cal intake over the remaining cal u have to consume
        else DecreaseHunger((cal/TEA)*100);
        // //if cal intake is greater than the required just decrease hunger with regards to its cal requirement
        // //at this point exceeding calorie intake should not have good benefits on pedro
        InsertInFoodLogs(food);
    }

    public void IncreaseHunger(float amt) {
        hunger += amt;
        if (hunger >= 100f) {
            hunger = 100f;
            isPedroDead = true;
        } else {
            isPedroDead = false;
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
        } else {
            isPedroDead = false;
        }
    }

    public void Revive(){
        isPedroDead = false;
        hunger = 0f;
        health = 100f;
        coups -= 1;
        streak = 0;
        softstreak = 0;
        pedromsg1 = "Hey! I am back to life! I missed you! \n \n ...Don't let me die again ok?";
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
        if (dailyFoodIntake.Count != 0) {
            if (DateTime.Today.ToString() != dailyFoodIntake[(dailyFoodIntake.Count - 1)].dateLogged)
            {
                //what we want: to be able to get the daily reward and check the intake 
                //even when the app is open and the app prgoresses to the next day 
                popped = false;
                checkIntake();
                CreateFoodLog();
            }
        } else {
            CreateFoodLog();
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
            pedromsg1 = "My name is Pedro and this is AniMeals! I hope you'll enjoy your time with me! Hmm... To tell you a little more about myself... I usually like anime, movies, and hmm what else... OH! I LOVE HARRY POTTER!!!! My makers thought I'll fit right in with Hufflepuff! Can you believe them? Haha! I personally think I'm more of a Gryffindor, to be honest. Also, I was born with the purpose to track your eating journey. So, I usually get a little sensitive when I don't eat for a whole day. I hope you'll understand... Yeah, I guess that's all for now. " + "\n\n" + "Oh and by the way, you can come back here every day to check up on me!";
            PopulateSkinList();
            currentSkinID = 0;
            popped = false;
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

    //files
    public void SaveData() {
        var save = new Save()
        {
            health = this.health,
            hunger = this.hunger,
            exp = this.exp,
            level = this.level,
            coups = this.coups,
            currentSkinID = this.currentSkinID,
            assess = this.assess,
            pedromsg1 = this.pedromsg1,
            pedromsg2 = this.pedromsg2,
            popped = this.popped,
            achieve = this.achieve,
            streak = this.streak,
            softstreak = this.softstreak,
            foodIntake = this.foodIntake,
            dailyFoodIntake = this.dailyFoodIntake,
            skins = this.skins,
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
        
        // Debug.Log("Data Saved");
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
            currentSkinID = save.currentSkinID;
            assess = save.assess;
            pedromsg1 = save.pedromsg1;
            pedromsg2 = save.pedromsg2;
            achieve = save.achieve;
            streak = save.streak;
            softstreak = save.softstreak;
            popped = save.popped;
            foodIntake = save.foodIntake;
            dailyFoodIntake = save.dailyFoodIntake;
            skins = save.skins;
            TEAIntake = save.TEAIntake;
            CHOIntake = save.CHOIntake;
            PROIntake = save.PROIntake;
            FATIntake = save.FATIntake;

 
            // Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save file doesn't exist.");
        }
    }

    public void DeleteData() {
        File.Delete(savePath);
    }

    //app activities
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

    //assess food intake
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
            softstreak += 1;
        } if (!a && !b && !c && !d) {
            pedromsg1 = "Remember: " + pedromsg1;
            pedromsg2 = "You didn't reach all the goal nutrients yesterday... So when you were gone my tummy started feeling weird... Let's eat better today so we can heal fast, ok?"; 
            Damage(20);
            streak = 0; 
            softstreak = 0;
        } else if (!a || !b || !c || !d) {
            pedromsg1 = "Remember: " + pedromsg1;
            pedromsg2 = pedromsg2 + "\n\n" + "I'm sure there are plenty of good food for our body. Let's eat better today!";
            Damage(20);
            streak = 0; 

            if (foodIntake.Count > 3) {
                softstreak += 1;
                Debug.Log(softstreak);
            } else {
                softstreak = 0;
            }
        }

        pedromsg1 = pedromsg1 + "\n\n" + pedromsg2;
    }

    public bool CheckCarbs() {
        if (CHOIntake < (TEA*0.55)/4) {
             pedromsg1 += "\n\t" + "Carbohydrates affect our blood sugar and energy.";
             pedromsg2 += " We didn’t eat enough carbs yesterday, my head is starting to hurt huhu ";
             return false;
        } else if (CHOIntake > (TEA*0.70)/4) {
             pedromsg1 += "\n\t" + "Carbohydrates affect our blood sugar and energy.";
             pedromsg2 += " I’m feeling a bit tired today. Maybe it’s because we ate too much carbs yesterday? :( ";
             return false;
        } else {
            return true;
        }
    }

    public bool CheckPro() {
        if (PROIntake < (TEA*0.10)/4) {
            pedromsg1 += "\n\t" + "Protein is used to create the building blocks of the body.";
            pedromsg2 += " I’m feeling so weak today. I think it’s because you didn’t eat enough protein yesterday.";
            return false;
        } else if (PROIntake > PRO){
            pedromsg1 += "\n\t" + "Protein is used to create the building blocks of the body.";
            pedromsg2 += " We ate too much protein yesterday, I feel nauseated and exhausted. :(";
            return false;
        } else {
            return true;
        }
    }

    public bool CheckFats() {
        if (FATIntake < FAT) {
            pedromsg1 += "\n\t" + "Fat is our body’s fuel source, the major storage of energy in the body.";
            pedromsg2 += " We didn’t eat enough fats. Fats are good too, you know… Now, I’m feeling a bit stressed. :(";
            return false;
        } else if (FATIntake > (TEA*0.30)/9) {
            pedromsg1 += "\n\t" + "Fat is our body’s fuel source, the major storage of energy in the body.";
            pedromsg2 += " We had too much fats. I feel like my blood pressure is up there somewhere…";
            return false;
        } else {
            return true;
        }
    }

    public bool CheckCal () {
        if (TEAIntake < 1200) {
            pedromsg1 += "\n\t" + "Our body needs calories for energy.";
            pedromsg2 += " There were not enough calories yesterday, I’m feeling a little weak today… :(";
            return false;
        } else if (TEAIntake > TEA) {
            pedromsg1 += "\n\t" + "Our body needs calories for energy.";
            pedromsg2 += "  I had WAAAAY too many calories, I’m feeling a little bloated… How are you?";
            return false;
        } else {
            return true;
        }
    }

    public void InsertInFoodLogs(FoodLog food) {
        if (dailyFoodIntake.Count != 0) {
            if (dailyFoodIntake[dailyFoodIntake.Count - 1].dateLogged == DateTime.Today.ToString()) {
                dailyFoodIntake[dailyFoodIntake.Count - 1].foodLogsForTheDay.Add(food);
                dailyFoodIntake[dailyFoodIntake.Count - 1].TEAIntake = TEAIntake;
                dailyFoodIntake[dailyFoodIntake.Count - 1].CHOIntake = CHOIntake;
                dailyFoodIntake[dailyFoodIntake.Count - 1].PROIntake = PROIntake;
                dailyFoodIntake[dailyFoodIntake.Count - 1].FATIntake = FATIntake;
            }
            else {
                CreateFoodLog();
                dailyFoodIntake[dailyFoodIntake.Count - 1].foodLogsForTheDay.Add(food);
            }
        } else { //can work without this
            CreateFoodLog();
        }
    }

    public void CreateFoodLog() {
        Reset();
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

    //store
    public void PopulateSkinList() {
        skins.Add(new Skin(0, "Pedro", "Your friend forever!", 0, true, true));
        skins.Add(new Skin(1, "Dairy Fairy", "Bippity boppityy cook!", 8, false, false));
        skins.Add(new Skin(2, "Surf and Burp", "My favorite course? Main, of course!", 12, false, false));
        skins.Add(new Skin(3, "Moonpie", "BRB. Currently, out of this world.", 20, false, false));
        skins.Add(new Skin(4, "P-Potter", "You're a wizard, Pedro...", 40, false, false));
    }

    public void SetDiscounts() {
        //do this only once
        //return to old price if not a sale day (you can set it manually naman since 4 lang yung items)

        DateTime today = DateTime.Today;
        DateTime regDate = Convert.ToDateTime(PlayerPrefs.GetString("isRegisteredKeyName"));

        if (regDate.AddDays(1) == today) {
            Debug.Log("Sale today!");
            for (int i = 0; i < 5; i++) {
                skins[i].price = skins[i].price - (skins[i].price*0.5f);
                Debug.Log(skins[i].title + ": " + skins[i].price);
                // isDiscounted = true;
            }
        } else {
            Debug.Log("Sale on: " + regDate.AddDays(1).ToString());
        }
    }









    
    

}
