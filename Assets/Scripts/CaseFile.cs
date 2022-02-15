using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CaseFile")]
public class CaseFile : ScriptableObject
{
    [SerializeField] string CaseName = "Default Name";
    
    [Tooltip ("First painting is the forgery, the rest are red herrings")]
    [SerializeField] Sprite[] paintings = null;  //First is forgery, rest are red herrings
    [SerializeField] Sprite[] spriteClues = null;
    [SerializeField] string[] textClues = null;
    [SerializeField] AudioClue[] audioClues = null;

    public Sprite[] Paintings_Shuffled;
    public AudioClue[] AudioClues_Shuffled;
    public string[] TextClues_Shuffled;
    public Sprite[] SpriteClues_Shuffled;

    static System.Random _random = new System.Random();

    public void PrepareCase()
    {
        Paintings_Shuffled = (Sprite[])paintings.Clone();
        AudioClues_Shuffled = (AudioClue[])audioClues.Clone();
        TextClues_Shuffled = (string[])textClues.Clone();
        SpriteClues_Shuffled = (Sprite[])spriteClues.Clone();

        Shuffle(Paintings_Shuffled);
        Shuffle(AudioClues_Shuffled);
        Shuffle(TextClues_Shuffled);
        Shuffle(SpriteClues_Shuffled);

        foreach (var audioClue in AudioClues_Shuffled)
        {
            audioClue.PrepareAudioClue();
        }
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

    public string GetCaseName()
    {
        return CaseName;
    }

    public Sprite GetForgery()
    {
        return paintings[0];
    }


}
