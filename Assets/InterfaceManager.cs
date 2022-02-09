using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class InterfaceManager : MonoBehaviour
{
    protected TextMeshProUGUI displayTextTMP;
    public Image imageScreen { get; protected set; }
    Image backgroundScreen;
    protected Slider slider0;
    protected Slider slider1;
    protected Slider slider2;

    public Action OnSlidersMoved;

    public enum Mode { Text, Image};

    //settings
    public int MaxSettings { get; private set; } = 20;

    //state
    public int SliderValue_0 { get; private set; }
    public int SliderValue_1 { get; private set; }
    public int SliderValue_2 { get; private set; }
    public Mode CurrentMode { get; private set; }

    public 

    // Start is called before the first frame update
    void Start()
    {
        FindComponents();
        SetupSliders();
    }

    private void FindComponents()
    {
        displayTextTMP = GameObject.FindGameObjectWithTag("ScreenText").GetComponent<TextMeshProUGUI>();
        imageScreen = GameObject.FindGameObjectWithTag("Screen").GetComponent<Image>();
        //backgroundScreen = GameObject.FindGameObjectWithTag("BackgroundScreen").GetComponent<Image>();
        slider0 = GameObject.FindGameObjectWithTag("Slider0").GetComponent<Slider>();
        slider1 = GameObject.FindGameObjectWithTag("Slider1").GetComponent<Slider>();
        slider2 = GameObject.FindGameObjectWithTag("Slider2").GetComponent<Slider>();
    }

    protected void SetupSliders()
    {
        slider0.minValue = 0;
        slider0.maxValue = MaxSettings - 1;
        slider0.value = 0;
        slider1.minValue = 0;
        slider1.maxValue = MaxSettings - 1;
        slider1.value = 0;
        slider2.minValue = 0;
        slider2.maxValue = MaxSettings - 1;
        slider2.value = 0;
        SliderValue_0 = Mathf.RoundToInt(slider0.value);
        SliderValue_1 = Mathf.RoundToInt(slider1.value);
        SliderValue_2 = Mathf.RoundToInt(slider2.value);
    }

    public void HandleUpdatedSliders()
    {
        SliderValue_0 = Mathf.RoundToInt(slider0.value);
        SliderValue_1 = Mathf.RoundToInt(slider1.value);
        SliderValue_2 = Mathf.RoundToInt(slider2.value);

        OnSlidersMoved?.Invoke();
    }

    public void HandleModeToggled()
    {

    }

    public void UpdateDisplay(string text)
    {
        displayTextTMP.gameObject.SetActive(true);
        backgroundScreen.gameObject.SetActive(true);
        imageScreen.gameObject.SetActive(false);

        displayTextTMP.text = text;
    }

    public void UpdateDisplay(Sprite sprite)
    {
        displayTextTMP.gameObject.SetActive(false);
        backgroundScreen.gameObject.SetActive(false);
        imageScreen.gameObject.SetActive(true);

        imageScreen.sprite = sprite;
    }

    public Material GetScreenMaterial()
    {
        return imageScreen.material;
    }

    public Image GetScreenImage()
    {
        return imageScreen;
    }
}
