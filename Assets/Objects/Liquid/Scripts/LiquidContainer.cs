using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LC_TYPE { BOTH, POURABLE, STORABLE }

public class LiquidInfo
{
    readonly Liquid liquidType;
    private float amount;

    public LiquidInfo(Liquid type, float amount)
    {
        liquidType = type;
        this.amount = amount;
    }

    public void AddLiquid(float amount)
    {
        this.amount += amount;
    }
    public Color GetColor()
    {
        return liquidType.l_color;
    }
    public float GetAmount()
    {
        return amount;
    }
}

[RequireComponent(typeof(Liquid))]
public class LiquidContainer : MonoBehaviour
{

    [Header("Renderer of Object hold Liquid Material")]
    [SerializeField]
    protected MeshRenderer rend;
    // Start is called before the first frame update

    [SerializeField]
    protected Vector3 boundSize;

    protected Material liquidMaterial;

    [Header("scaler for ratio")]
    [SerializeField]
    protected float scale;

    [Header("Capacity of how much liquid the container holds")]
    [SerializeField]
    protected float capacity = 0;

    [Header("Amount of how much liquid in container now")]
    [SerializeField]
    protected float amount = 0;

    [SerializeField]
    protected float angle = 0;
    protected float resultAmount;

    [Header("Liquid in Bottle")]
    [SerializeField]
    protected Liquid liquidInBottle;

    [SerializeField]
    Transform bottom;

    protected virtual void Start()
    {
        liquidMaterial = rend.material;
        boundSize = rend.bounds.size;
        liquidInBottle = GetComponent<Liquid>();


        Color color = liquidInBottle.l_color;
        liquidMaterial.SetColor("_Color", color);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Liquid_Calculate();

        liquidMaterial.SetColor("_Color", liquidInBottle.l_color);
    }

    protected void Liquid_Calculate()
    {
        float ratio = amount > capacity ? 1 : (amount / capacity);

        // x-z 평면과 이루는 각도를 구함
        angle = Vector3.Dot(Vector3.up, bottom.transform.up.normalized);


        resultAmount = angle >= 0 ? angle * boundSize.y  *  ratio  * scale : angle * boundSize.y * (1 - ratio) * scale;

        liquidMaterial.SetFloat("_FillAmount", resultAmount + bottom.position.y);
    }
}
