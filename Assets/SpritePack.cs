using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpritePack
{

    public Sprite SpritePlain;
    public int[] TargetValues;

    public SpritePack(Sprite spritePlain, int[] targetValues)
    {
        this.SpritePlain = spritePlain;
        this.TargetValues = targetValues;
    }
}
