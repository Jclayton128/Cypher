using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCipherEngine : CipherEngine
{
    [SerializeField] AudioSource[] audioSources = null;
    [SerializeField] AudioListener audioMouse = null;

    //state
    bool isInAutoPlay = true;
    AudioClip[] clips = new AudioClip[3];

    public override void InitializeNewFile(System.Object newObject)
    {
        AudioPack audioPack = (AudioPack)newObject;
        im.UpdateDisplay("Audio Clip: press Play");
        targetVal_0 = audioPack.TargetValues[0];
        targetVal_1 = audioPack.TargetValues[1]; ;
        targetVal_2 = audioPack.TargetValues[2];

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].clip = audioPack.PlainClips[i];
        }

        isReadyToObfuscate = true;

        //Obfuscate();
    }

    public bool HandleAutoPlayToggle()
    {
        isInAutoPlay = !isInAutoPlay;
        return isInAutoPlay;

        //if (auso_0.isPlaying)
        //{
        //    Debug.Log("Pause commanded");
        //    auso_0.Pause();
        //    auso_1.Pause();
        //    auso_2.Pause();
        //}
        //else
        //{
        //    Debug.Log("Play commanded");
        //    auso_0.Play();
        //    auso_1.Play();
        //    auso_2.Play();
        //}
    }

    public override void Obfuscate()
    {
        HandleSlider0();  //

        if (isInAutoPlay)
        {
            PlayAudioClue();
        }
    }

    public void PlayAudioClue()
    {
        foreach (var auso in audioSources)
        {
            auso.Play();
        }
    }

    private void HandleSlider0()
    {
        audioMouse.transform.position = new Vector2(im.SliderValue_0, 0);
    }

    public void AdjustVolume(float factor)
    {
        foreach (var auso in audioSources)
        {
            auso.volume = factor;
        }
    }
}
