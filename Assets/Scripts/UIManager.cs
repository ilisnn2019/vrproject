using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
        set { }
    }


    [Header("Variants of Menu UI")]
    [SerializeField] GameObject c_Menu;

    [Space]Transform content;
    [SerializeField] GameObject s_recipe;
    [SerializeField] Transform detailview;

    [SerializeField] TextMeshProUGUI t_mname;
    [SerializeField] TextMeshProUGUI t_mdescription;

    [Header("Variants of Recipe UI")]
    [SerializeField] GameObject c_Recipe;

    [SerializeField] TextMeshProUGUI t_rname;
    [SerializeField] TextMeshProUGUI t_ringredient;
    [SerializeField] TextMeshProUGUI t_rrecipe;

    [Header("Variants of Evaluation UI")]
    [SerializeField] GameObject c_Eval;
    [SerializeField] GameObject c_Retry;
    [SerializeField] GameObject c_Return;
    [SerializeField] TextMeshProUGUI t_score;

    Recipe cur_recipe;

    public Evaluation eval;

    public List<Recovery> recoverys = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(this);


        content = GameObject.Find("menu_content").transform;

        detailview = GameObject.Find("Detail View").transform;

        t_rname = c_Recipe.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        t_ringredient = c_Recipe.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        t_rrecipe = c_Recipe.transform.GetChild(1).GetChild(4).GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();


        t_mname = detailview.GetChild(0).GetComponent<TextMeshProUGUI>();
        t_mdescription = detailview.GetChild(1).GetComponent<TextMeshProUGUI>();

        Init_Recipe_Picker();
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init_Recipe_Picker()
    {
        c_Menu.SetActive(true);
        c_Recipe.SetActive(false);
    }

    public void Generate_Recipe_Slot(List<Recipe> r_list)
    {
        cur_recipe = null;
        t_mname.text = "";
        t_mdescription.text = "";

        foreach (var recipe in r_list)
        {
            Instantiate(s_recipe, content).GetComponent<RecipeSlot>().Recipe = recipe;
        }
    }

    public void Choose_Recipe(Recipe r)
    {
        cur_recipe = r;

        if (t_mname !=null && t_mdescription != null)
        {
            //Debug.Log(cur_recipe.Name);
            t_mname.text = cur_recipe.Name;
            t_mdescription.text = cur_recipe.Description;
        }
        else
        {
            Debug.Log("text not assigned");
        }


    }
    public void Pick_Recipe()
    {
        if (cur_recipe == null) return;

        c_Menu.SetActive(false);
        c_Recipe.SetActive(true);

        t_rname.text = cur_recipe.Name;
        t_ringredient.text = cur_recipe.Ingredients_Tostring();
        t_rrecipe.text = cur_recipe.RecipeInstructions;

        eval.SetRecipe(cur_recipe.Ingredients);

        foreach(var r in recoverys)
        {
            r.OnRecovery();
        }
        c_Eval.SetActive(true);
    }

    public void On_Return_Btn()
    {
        Off_Score_Text();
        Init_Recipe_Picker();
        eval.Init();
        c_Retry.SetActive(false);
        c_Return.SetActive(false);

    }

    public void On_Retry_Btn()
    {
        Off_Score_Text();
        eval.Init_Retry();
        c_Retry.SetActive(false);
        c_Return.SetActive(false);

        eval.SetRecipe(cur_recipe.Ingredients);

        foreach (var r in recoverys)
        {
            r.OnRecovery();
        }
        c_Eval.SetActive(true);

    }
    public void On_Evaluation()
    {
        c_Retry.SetActive(true);
        c_Return.SetActive(true);
    }

    public void On_Score_Text(float score)
    {
        t_score.enabled = true;
        t_score.text = score.ToString("F1");
    }
    private void Off_Score_Text()
    {
        t_score.enabled = false;
        t_score.text = "";
    }
}
