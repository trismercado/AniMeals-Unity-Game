using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class FoodLogScene : MonoBehaviour
{
    public Text servingText;
    float serving;

    public Text gramPerServingText;

    public Text calText;
    float cal;

    public Text carbsText;
    float carbs;

    public Text proteinText;
    float protein;

    public Text fatText;
    float fat;

    public Text serveText;
    float grams;

    public bool isManual;
    public Button suggestManual; 
    public GameObject inputServe;
    public GameObject inputCal;
    public GameObject inputCarbs;
    public GameObject inputPro;
    public GameObject inputFat;
    InputField serveField;
    InputField calField;
    InputField carbsField;
    InputField proField;
    InputField fatField;

    int id;
    string foodName;
    float pieces;
    
    bool needManualInput;

    private Player gs;
    private GameObject go;
    private FoodLogPopUp pop;

    public Dropdown drop;

    //FoodList
    private const string DATABASE_NAME = @"ResourceLoader/foodDatabase.json";
    public InputField inputFoodName;
    public List<string> suggestionList;

    public Button saveBtn;
    private string display;

    [SerializeField]
    public List<FoodData> foodList = new List<FoodData>();
    
    //Food Intake
    public FoodData foodLog = new FoodData();
    public FoodLog log = new FoodLog(); //with meal category of breakfast, lunch, snack, dinner    

    Color32 rewardcolor = new Color32(0x3F, 0xC8, 0x8D, 0xFF); 



    // Start is called before the first frame update
    void Start () {
        serving = 1f;
        cal = 0f;
        carbs = 0f;
        protein = 0f;
        fat = 0f;
        GameObject jsonload = GameObject.Find("JsonLoader");
        DatabaseLoader list = jsonload.GetComponent<DatabaseLoader>();
        foodList = list.ReturnDatabase<FoodData>(DATABASE_NAME);
        
        go = GameObject.Find("Player");
        if (go == null) {
            Debug.LogError("No player gameobject");
            this.enabled = false;
            return;
        }
        gs = go.GetComponent<Player>();

        go = GameObject.Find("FoodLogPopUp");
            if (go == null) {
                Debug.LogError("No food log pop up gameobject");
                this.enabled = false;
                return;
            }
        pop = go.GetComponent<FoodLogPopUp>();

        drop.value = 0;
    }

    // Update is called once per frame
    void Update () {
        servingText.text = serving.ToString();  // make it a string to output to the Text object     
        gramPerServingText.text = grams.ToString() + " grams per serving";
        calText.text = (cal * serving).ToString() + "kcal";
        carbsText.text = (carbs * serving).ToString() + "g";
        proteinText.text = (protein * serving).ToString() + "g";
        fatText.text = (fat * serving).ToString() + "g";
        serveText.text = (grams * serving).ToString() + "g";
        SearchFood();

        CheckForManual();
        if (isManual) {
            calField = inputCal.GetComponent<InputField>();
            serveField = inputServe.GetComponent<InputField>();
            carbsField = inputCarbs.GetComponent<InputField>();
            proField = inputPro.GetComponent<InputField>();
            fatField = inputFat.GetComponent<InputField>();
            ManualInput();
        }
        
        saveBtn.interactable = CheckEntries();
        // ShowSuggestions();
    }

    public void OnBackClick(){
        Debug.Log("Back was clicked!");
        SceneManager.LoadScene(3);

    }

    public void OnAddClick() {
        // Debug.Log("Add was clicked!");
        serving = serving + 0.5f;
    }

    public void OnMinusClick() {
        // Debug.Log("Minus was clicked!");
        if (serving > 1) 
            serving = serving - 0.5f;
    }

    public void OnInfoClick() {
        Action action = () => {
        };
        string saysmtng = "To log in your food:" + "\n" + "1.  Input the food you just ate." + "\n" + "2.  If information about the food does not show, you can manually enter them by pressing the Manual button" + "\n\n" + "It's very good to be aware of the food we eat. So, let's eat well!";
        pop.CenterPopUp("Pedro:", saysmtng, "OK", rewardcolor, action);
    }

    public void OnSaveClick() {
        if (CheckEntries()) {
            CreateFoodLog();
            log.food = foodLog;
            gs.AddToIntake(cal*serving, carbs*serving, protein*serving, fat*serving, log);
            gs.SaveData();
            SceneManager.LoadScene(3); 
        }        
    }


    public void MealDropDown(int val) {
        if (val == 0) {
            log.mealCategory = "Breakfast";
        } else if (val == 1) {
            log.mealCategory = "Lunch";
        } else if (val == 2) {
            log.mealCategory = "Snack";
        } else if (val == 3) {
            log.mealCategory = "Dinner";
        } 
        // Debug.Log(val);
    }

    public void SearchFood() {
        if (inputFoodName.text != "") {
            foreach(var food in foodList)
            {
                if (inputFoodName.text.ToLower().Equals(food.foodName.ToLower())){
                    suggestionList.Clear();   
                    id = food.id;
                    foodName = food.foodName;
                    pieces = food.pieces;
                    cal = food.calories;
                    carbs = food.carbs;
                    protein = food.proteins;
                    fat = food.fats;
                    grams = food.grams;
                    suggestManual.gameObject.SetActive(false);
                    isManual = false;
                    needManualInput = false;
                    break;
                } else if (food.foodName.ToLower().StartsWith(inputFoodName.text.ToLower())) {
                    if (!suggestionList.Contains(food.foodName.ToLower())) {
                        suggestionList.Add(food.foodName.ToLower());
                    }
                    suggestionList.RemoveAll(s => !s.StartsWith(inputFoodName.text.ToLower()));   
                    id = 0;
                    foodName = "";
                    pieces = 0;
                    cal = 0f;
                    carbs = 0f;
                    protein = 0f;
                    fat = 0f;
                    grams = 0f;
                    suggestManual.gameObject.SetActive(true);
                    needManualInput = true;
                } else {
                    suggestionList.RemoveAll(s => !s.StartsWith(inputFoodName.text.ToLower()));     
                    id = 0;
                    foodName = "";
                    pieces = 0;
                    cal = 0f;
                    carbs = 0f;
                    protein = 0f;
                    fat = 0f;
                    grams = 0f;
                    suggestManual.gameObject.SetActive(true);
                    needManualInput = true;
                }
            }
        } else {
            suggestionList.Clear();
        }
    }

    public void ToggleManual() {
        isManual = !isManual;
    }

    public void CheckForManual() {
        inputServe.SetActive(isManual);
        inputCal.SetActive(isManual);
        inputCarbs.SetActive(isManual);
        inputPro.SetActive(isManual);
        inputFat.SetActive(isManual);
    }

    public void ManualInput() {
        if (calField.text != "" && serveField.text != "" && carbsField.text != "" && fatField.text != "" && proField.text != "") {
            id = 0;
            pieces = 0;
            foodName = inputFoodName.text.ToLower();
            grams = float.Parse(serveField.text);
            cal = float.Parse(calField.text);
            carbs = float.Parse(carbsField.text);
            fat = float.Parse(fatField.text);
            protein = float.Parse(proField.text);
        }
    }

    public bool CheckEntries() {
        if (isManual) {
            if ((inputFoodName.text != "") && grams > 0f && cal > 0f && carbs >= 0f && fat >= 0f && protein >= 0f) return true;
            else return false;
        } else {
            if (inputFoodName.text != "" && !needManualInput) return true;
            else return false;
        }
    }

    public void CreateFoodLog() {
        foodLog.id = id;
        foodLog.foodName = foodName;
        foodLog.serving = serving;
        foodLog.pieces = pieces*serving;
        foodLog.grams = grams*serving;
        foodLog.calories = cal*serving;
        foodLog.carbs = carbs*serving;
        foodLog.fats = fat*serving;
        foodLog.proteins = protein*serving;
    }



    
}
