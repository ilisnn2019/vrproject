using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pourable : LiquidContainer
{
    [Header("ParticleSystem water pouring")]
    [SerializeField]
    protected ParticleSystem particle;

    protected ParticleSystemRenderer particleRenderer;

    protected Material m_particle;

    protected Material m_particletail;

    protected override void Start()
    {
        particleRenderer = particle.gameObject.GetComponent<ParticleSystemRenderer>();

        base.Start();
    }

    public void Set_Color(Color color)
    {
        liquidInBottle.l_color = color;
    }

    // Update is called once per frame
    protected override void Update()
    {
        m_particle = particleRenderer.material;
        m_particletail = particleRenderer.trailMaterial;

        Color color = liquidInBottle.l_color;

        m_particle.SetColor("_Color", color);
        m_particletail.SetColor("_Color", color);

        base.Update();
        Liquid_Pouring();

    }

    protected void Liquid_Pouring()
    {
        float angle = Vector3.Dot(Vector3.up, transform.up);
        //DebugText.debugText.text = angle.ToString();

        if (angle <= 0)
        {

            particle.Play();
        }
        else if (angle > 0)
        {
            particle.Stop();
        }
    }

    public Liquid Get_Liquid()
    {
        if (liquidInBottle == null) return null;
        else return liquidInBottle;
    }
}

