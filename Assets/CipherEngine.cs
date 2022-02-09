using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class CipherEngine : MonoBehaviour
{
    protected InterfaceManager im;
    protected bool isReadyToObfuscate = false;

    protected virtual void Start()
    {
        im = FindObjectOfType<InterfaceManager>();
    }


    public abstract void Obfuscate();


    public abstract void InitializeNewFile(System.Object newFile);
}

