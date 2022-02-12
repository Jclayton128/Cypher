using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TextCipherEngine : CipherEngine
{
    FileManager fm;

    [SerializeField] char[] letterPool;
    char space = ' ';
    
    //settings
    int maxEncryptionShift = 2;


    //state
    string plainText;
    string cypherText;
    [SerializeField] bool[,] suppressions;
    [SerializeField] int[,] encryptions;
    [SerializeField] int[,] scrambles;

    #region Preparation
    protected override void Start()
    {
        base.Start();
        fm = FindObjectOfType<FileManager>();
        CreateLetterPool();
        //InitializeNewFile(fm.SetInitialText());
    }
    
    public override void InitializeNewFile(System.Object newObject)
    {
        TextPack file = (TextPack)newObject;
        plainText = file.FilePlaintext;
        targetVal_0 = file.TargetValues[0];
        targetVal_1 = file.TargetValues[1];
        targetVal_2 = file.TargetValues[2];

        InitializeParameterizedSettings();
        GenerateParameterizedSettings();

        isReadyToObfuscate = true;

        Obfuscate();
    }

    private void CreateLetterPool()
    {
        letterPool = new char[30];
        for (int i = 0; i < 26; i++)
        {
            letterPool[i] = Convert.ToChar('a' + i);
        }
        for (int i = 26; i < letterPool.Length; i++)
        {
            letterPool[i] = space;
        }
    }


    private void InitializeParameterizedSettings()
    {
        int length = plainText.Length;
        suppressions = new bool[im.MaxSettings,length];
        encryptions = new int[im.MaxSettings, length];
        scrambles = new int[im.MaxSettings, length];
    }

    private void GenerateParameterizedSettings()
    {
        for (int j = 0; j < im.MaxSettings; j++)
        {
            for (int i = 0; i < plainText.Length; i++)
            {
                suppressions[j, i] = ReturnWeightedBoolean(j, targetVal_0);
                encryptions[j, i] = ReturnWeightedInt(j, targetVal_1);
                scrambles[j, i] = ReturnWeightedInt(j, targetVal_2);
            }
        }
        //for (int k = 0; k < plainText.Length; k++)
        //{
        //    Debug.Log($"slider value {1}, index {k} is {suppressions[0, k]}");
        //}

        Obfuscate();
    }

    private bool ReturnWeightedBoolean(float currentValue, float targetValue)
    {
        int diff = Mathf.RoundToInt(Mathf.Abs(targetValue - currentValue));
        int rand = UnityEngine.Random.Range(0, 21);
        if (rand >= diff)
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
        int rand = UnityEngine.Random.Range(0, 21);
        if (rand >= diff)
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
    public override void Obfuscate()
    {
        if (!isReadyToObfuscate)
        {
            Debug.Log("Not ready to obfuscate");
            return;
        }
        char[] cypherChars = plainText.ToCharArray();

        cypherChars = ApplyEncryption(cypherChars, im.SliderValue_1);
        cypherChars = ApplyScrambling(cypherChars, im.SliderValue_2);
        cypherChars = ApplySuppression(cypherChars, im.SliderValue_0);

        cypherChars = Despace(cypherChars);
        cypherText = AssembleCypherText(cypherChars);

        im.UpdateDisplay(cypherText);
    }

    #region Helpers

    private char[] ApplyEncryption(char[] inputChars, float value)
    {
        char[] cypherChars = inputChars;
        for (int i = 0; i < inputChars.Length; i++)
        {
            if (Char.IsLetter(cypherChars[i]))
            {
                cypherChars[i] = Convert.ToChar(inputChars[i] + encryptions[im.SliderValue_1, i]);
                // Add in a "clamp" that keeps the new characters as something legible.
            }
            else
            {
                if (encryptions[im.SliderValue_1, i] == 0)
                {
                    cypherChars[i] = inputChars[i];
                }
                else
                {
                    cypherChars[i] = Convert.ToChar(GetRandomChar() + encryptions[im.SliderValue_1, i]);
                }                
            }
        }

        return cypherChars;
    }

    private char[] ApplySuppression(char[] inputChars, float value)
    {
        //char[] plainChars = plainText.ToCharArray();
        char[] newCypherChars = inputChars;
        for (int i = 0; i < inputChars.Length; i++)
        {
            if (suppressions[im.SliderValue_0, i] == false)  //
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
            int movement = scrambles[im.SliderValue_2, i];
            if ( movement == 0)
            {
                //do no scrambling 
            }
            else
            {
                char charCurrentlyInNewSpot;
                char charInCurrentSpot = newCypherChars[i];
                //Debug.Log($"at index{i} of {inputChars.Length - 1}. Movement is {movement}");
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

    private char[] Despace(char[] cypherChars)
    {
        for(int i = 0; i < cypherChars.Length; i++)
        {
            if (Char.IsWhiteSpace(cypherChars[i]))
            {
                cypherChars[i] = space;
            }
        }
        return cypherChars;
    }
    #endregion
}
