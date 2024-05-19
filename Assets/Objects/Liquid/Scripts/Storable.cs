using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storable : LiquidContainer
{
    protected Dictionary<Liquid, LiquidInfo> liquids = new();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void OnParticleCollision(GameObject other)
    {
        AddLiquid(other);
    }

    protected void AddLiquid(GameObject other, float amount = 0.1f)
    {
        
        other.transform.parent.TryGetComponent(out Liquid liquid);

        if (liquid == null)
        {
            return;
        }

        LiquidInfo value;

        if (!liquids.ContainsKey(liquid)) //이미 있는 액체
        {
            liquids.Add(liquid, new LiquidInfo(liquid, amount));
        }

        liquids.TryGetValue(liquid, out value);
        value.AddLiquid(amount);

        this.amount += amount;

        MixColor();
    }

    private void MixColor()
    {
        Color resultColor = Color.white;
        foreach (LiquidInfo info in liquids.Values)
        {
            resultColor = Color.Lerp(resultColor, info.GetColor(), 0.5f);
        }
        resultColor.a = 1;

        liquidInBottle.l_color = resultColor;

    }
}
