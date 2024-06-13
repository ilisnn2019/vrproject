using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zigger : Storable
{
    [Header("ParticleSystem water pouring")]
    [SerializeField]
    protected ParticleSystem particle;

    protected ParticleSystemRenderer particleRenderer;

    protected Material m_particle;

    [SerializeField]
    Transform center;

    [SerializeField]
    Transform vertex;

    float radius;

    public Mensuration MUI;


    protected override void Start()
    {
        base.Start();
        particleRenderer = particle.gameObject.GetComponent<ParticleSystemRenderer>();

        radius = Vector3.Distance(center.position, vertex.position);

        m_particle = particleRenderer.materials[0];
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (interval_time <= 0) MUI.enabled = false;

        Liquid_Pouring();
        Set_Particle_Location();

    }

    private void DyeParticle()
    {
        m_particle = particleRenderer.materials[0];
        m_particle.SetColor("_Color", liquids.GetOverallColor());
        particleRenderer.trailMaterial = m_particle;
    }

    void Set_Particle_Location()
    {
        center.LookAt(bottom);

        Vector3 local = center.localPosition;
        local.y -= radius * 0.5f;
        local.z = 0.015f;
    }

    protected void Liquid_Pouring()
    {
        float angle = Vector3.Dot(Vector3.up, bottom.transform.up.normalized);
        //DebugText.debugText.text = angle.ToString();

        if (amount > 0)
        {
            DyeParticle();

            if (angle <= 0)
            {
                particle.Play();
                amount -= 0.8f;
            }
            else if (angle > 0)
            {
                particle.Stop();
            }
        }
        else
        {
            particle.Stop();
        }
    }

    //only liquid acquired
    protected override void AddLiquid(float amount = 0.4f)
    {
        interval_time = recover_time;

        if (ispoured == false) //������ ���°� �ƴ϶��,
        {
            ispoured = true;
            if (MUI != null) MUI.enabled = true;
        }

        if (prev_liquids == null) //ó�� ������ ��ü ���
        {
            prev_liquids = input_liquids;
        }

        if(!prev_liquids.Equals(input_liquids)) //�ٸ� ��ü�� ���ý�, ���� ����
        {
            MUI.Update_Text(null, 0, "cannot mix!");
            return;
        }

        //�ϳ��� ��ü�� ����
        if (this.amount <= capacity)
        {
            liquids.AddLiquids(input_liquids, amount);

            //�跮���� ��ü��
            this.amount += amount;

            liquid_mensurating += amount;
        }

        //UI�� ���� ����
        if (MUI != null) MUI.Update_Text(input_liquids, liquid_mensurating);


    }


}
