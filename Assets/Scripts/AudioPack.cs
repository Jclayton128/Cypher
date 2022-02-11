using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AudioPack
{
    public AudioClip[] PlainClips;
    public int[] TargetValues;

    public AudioPack(AudioClip[] plainAudioClips, int[] targetValues)
    {
        this.PlainClips = plainAudioClips;
        this.TargetValues = targetValues;
    }
}
