using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Washing : MonoBehaviour
{
    float time_interval = 5;
    public void OnTriggerStay(Collider other)
    {
        if(time_interval >= 0)
        {
            time_interval -= time_interval;

        }
        else
        {

        }
    }

}
