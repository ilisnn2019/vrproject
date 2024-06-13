using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    public TextMeshProUGUI ScreenText;

    // Start is called before the first frame update
    void Start()
    {
        ScreenText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScreenText(string text, float time = 5)
    {
        StartCoroutine(TextTimer(text, time));
    }

    IEnumerator TextTimer(string text, float time)
    {
        ScreenText.text = text;
        yield return new WaitForSeconds(time);
        ScreenText.text = "";
    }
}
