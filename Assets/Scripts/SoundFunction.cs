using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundFunction : MonoBehaviour
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

    public bool IsPlaying() { return audiosource.isPlaying;  }

    public void ClipPlay(AudioClip clip)
    {

        if (audiosource.isPlaying)
            audiosource.Stop();

        audiosource.clip = clip;



        audiosource.Play();
    }
}
