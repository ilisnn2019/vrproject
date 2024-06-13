using System.Collections.Generic;
using UnityEngine;

public class Evaluation : MonoBehaviour
{
    // ������ ��ü
    Dictionary<string, float> recipe = new();

    // �ܿ� �� ��ü
    Dictionary<string, float> result = new();

    float total_result = 0;
    float total_recipe = 0; // �����ǿ� ���� ��� ��ü�� ��

    [SerializeField]
    float score = 0;

    bool iseval;

    void Start()
    {
        Init();
    }

    void Update()
    {
        // DebugText.debugText.text = score.ToString();
    }

    public void Init()
    {
        iseval = false;
        recipe = new();
        result = new();
        total_recipe = total_result = 0;
        score = 0;
    }

    public void Init_Retry()
    {
        iseval = false;
        result = new();
        score = total_result = 0;
    }

    public void SetRecipe(Recipe.Ingredient[] ingredients)
    {
        iseval = true;
        foreach (var ingredient in ingredients)
        {
            recipe.Add(ingredient.Name, ingredient.Amount);
            total_recipe += ingredient.Amount;
        }
        // Debug.Log(total_recipe);
    }

    public void EvaluationFunc()
    {
        score = 100;

        if (recipe.Count == 0) return;

        float part_score = 100f / recipe.Count;

        foreach (var pair in recipe)
        {
            string ingredientName = pair.Key;
            float recipeAmount = pair.Value;

            if (!result.ContainsKey(ingredientName))
            {
                // ����� ���� ��� �ش� �κ� ���� ��� ����
                score -= part_score;
            }
            else
            {
                float resultAmount = result[ingredientName];
                float recipeRatio = recipeAmount / total_recipe;
                float resultRatio = resultAmount / total_result;

                // ���� ���̿� ���� ����
                float ratioDifference = Mathf.Abs(recipeRatio - resultRatio);
                score -= part_score * ratioDifference;
            }
        }

        score = Mathf.Max(score, 0); // Ensure score is not negative

        UIManager.Instance.On_Evaluation();
        UIManager.Instance.On_Score_Text(score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!iseval) return;

        Debug.Log("Start Evaluating");

        other.transform.TryGetComponent(out Storable glass);

        if (glass == null) return;

        Liquids liquid = glass.Get_Liquids(); // �ܿ� �ִ� ��ü ��������

        if (liquid == null) return;

        Dictionary<Liquid, float> l_info = liquid.GetLiquids(); // ��ü ��ųʸ� ��������

        MakeDictionary(l_info);
    }

    private void MakeDictionary(Dictionary<Liquid, float> l_info)
    {
        iseval = false;

        foreach (var pair in l_info) // ��ü ������ ��ȸ
        {
            if (result.ContainsKey(pair.Key.liquid_name))
                result[pair.Key.liquid_name] += pair.Value;
            else
                result.Add(pair.Key.liquid_name, pair.Value);
        }

        total_result = 0;
        foreach (var pair in result)
        {
            total_result += pair.Value;
        }

        Debug_Result();

        EvaluationFunc();


    }

    void Debug_Result()
    {
        string str = "";
        foreach (var pair in result)
        {
            str += pair.Key;
            str += " " + pair.Value.ToString();
            str += "\n";
        }

        Debug.Log(str);
    }
}
