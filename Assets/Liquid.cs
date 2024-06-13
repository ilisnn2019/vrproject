using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Liquid : Ingredient
{ 
    Color liquid_color;
    public string liquid_name;

    public bool alcohol;

    

    public void SetColor(Color color)
    {
        liquid_color = color;
    }

    public Color GetColor() {
        return liquid_color;
    }

}
    

