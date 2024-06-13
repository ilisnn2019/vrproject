using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShovel : MonoBehaviour
{
    [SerializeField]
    private Transform[] ice_output = new Transform[3];

    [SerializeField]
    private GameObject ice;

    [SerializeField]
    private GameObject iceOnshovel;

    AudioSource audiosource;

    bool isiceon = false;
    bool isincontainer = false;
    
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Vector3.Dot(Vector3.up, transform.forward);
        float flip = Vector3.Dot(Vector3.up, transform.up);

        //Debug.Log(angle + " || " + flip);

        if (isiceon && !isincontainer)
        {
            if (flip > 0 && angle < 0.5)
            {
                //nothing happen
            }
            else
            {
                StartCoroutine(Instanciate_Ice());
            }
        }
    }

    IEnumerator Instanciate_Ice()
    {
        isiceon = false;

        for (int i = 0; i < 6; i++)
        {
            int rand = Random.Range(0, 2);
            Instantiate(ice, ice_output[rand].position, Quaternion.identity);
            yield return new WaitForSeconds(0.02f);
        }

        iceOnshovel.SetActive(false);
        yield return null;
    }

    protected void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("icecontainer") && !isincontainer)
        {
            isincontainer = true;
            audiosource.Play();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("icecontainer") && isincontainer)
        {
            //Debug.Log("ice in");
            if (iceOnshovel.activeInHierarchy == false)
            {

                isiceon = true;
                iceOnshovel.SetActive(true);
            }
        }

        isincontainer = false;
    }
}
