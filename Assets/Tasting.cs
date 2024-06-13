using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasting : MonoBehaviour
{
    private bool is_tasting;

    public ScreenUI screenui;

    // Start is called before the first frame update
    void Start()
    {
        is_tasting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("glass") && !is_tasting)
        {
            is_tasting = true;

            other.TryGetComponent(out Liquids liquids); //liquids 정보를 가져옴

            GetComponent<AudioSource>().Play();


            AlcoholState state = liquids.GetAlcoholState();

            string state_str = "";
            if (state == AlcoholState.MostlyAlcohol) state_str = "The taste of alcohol is strong. It feels like you might get drunk.";
            else if (state == AlcoholState.MostlyNonAlcohol) state_str = "It tastes like a beverage instead of alcohol.";
            else if (state == AlcoholState.NearlyHalfAndHalf) state_str = "It makes you feel pleasantly balanced.";
            else state_str = "It tastes like nothing.";

            screenui.SetScreenText(state_str);

            if (state == AlcoholState.MostlyAlcohol)
            {
                StartCoroutine(DrinkingEffect());
            }

            is_tasting = false;
        }

    }

    IEnumerator DrinkingEffect()
    {
        int remember_frame = Application.targetFrameRate;

        Application.targetFrameRate = 1;
        yield return new WaitForSeconds(5f);

        Application.targetFrameRate = remember_frame;
    }

}
