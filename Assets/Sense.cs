using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    bool is_sniffing;


    public ScreenUI screenui;


    // Start is called before the first frame update
    void Start()
    {
        is_sniffing = false;
        RenderSettings.fog = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("glass") && !is_sniffing)
        {
            is_sniffing = true;

            other.TryGetComponent(out Liquids liquids); //liquids 정보를 가져옴

            AlcoholState state = liquids.GetAlcoholState();

            string state_str = "";
            if (state == AlcoholState.MostlyAlcohol) state_str = "The smell of alcohol is strong.";
            else if (state == AlcoholState.MostlyNonAlcohol) state_str = "There is almost no smell of alcohol.";
            else if (state == AlcoholState.NearlyHalfAndHalf) state_str = "The smell of alcohol is moderate.";
            else state_str = "There is no smell at all.";


            screenui.SetScreenText(state_str);

            GetComponent<AudioSource>().Play();

            RenderSettings.fog = true;

            StartCoroutine(Sniffing(liquids.GetOverallColor()));
        }

    }

    IEnumerator Sniffing(Color RkFogColor)
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = RkFogColor;
        yield return new WaitForSeconds(5f);
        RenderSettings.fog = false;
        is_sniffing = false;
    }
}
