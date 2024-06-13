using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum AlcoholState
{
    None,
    MostlyAlcohol,
    MostlyNonAlcohol,
    NearlyHalfAndHalf
}
public class Liquids : MonoBehaviour
{
    private Dictionary<Liquid, float> liquidsDict;

    private void Awake()
    {
        liquidsDict = new Dictionary<Liquid, float>();
    }

    public Dictionary<Liquid, float> GetLiquids()
    {
        return liquidsDict;
    }

    public Liquid GetLiquid()
    {
        return liquidsDict.Keys.ToList()[0]; // only use Zigger
    }

    public bool IsEmpty()
    {
        return liquidsDict.Count == 0;
    }

    public void Clear()
    {
        liquidsDict.Clear();
    }

    public void AddLiquid(Liquid liquid, float amount)
    {
        if (liquidsDict.ContainsKey(liquid))
        {
            liquidsDict[liquid] += amount;
        }
        else
        {
            liquidsDict.Add(liquid, amount);
        }
    }

    public void AddLiquids(Liquids otherLiquids, float newAmount)
    {
        float totalAmount = 0;
        foreach (var item in otherLiquids.GetLiquids())
        {
            totalAmount += item.Value;
        }

        var otherLiquidsList = otherLiquids.GetLiquids().ToList();
        for (int i = 0; i < otherLiquidsList.Count; i++)
        {
            var item = otherLiquidsList[i];
            float ratio = item.Value / totalAmount;
            float addedAmount = newAmount * ratio;
            AddLiquid(item.Key, addedAmount);
        }
    }

    public Color GetOverallColor()
    {
        Color overallColor = new Color(1, 1, 1, 1);

        if (IsEmpty())
            return overallColor;

        float totalAmount = 0;

        foreach (var item in liquidsDict)
        {
            overallColor += item.Key.GetColor() * item.Value;
            totalAmount += item.Value;
        }

        if (totalAmount > 0)
        {
            overallColor /= totalAmount;
        }

        return overallColor;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in liquidsDict)
        {
            sb.AppendLine($"{item.Key.liquid_name}: {item.Value} ml");
        }
        return sb.ToString();
    }

    // 새로운 메서드 추가
    public AlcoholState GetAlcoholState()
    {
        float totalAmount = liquidsDict.Values.Sum();
        if (totalAmount == 0)
            return AlcoholState.None; // 비어있는 경우, 비알콜로 간주

        float alcoholAmount = 0;

        foreach (var item in liquidsDict)
        {
            if (item.Key.alcohol)
            {
                alcoholAmount += item.Value;
            }
        }

        float alcoholRatio = alcoholAmount / totalAmount;

        if (alcoholRatio > 0.6f)
        {
            return AlcoholState.MostlyAlcohol;
        }
        else if (alcoholRatio < 0.4f)
        {
            return AlcoholState.MostlyNonAlcohol;
        }
        else
        {
            return AlcoholState.NearlyHalfAndHalf;
        }
    }
}
