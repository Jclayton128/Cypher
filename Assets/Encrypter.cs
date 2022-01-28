using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Encrypter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTMP = null;
    [SerializeField] Toggle[] encryptions = null;
    private static System.Random rng = new System.Random();

    //Settings
    char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
    char[] consonants_first = { 'b', 'c', 'd', 'f', 'g', 'h' };
    char[] consonants_second = { 'j', 'k', 'l', 'm', 'n', 'p', 'r', 's', 't' };
    char[] wordEnders = { 'd', 'h', 'k', 'l', 'm', 'n', 's', 't', 'w' };


    //state
    string plainText = "";
    char[] plainTextChars;
    char[] cypherTextChars;
    string cypherText = "";

    bool[] activeEncryptions;

    private void Start()
    {
        plainText = textTMP.text;
        plainTextChars = plainText.ToCharArray();
        cypherTextChars = new char[plainTextChars.Length];
        activeEncryptions = new bool[encryptions.Length];
    }
    public void Encrypt()
    {
        int[] encryptionChars = new int[plainTextChars.Length];
        if (activeEncryptions[0])
        {
            ApplyEncryption(ref encryptionChars, 0);
        }
        if (activeEncryptions[1])
        {
            ApplyEncryption(ref encryptionChars, 1);
        }
        if (activeEncryptions[2])
        {
            ApplyEncryption(ref encryptionChars, 2);
        }
        if (activeEncryptions[3])
        {
            ApplyEncryption(ref encryptionChars, 3);
        }

        TotalizeEncryption(in encryptionChars, ref plainTextChars);

        // Apply intra-word letter scrambling

        // Apply intra-sentence word scrambling
        if (activeEncryptions[4])
        {
            ApplyIntrasentenceWordScrambling();
        }

        // Apply intra-sentence letter scrambling
        if (activeEncryptions[5])
        {
            ApplyIntrasentenceLetterScrambling();
        }

        UpdateTMP();
    }


    private void UpdateTMP()
    {
        textTMP.text = cypherText;
    }

    private void ApplyIntrasentenceWordScrambling()
    {
        string newCypherText = "";
        string[] words = cypherText.Split();
        Shuffle<string>(words);
        for (int i = 0; i < words.Length; i++)
        {
            newCypherText += words[i];
        }
        cypherText = newCypherText;
    }

    static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < (n - 1); i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + rng.Next(n - i);
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    private void ApplyIntrasentenceLetterScrambling()
    {
        string newCypherText = "";
        char[] letters = new char[cypherText.Length];
        letters = cypherText.ToCharArray();
        Shuffle<char>(letters);
        for (int i = 0; i < letters.Length; i++)
        {
            newCypherText += letters[i];
        }
        cypherText = newCypherText;
    }

    private void TotalizeEncryption(in int[] encryptionChars, ref char[] plainTextChars)
    {
        cypherText = "";
        for (int i = 0; i < encryptionChars.Length; i++)
        {
            cypherTextChars[i] = Convert.ToChar(plainTextChars[i] + (encryptionChars[i]));
            cypherText += cypherTextChars[i];
        }
    }

    private void ApplyEncryption(ref int[] encryptionChars, int encryptionMethod)
    {
        for(int i = 0; i < encryptionChars.Length; i++)
        {
            switch (encryptionMethod)
            {
                case 0:
                    if (CheckForLetters(plainTextChars[i], vowels))
                    {
                        encryptionChars[i] -= 10;
                    }
                    break;

                case 1:
                    if (CheckForLetters(plainTextChars[i], consonants_first))
                    {
                        encryptionChars[i] += 2;
                    }
                    break;

                case 2:
                    if (CheckForLetters(plainTextChars[i], consonants_second))
                    {
                        encryptionChars[i] += 3;
                    }
                    break;

                case 3:
                    if (CheckForLetters(plainTextChars[i], wordEnders))
                    {
                        encryptionChars[i] *= 2;
                    }
                    break;
            }
        }
    }


    public void CheckActiveEncryptions()
    {
        for (int i = 0; i < encryptions.Length; i++)
        {
            activeEncryptions[i] = encryptions[i].isOn;
        }
    }

    private bool CheckForLetters(char referenceLetter, char[] targetLetters)
    {
        foreach (char targetLetter in targetLetters)
        {
            Debug.Log($"{targetLetter} compared to {referenceLetter}");
            if (targetLetter == referenceLetter)
            {
                
                return true;
            }
        }

        return false;
    }

}
