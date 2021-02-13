using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Globalization;

public class LogsScene : MonoBehaviour
{
    private Player gs;
    private GameObject go;

    DateTime today = DateTime.Today;
    DateTime current;
    
    public DailyIntake Log;
    public Text dateText;

    public GameObject breakfast;
    public Transform breakfastHolder;
    GameObject bfast;
    public GameObject[] mealCat;
    
    public Text TEAIntake;
    public Text CHOIntake;
    public Text FATIntake;
    public Text PROIntake;

    public BarSlider calSlider;
    public BarSlider choSlider;
    public BarSlider proSlider;
    public BarSlider fatsSlider;
    
    public Dropdown drop;
    public RectTransform _rootRectTransform;
    
    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
    
    string meal;
    

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

        current = today;
        drop.value = 0;
        meal = "Breakfast";
        int index = gs.dailyFoodIntake.FindIndex(a => a.dateLogged.Equals(current.ToString()));

        if (index >= 0) {
            Log = gs.dailyFoodIntake[index];        
            DisplayFoodLogs();
        } else {
            Debug.Log("No logs.");
        }

    }

    // Update is called once per frame
    void Update() 
    {
        dateText.text = current.ToString("dd MMM yyyy").ToUpper();

        
    }

    public void OnBackClick() {
        SceneManager.LoadScene(3);
    }

    public void DecreaseDate() {
        DateTime firstDay = Convert.ToDateTime(PlayerPrefs.GetString("isRegisteredKeyName"));

        if (current <= firstDay) {

        } else {
            current = current.AddDays(-1);
        }
        int index = gs.dailyFoodIntake.FindIndex(a => a.dateLogged.Equals(current.ToString()));
        
        if (index >= 0) {
            Log = gs.dailyFoodIntake[index];
            ReplaceFoodLogs();
            DisplayFoodLogs();
        } else {
            ReplaceFoodLogs();
        }

        
    }

    public void IncreaseDate() {
        if (current >= today) {

        } else {
            current = current.AddDays(1);
        }

        int index = gs.dailyFoodIntake.FindIndex(a => a.dateLogged.Equals(current.ToString()));

        if (index >= 0) {
            // Debug.Log(index);
            Log = gs.dailyFoodIntake[index];
            ReplaceFoodLogs();
            DisplayFoodLogs();
        } else {
            ReplaceFoodLogs();
        }
        
    }

    public void MealDropDown(int val) {
        if (val == 0) {
            meal = "Breakfast";
        } else if (val == 1) {
            meal = "Lunch";
        } else if (val == 2) {
            meal = "Snack";
        } else if (val == 3) {
            meal = "Dinner";
        } 
        ReplaceMealCategory(val);
        DisplayFoodLogs();
    }

    public void DisplayFoodLogs() {
        TEAIntake.text = Log.TEAIntake + "kcal/ \n" + Log.TEA + "kcal";
        CHOIntake.text = Log.CHOIntake + "g/ \n" + Log.CHO + "g";
        FATIntake.text = Log.FATIntake + "g/ \n" + Log.FAT + "g";
        PROIntake.text = Log.PROIntake + "g/ \n" + Log.PRO + "g";
        calSlider.SetMaxValue(Log.TEA);
        calSlider.SetCurrentVal(Log.TEAIntake);
        choSlider.SetMaxValue(Log.CHO);
        choSlider.SetCurrentVal(Log.CHOIntake);
        proSlider.SetMaxValue(Log.PRO);
        proSlider.SetCurrentVal(Log.PROIntake);
        fatsSlider.SetMaxValue(Log.FAT);
        fatsSlider.SetCurrentVal(Log.FATIntake);

        foreach(var foodLog in Log.foodLogsForTheDay) {
            if (foodLog.mealCategory.Equals(meal)) {
                bfast = Instantiate(breakfast, breakfastHolder);
                bfast.transform.GetChild(0).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.foodName);
                bfast.transform.GetChild(1).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.serving.ToString());
                bfast.transform.GetChild(2).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.calories.ToString()) + " kcal";
            } 
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rootRectTransform);
    }

    public void ReplaceFoodLogs() {
        TEAIntake.text = "0kcal/ \n" + "0kcal";
        CHOIntake.text = "0g/ \n" + "0g";
        FATIntake.text = "0g/ \n" + "0g";
        PROIntake.text = "0g/ \n" + "0g";
        calSlider.SetMaxValue(0f);
        calSlider.SetCurrentVal(0f);
        choSlider.SetMaxValue(0f);
        choSlider.SetCurrentVal(0f);
        proSlider.SetMaxValue(0f);
        proSlider.SetCurrentVal(0f);
        fatsSlider.SetMaxValue(0f);
        fatsSlider.SetCurrentVal(0f);
        var clones = GameObject.FindGameObjectsWithTag("item");
        foreach (var clone in clones){
            Destroy(clone);
        }
        
    }

    public void ReplaceMealCategory(int temp) {
        var clones = GameObject.FindGameObjectsWithTag("item");
        foreach (var clone in clones){
            Destroy(clone);
        }
        for (int i=0; i < 4; i++) {
            if (temp == i) {
                mealCat[i].SetActive(true);
            } else {
                mealCat[i].SetActive(false);
            }
        }
    }


}

