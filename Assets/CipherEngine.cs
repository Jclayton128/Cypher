using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class CipherEngine : MonoBehaviour
{
    protected InterfaceManager im;

    protected virtual void Start()
    {
        im = FindObjectOfType<InterfaceManager>();
        im.OnSlidersMoved += Obfuscate;
    }   


    public abstract void Obfuscate();

}

