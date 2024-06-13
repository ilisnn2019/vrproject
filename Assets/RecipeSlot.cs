using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeSlot : MonoBehaviour
{
    private Recipe recipe;
    TextMeshProUGUI t_name;

    public Recipe Recipe { get => recipe; set => recipe = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t_name = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if(Recipe!=null)
            t_name.text = Recipe.Name;
    }

    public void Choose_Recipe()
    {
        UIManager.Instance.Choose_Recipe(recipe);
    }
}
