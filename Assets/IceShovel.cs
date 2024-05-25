using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShovel : MonoBehaviour
{
    [SerializeField]
    private Transform ice_output;

    [SerializeField]
    private GameObject ice;

    [SerializeField]
    private GameObject iceOnshovel;

    bool isiceon = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Dot(Vector3.up, transform.forward);
        float flip = Vector3.Dot(Vector3.up, transform.up);

        //Debug.Log(angle + " || " + flip);

        if (isiceon)
        {
            if (flip > 0 && angle < 0.5)
            {
                //nothing happen
            }
            else
            {
                StartCoroutine(instanciate_Ice());
            }
        }
    }

    IEnumerator instanciate_Ice()
    {
        isiceon = false;

        for (int i = 0; i < 6; i++)
        {
            Instantiate(ice, ice_output.position, Quaternion.identity);
        }

        iceOnshovel.SetActive(false);
        yield return null;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("icecontainer"))
        {
            //Debug.Log("ice in");
            if (iceOnshovel.activeInHierarchy == false)
            {
                isiceon = true;
                iceOnshovel.SetActive(true);
            }
        }
    }
}
