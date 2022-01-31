using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Encrypter2 : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayTextTMP = null;

    [SerializeField] Slider sliderSuppression = null;
    [SerializeField] Slider sliderEncryption = null;
    [SerializeField] Slider sliderScramble = null;
    [SerializeField] char[] letterPool;
    char space = ' ';
    
    //settings
    int maxSettings = 20;
    int maxEncryptionShift = 2;

    //state
    int targetValue_Suppression = 10;
    int targetValue_Encryption = 10;
    int targetValue_Scramble = 10;
    string plainText;
    string cypherText;
    [SerializeField] bool[,] suppressions;
    [SerializeField] int[,] encryptions;
    [SerializeField] int[,] scrambles;

    #region Preparation
    private void Start()
    {
        plainText = displayTextTMP.text;
        SetupSliders();
        CreateLetterPool();
        InitializeParameterizedSettings();
        GenerateParameterizedSettings();
    }

    private void CreateLetterPool()
    {
        letterPool = new char[26];
        for (int i = 0; i < 26; i++)
        {
            letterPool[i] = Convert.ToChar('a' + i);
        }
    }

    private void SetupSliders()
    {
        sliderSuppression.minValue = 0;
        sliderSuppression.maxValue = maxSettings - 1;
        sliderEncryption.minValue = 0;
        sliderEncryption.maxValue = maxSettings - 1;
        sliderScramble.minValue = 0;
        sliderScramble.maxValue = maxSettings - 1;
    }

    private void InitializeParameterizedSettings()
    {
        int length = plainText.Length;
        suppressions = new bool[maxSettings,length];
        encryptions = new int[maxSettings, length];
        scrambles = new int[maxSettings, length];
    }

    private void GenerateParameterizedSettings()
    {
        for (int j = 0; j < maxSettings; j++)
        {
            for (int i = 0; i < plainText.Length; i++)
            {
                suppressions[j, i] = ReturnWeightedBoolean(j, targetValue_Suppression);
                encryptions[j, i] = ReturnWeightedInt(j, targetValue_Encryption);
                scrambles[j, i] = ReturnWeightedInt(j, targetValue_Scramble);
            }
        }
        //for (int k = 0; k < plainText.Length; k++)
        //{
        //    Debug.Log($"slider value {1}, index {k} is {suppressions[0, k]}");
        //}

        Encrypt();
    }

    private bool ReturnWeightedBoolean(float currentValue, float targetValue)
    {
        int diff = Mathf.RoundToInt(Mathf.Abs(targetValue - currentValue));
        int rand = UnityEngine.Random.Range(0, 12);
        if (rand > diff)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int ReturnWeightedInt(float currentValue, float targetValue)
    {
        int diff = Mathf.RoundToInt(Mathf.Abs(targetValue - currentValue));
        int rand = UnityEngine.Random.Range(0, 12);
        if (rand > diff)
        {
            return 0;
        }
        else
        {
            int rand2 = UnityEngine.Random.Range(1, maxEncryptionShift + 1);
            return rand2;
        }

    }

    #endregion
    public void Encrypt()
    {
        char[] cypherChars = new char[plainText.Length];

        cypherChars = ApplyEncryption(cypherChars, sliderEncryption.value);
        cypherChars = ApplyScrambling(cypherChars, sliderScramble.value);
        cypherChars = ApplySuppression(cypherChars, sliderSuppression.value);

        cypherText = AssembleCypherText(cypherChars);
        displayTextTMP.text = cypherText;
    }

    #region Helpers

    private char[] ApplyEncryption(char[] inputChars, float value)
    {
        char[] plainChars = plainText.ToCharArray();
        char[] cypherChars = inputChars;
        for (int i = 0; i < inputChars.Length; i++)
        {
            cypherChars[i] = Convert.ToChar(plainChars[i] + encryptions[Mathf.RoundToInt(sliderEncryption.value), i]);
            // Add in a "clamp" that keeps the new characters as something legible.
        }

        return cypherChars;
    }

    private char[] ApplySuppression(char[] inputChars, float value)
    {
        //char[] plainChars = plainText.ToCharArray();
        char[] newCypherChars = inputChars;
        for (int i = 0; i < inputChars.Length; i++)
        {
            if (suppressions[Mathf.RoundToInt(sliderSuppression.value), i] == false)  //
            {
                if (Char.IsLetter(inputChars[i]) == true)
                {
                    newCypherChars[i] = space;
                }
                else
                {
                    newCypherChars[i] = GetRandomChar();
                }
            }
            else
            {
                newCypherChars[i] = inputChars[i];
            }

        }

        //for (int k = 0; k < plainText.Length; k++)
        //{
        //    Debug.Log($"new cypher text at {k} is {newCypherChars[k]}");
        //}

        return newCypherChars;
    }

    private char[] ApplyScrambling(char[] inputChars, float value)
    {
        char[] newCypherChars = inputChars;
        for (int i = 0; i < inputChars.Length; i++)
        {
            int movement = scrambles[Mathf.RoundToInt(sliderScramble.value), i];
            if ( movement == 0)
            {
                //do no scrambling 
            }
            else
            {
                char charCurrentlyInNewSpot;
                char charInCurrentSpot = newCypherChars[i];
                Debug.Log($"at index{i} of {inputChars.Length - 1}. Movement is {movement}");
                if (i+movement >= inputChars.Length)
                {
                    charCurrentlyInNewSpot = newCypherChars[i - movement];
                    newCypherChars[i] = charCurrentlyInNewSpot;
                    newCypherChars[i - movement] = charInCurrentSpot;
                }
                else
                {                    
                    charCurrentlyInNewSpot = newCypherChars[i + movement];
                    newCypherChars[i] = charCurrentlyInNewSpot;
                    newCypherChars[i + movement] = charInCurrentSpot;
                }             

            }
        }
        return newCypherChars;
    }

    private string AssembleCypherText(char[] cypherChars)
    {
        string newText = "";
        for (int i = 0; i < cypherChars.Length; i++)
        {
            newText += cypherChars[i];
        }
        return newText;
    }

    private char GetRandomChar()
    {
        int rand = UnityEngine.Random.Range(0, letterPool.Length);
        return letterPool[rand];
    }
    #endregion
}
