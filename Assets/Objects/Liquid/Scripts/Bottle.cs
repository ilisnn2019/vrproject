using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Liquid))]
public class Bottle : Pourable
{
    Liquid origin_Liquid;

    protected override void Start()
    {
        base.Start();

        Color color = liquidMaterial.GetColor("_Color");

        origin_Liquid = GetComponent<Liquid>();

        origin_Liquid.SetColor(color);

        liquids.AddLiquid(origin_Liquid, 100);


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
