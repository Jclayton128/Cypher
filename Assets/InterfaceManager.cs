using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class InterfaceManager : MonoBehaviour
{
    protected TextMeshProUGUI displayTextTMP;
    TextCipherEngine textCE;
    ImageCipherEngine imageCE;
    FileManager fm;
    public Image ImageScreen { get; protected set; }
    [SerializeField] Slider slider0 = null;
    [SerializeField] Slider slider1 = null;
    [SerializeField] Slider slider2 = null;
    [SerializeField] Slider toggleMode = null;

    [SerializeField] GameObject SuspectScreen = null;
    [SerializeField] TextMeshProUGUI fileIndexTMP = null;

    [SerializeField] TextMeshProUGUI[] sliderLabels = null;
    
    public Action OnSlidersMoved;
    public Action<Mode> OnModeChanged;
    public enum Mode { Text, Image, Suspect};

    //settings
    public int MaxSettings { get; private set; } = 20;

    //state
    public int SliderValue_0 { get; private set; }
    public int SliderValue_1 { get; private set; }
    public int SliderValue_2 { get; private set; }
    public Mode CurrentMode { get; private set; }
    protected CipherEngine currentCipherEngine;

    public 

    // Start is called before the first frame update
    void Start()
    {
        FindComponents();
        HandleModeToggled();
        SetupSliders();
        HandleUpdatedSliders();
        UpdateDisplayMode(CurrentMode);
    }

    private void FindComponents()
    {
        displayTextTMP = GameObject.FindGameObjectWithTag("ScreenText").GetComponent<TextMeshProUGUI>();
        ImageScreen = GameObject.FindGameObjectWithTag("Screen").GetComponent<Image>();
        slider0 = GameObject.FindGameObjectWithTag("Slider0").GetComponent<Slider>();
        slider1 = GameObject.FindGameObjectWithTag("Slider1").GetComponent<Slider>();
        slider2 = GameObject.FindGameObjectWithTag("Slider2").GetComponent<Slider>();
        toggleMode = GameObject.FindGameObjectWithTag("ToggleMode").GetComponent<Slider>();
        imageCE = FindObjectOfType<ImageCipherEngine>();
        textCE = FindObjectOfType<TextCipherEngine>();
        fm = FindObjectOfType<FileManager>();
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
        toggleMode.value = 0;
        //SliderValue_0 = Mathf.RoundToInt(slider0.value);
        //SliderValue_1 = Mathf.RoundToInt(slider1.value);
        //SliderValue_2 = Mathf.RoundToInt(slider2.value);
    }

    private void UpdateLabels()
    {
        if (CurrentMode == Mode.Suspect)
        {
            for (int i = 0; i < sliderLabels.Length; i++)
            {
                sliderLabels[i].text = " ";
            }
            return;
        }

        string[] labels = currentCipherEngine.GetSliderLabels();
        for (int i = 0; i < sliderLabels.Length; i++)
        {
            sliderLabels[i].text = labels[i];
        }
    }

    public void HandleUpdatedSliders()
    {
        SliderValue_0 = Mathf.RoundToInt(slider0.value);
        SliderValue_1 = Mathf.RoundToInt(slider1.value);
        SliderValue_2 = Mathf.RoundToInt(slider2.value);

        currentCipherEngine.Obfuscate();
    }

    public void HandleModeToggled()
    {
        currentCipherEngine = textCE;
        if (toggleMode.value == 0)
        {
            CurrentMode = Mode.Text;
            currentCipherEngine = textCE;
        }
        if (toggleMode.value == 1)
        {
            CurrentMode = Mode.Image;
            currentCipherEngine = imageCE;
        }
        if (toggleMode.value == 2)
        {
            CurrentMode = Mode.Suspect;
        }
        //else
        //{
        //    currentCipherEngine = textCE;
        //    Debug.Log($"invoking fallback mode selection of {currentCipherEngine}");
        //}
        UpdateLabels();
        OnModeChanged?.Invoke(CurrentMode);


    }

    public void HandleNextPressed()
    {
        fm.StepToNextFile(CurrentMode);
    }

    public void HandleBackPressed()
    {
        fm.StepBackFile(CurrentMode);
    }

    public void UpdateFileIndexDisplay(string newFileIndex)
    {
        fileIndexTMP.text = newFileIndex;
    }

    public void UpdateDisplayMode(Mode newMode)
    {
        Debug.Log($"updating mode to {newMode}");
        switch (newMode)
        {
            case Mode.Image:
                displayTextTMP.gameObject.SetActive(false);
                ImageScreen.gameObject.SetActive(true);
                SuspectScreen.gameObject.SetActive(false);
                return;

            case Mode.Text:
                displayTextTMP.gameObject.SetActive(true);
                ImageScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(false);
                return;

            case Mode.Suspect:
                displayTextTMP.gameObject.SetActive(false);
                ImageScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(true);
                return;


        }
    }

    public bool UpdateDisplay(string text)
    {
        if (CurrentMode != Mode.Text)
        {
            Debug.Log($"Cannot send text while in {CurrentMode} mode");
            return false;
        }
        displayTextTMP.text = text;
        return true;
    }

    public bool UpdateDisplay(Sprite sprite)
    {
        if (CurrentMode != Mode.Image)
        {
            Debug.Log($"Cannot send images while in {CurrentMode} mode");
            return false;
        }
        ImageScreen.sprite = sprite;
        return true;
    }

    public Material GetScreenMaterial()
    {
        return ImageScreen.material;
    }

    public Image GetScreenImage()
    {
        return ImageScreen;
    }
}
