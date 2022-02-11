using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestPanel : MonoBehaviour
{

    AudioSource auso;

    private void Start()
    {
        auso = GetComponent<AudioSource>();
    }

    public void HandlePress()
    {
        auso.Play();
    }

}
