using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "AudioClue")]
public class AudioClue : ScriptableObject
{
    [SerializeField] AudioClip[] simoClips = new AudioClip[5];

    public AudioClip[] Clips_Shuffled;

    static System.Random _random = new System.Random();

    public void PrepareAudioClue()
    {
        Clips_Shuffled = simoClips;
        Shuffle(Clips_Shuffled);
    }

    static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    public AudioClip[] GetShuffledClips()
    {
        return Clips_Shuffled;
    }
}
