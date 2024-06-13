using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storable : LiquidContainer
{
    [Header("Variables for Storable Container")]
    protected bool ispoured = false;

    protected float recover_time = 1.5f;
    protected float interval_time = 0;

    protected Liquids input_liquids; //들어오는 액체

    protected Liquids prev_liquids;

    public AudioClip storingclip;

    public AudioClip washingclip;

    protected float liquid_mensurating;


    [SerializeField]
    GameObject ice;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (interval_time > 0)
        {
            interval_time -= Time.deltaTime;
        }
        else
        {
            input_liquids = null;
            ispoured = false;
        }

        if (amount <= 0)
        {
            liquid_mensurating = 0;
            prev_liquids = null;
            input_liquids = null;
            liquids.Clear();
        }
    }

    protected void OnParticleCollision(GameObject other)
    {
        if (!other.transform.parent.TryGetComponent(out LiquidContainer container))
        {
            return;
        }
        input_liquids = container.Get_Liquids();
        AddLiquid();
    }

    protected virtual void AddLiquid(float amount = 0.4f)
    {
        if (input_liquids == null) return;

        interval_time = recover_time;

        if (ispoured == false) //따르는 상태가 아니라면,
        {
            ispoured = true;
        }

        if (this.amount <= capacity)
        {
            liquids.AddLiquids(input_liquids, amount);

            //계량중인 액체양
            this.amount += amount;

            soundfunction.ClipPlay(storingclip);
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("sink"))
        {
            if (washingclip == null) return;
            soundfunction.ClipPlay(washingclip);
            amount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ice == null) return;
        if (other.CompareTag("ice"))
        {
            if (clips[0] != null) soundfunction.ClipPlay(clips[0]);

            if (ice.activeSelf == false)
            {
                ice.SetActive(true);
            }
        }
    }
}

