using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Recipe
{
    public int id;
    public string Name;
    public Ingredient[] Ingredients;
    public string Description;
    public string RecipeInstructions;
    [Serializable]
    public class Ingredient
    {
        public string Name;
        public float Amount;
    }

    // Default constructor
    public Recipe()
    {
        Name = "";
        Ingredients = new Ingredient[0];
        Description = "";
        RecipeInstructions = "";
    }

    // Parameterized constructor
    public Recipe(string name, Ingredient[] ingredients, string description, string recipe)
    {
        Name = name;
        Ingredients = ingredients;
        Description = description;
        RecipeInstructions = recipe;
    }

    // Override ToString method to provide a meaningful string representation
    public override string ToString()
    {
        return $"ID : {id}\nRecipe Name: {Name}\nDescription: {Description}\nIngredients: {string.Join(", ", Ingredients.Select(i => $"{i.Name}: {i.Amount}"))}\nRecipe Instructions: {RecipeInstructions}";
    }

    public string Ingredients_Tostring()
    {
        string str = "";
        foreach (var pair in Ingredients)
        {
            str += pair.Name;
            if (pair.Amount != 0)
            {
                str += " : ";
                str += pair.Amount;
            }
            str += "\n";
        }
        return str;
    }
}
public class Program
{
    [SerializeField] private string jsonFileName = "Recipes/recipes"; // name of the JSON file without extension



    public static List<Recipe> LoadRecipesFromResources()
    {
        List<Recipe> recipes = new List<Recipe>();
        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Recipes/recipes");
            if (jsonFile != null)
            {
                string jsonContent = jsonFile.text;
                Recipe[] recipeArray = JsonUtility.FromJson<RecipeArrayWrapper>(jsonContent).Recipes;
                recipes = new List<Recipe>(recipeArray);
            }
            else
            {
                Debug.LogError("Could not find JSON file in Resources.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while reading or parsing the file: {ex.Message}");
        }
        return recipes;
    }

    [Serializable]
    private class RecipeArrayWrapper
    {
        public Recipe[] Recipes;
    }
}

public class RecipeManager : MonoBehaviour
{
    private static RecipeManager instance;
    public static RecipeManager Instance
    {
        get { return instance; }
        set { }
    }


    // Start is called before the first frame update
    List<Recipe> recipes = new();

    [SerializeField] UIManager menuUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(this);
    }

    void Start()
    {
        recipes = Program.LoadRecipesFromResources();

        int i = 0;

        foreach (var recipe in recipes)
        {
            recipe.id = i++;
        }

        menuUI.Generate_Recipe_Slot(recipes);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public Recipe Find_Recipe_By_Index(int i)
    {
        return recipes[i];
    }
}
