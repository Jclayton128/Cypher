using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TextPack
{

    public string FilePlaintext;
    public int[] TargetValues;

    public TextPack(string plaintext, int[] targetValues)
    {
        //FilePlaintext = "default plain text goes here";
        //TargetValues = new int[3] { 0,0,0};
        FilePlaintext = plaintext;
        TargetValues = targetValues;
    }
}
