using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mensuration : MonoBehaviour
{
    Transform player;

    [SerializeField]
    TextMeshProUGUI liquid_name_t;
    [SerializeField]
    TextMeshProUGUI liquid_amount_t;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("MainCamera").transform;
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {


        transform.LookAt(player);
    }

    public void Update_Text(Liquids liquid, float amount, string msg = null)
    {
        if (msg != null)
        {
            liquid_name_t.text = msg;
            liquid_amount_t.text = null;
        }
        else
        {
            liquid_name_t.text = liquid.GetLiquid().liquid_name;
            liquid_amount_t.text = amount.ToString("F1");
        }
    }

    private void OnEnable()
    {
        canvas.enabled = true;
    }

    private void OnDisable()
    {
        canvas.enabled = false;
    }
}
