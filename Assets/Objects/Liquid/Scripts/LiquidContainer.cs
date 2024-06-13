using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoundFunction))]
[RequireComponent(typeof(Liquids))]
public class LiquidContainer : MonoBehaviour
{
    [Header("Variables for Liquid Container")]

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
    protected Liquids liquids;

    [SerializeField]
    protected Transform bottom;


    public List<AudioClip> clips;

    protected SoundFunction soundfunction;

    protected virtual void Start()
    {
        liquids = GetComponent<Liquids>();

        liquidMaterial = rend.material;
        boundSize = rend.bounds.size;

        soundfunction = GetComponent<SoundFunction>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Liquid_Calculate();
        liquidMaterial.SetColor("_Color", liquids.GetOverallColor());

    }

    protected virtual void Liquid_Calculate()
    {
        if (amount <= 0)
        {

            liquidMaterial.SetFloat("_FillAmount", -100) ;
        }
        else if(amount >= capacity)
        {
            liquidMaterial.SetFloat("_FillAmount", 100);
        }
        else
        {
            float ratio = amount > capacity ? 1 : (amount / capacity);

            // x-z 평면과 이루는 각도를 구함
            angle = Vector3.Dot(Vector3.up, bottom.transform.up.normalized);

            resultAmount = angle >= 0 ? angle * boundSize.y * ratio * scale : angle * boundSize.y * (1 - ratio) * scale;

            liquidMaterial.SetFloat("_FillAmount", resultAmount + bottom.position.y);
        }
    }

    public void Set_Liquids(Liquids liquid)
    {
        liquids = liquid;
        liquidMaterial.SetColor("_Color", liquids.GetOverallColor());
    }

    public Liquids Get_Liquids()
    {
        if (liquids == null) return null;
        else return liquids;
    }
}
