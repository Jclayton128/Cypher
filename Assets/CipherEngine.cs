using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CipherEngine : MonoBehaviour
{
    protected TextMeshProUGUI displayTextTMP;
    protected RawImage displayImage;
    protected Slider slider0 ;
    protected Slider slider1;
    protected Slider slider2;

    //settings
    public int MaxSettings { get; private set; } = 20;

    protected virtual void Start()
    {
        FindComponents();
        SetupSliders();
    }
    
    private void FindComponents()
    {
        displayTextTMP = GameObject.FindGameObjectWithTag("ScreenText").GetComponent<TextMeshProUGUI>();
        displayImage = GameObject.FindGameObjectWithTag("Screen").GetComponent<RawImage>();
        slider0 = GameObject.FindGameObjectWithTag("Slider0").GetComponent<Slider>();
        slider1 = GameObject.FindGameObjectWithTag("Slider1").GetComponent<Slider>();
        slider2 = GameObject.FindGameObjectWithTag("Slider2").GetComponent<Slider>();

    }

    protected void SetupSliders()
    {
        slider0.minValue = 0;
        slider0.maxValue = MaxSettings - 1;
        slider1.minValue = 0;
        slider1.maxValue = MaxSettings - 1;
        slider2.minValue = 0;
        slider2.maxValue = MaxSettings - 1;
    }

}

