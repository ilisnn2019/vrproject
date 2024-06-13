using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ice") || collision.gameObject.CompareTag("shaker") || collision.gameObject.CompareTag("glass"))
        {

        }
        else
        {
            audiosource.Play();
            Destroy(gameObject, 3f);
        }
    }
}
