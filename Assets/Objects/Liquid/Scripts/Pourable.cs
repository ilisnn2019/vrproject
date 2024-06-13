using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pourable : LiquidContainer
{
    public float Threshold_Angle;

    [Header("ParticleSystem water pouring")]
    [SerializeField]
    protected ParticleSystem particle;

    protected ParticleSystemRenderer particleRenderer;

    protected Material m_particle;

    public bool IsCapoff;

    public GameObject remove_cap;

    public AudioClip sound;
    AudioSource audiosource;

    protected override void Start()
    {
        particleRenderer = particle.gameObject.GetComponent<ParticleSystemRenderer>();

        m_particle = particleRenderer.materials[0];

        audiosource = GetComponent<AudioSource>();

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(IsCapoff) Liquid_Pouring(); //따를 수 있는 상태
    }


    private void DyeParticle()
    {
        m_particle = particleRenderer.materials[0];
        m_particle.SetColor("_Color", liquids.GetOverallColor());
        particleRenderer.trailMaterial = m_particle;
    }


    protected void Liquid_Pouring()
    {
        float angle = Vector3.Dot(Vector3.up, bottom.transform.up);
        

        if (angle <= Threshold_Angle)
        {
            OnParticlePlay();
        }
        else if (angle > Threshold_Angle)
        {
            OnParticleStop();
        }

        DyeParticle();
    }

    public void Capoff()
    {
        if (audiosource != null) audiosource.Play();
        IsCapoff = true;
        remove_cap.SetActive(false);
    }

    public void OnParticlePlay()
    {
        particle.Play();
    }

    public void OnParticleStop()
    {
        particle.Stop();
    }
}

