using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip[] bgm;
    private AudioSource audioSource;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(int chapter)
    {
        audioSource.Stop();
        audioSource.clip = bgm[chapter - 1];
        audioSource.Play();
    }
}
