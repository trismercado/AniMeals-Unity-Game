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
    public GameObject lunch;
    public Transform lunchHolder;
    public GameObject snack;
    public Transform snackHolder;
    public GameObject dinner;
    public Transform dinnerHolder;

    public Text TEAIntake;
    public Text CHOIntake;
    public Text FATIntake;
    public Text PROIntake;

    public RectTransform _rootRectTransform;
    public VerticalLayoutGroup vlg;

    public BarSlider calSlider;
    public BarSlider choSlider;
    public BarSlider proSlider;
    public BarSlider fatsSlider;

    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

    GameObject bfast;
    GameObject lnch;
    GameObject snck;
    GameObject dnnr;

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
        current = current.AddDays(-1);

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
            if (foodLog.mealCategory.Equals("Breakfast")) {
                bfast = Instantiate(breakfast, breakfastHolder);
                bfast.transform.GetChild(0).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.foodName);
                bfast.transform.GetChild(1).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.serving.ToString());
                bfast.transform.GetChild(2).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.calories.ToString()) + " kcal";
            } else if (foodLog.mealCategory.Equals("Lunch")) {
                lnch = Instantiate(lunch, lunchHolder);
                lnch.transform.GetChild(0).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.foodName); 
                lnch.transform.GetChild(1).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.serving.ToString());
                lnch.transform.GetChild(2).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.calories.ToString()) + " kcal";
            } else if (foodLog.mealCategory.Equals("Snack")) {
                snck =Instantiate(snack, snackHolder);
                snck.transform.GetChild(0).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.foodName);
                snck.transform.GetChild(1).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.serving.ToString());
                snck.transform.GetChild(2).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.calories.ToString()) + " kcal";
            } else if (foodLog.mealCategory.Equals("Dinner")) {
                dnnr = Instantiate(dinner, dinnerHolder);
                dnnr.transform.GetChild(0).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.foodName);
                dnnr.transform.GetChild(1).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.serving.ToString());
                dnnr.transform.GetChild(2).GetComponent<Text>().text = textInfo.ToTitleCase(foodLog.food.calories.ToString()) + " kcal";
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

}

