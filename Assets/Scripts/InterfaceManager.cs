using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class InterfaceManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayTextTMP = null;
    TextCipherEngine textCE;
    ImageCipherEngine imageCE;
    AudioCipherEngine audioCE;
    FileManager fm;
    [SerializeField] Image ImageScreen = null;
    [SerializeField] Image PaintingScreen = null;
    [SerializeField] Slider[] sliders = null;
    [SerializeField] Slider toggleMode = null;

    [SerializeField] GameObject SuspectScreen = null;
    [SerializeField] Image MugshotImage = null;
    [SerializeField] TextMeshProUGUI SuspectNameTMP = null;
    [SerializeField] TextMeshProUGUI[] SuspectTraitsTMP = null;
    [SerializeField] Slider SuspectGuiltinessSlider = null;


    [SerializeField] TextMeshProUGUI fileIndexTMP = null;

    [SerializeField] TextMeshProUGUI[] sliderLabels = null;
    
    public Action OnSlidersMoved;
    public Action<Mode> OnModeChanged;
    public enum Mode { Text, Sprite, Audio};

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

        SetupSliders();
        InitializeGame();
    }

    public void InitializeGame()
    {
        HandleModeToggled(true);
        HandleUpdatedSliders(true);
        UpdateDisplayMode(CurrentMode);
    }

    private void FindComponents()
    {
        //displayTextTMP = GameObject.FindGameObjectWithTag("ScreenText").GetComponent<TextMeshProUGUI>();
        //ImageScreen = GameObject.FindGameObjectWithTag("Screen").GetComponent<Image>();
        //slider0 = GameObject.FindGameObjectWithTag("Slider0").GetComponent<Slider>();
        //slider1 = GameObject.FindGameObjectWithTag("Slider1").GetComponent<Slider>();
        //slider2 = GameObject.FindGameObjectWithTag("Slider2").GetComponent<Slider>();
        //toggleMode = GameObject.FindGameObjectWithTag("ToggleMode").GetComponent<Slider>();
        imageCE = FindObjectOfType<ImageCipherEngine>();
        textCE = FindObjectOfType<TextCipherEngine>();
        audioCE = FindObjectOfType<AudioCipherEngine>();
        fm = FindObjectOfType<FileManager>();
    }

    protected void SetupSliders()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].minValue = 0;
            sliders[i].maxValue = MaxSettings - 1;
            sliders[i].value = 0;
        }
    }


    private void UpdateLabels()
    {

        string[] labels = currentCipherEngine.GetSliderLabels();
        for (int i = 0; i < sliderLabels.Length; i++)
        {
            sliderLabels[i].text = labels[i];
        }
    }


    #region Button Handlers
    public void HandleUpdatedSliders(bool isForInitialization)
    {
        SliderValue_0 = Mathf.RoundToInt(sliders[0].value);
        SliderValue_1 = Mathf.RoundToInt(sliders[1].value);
        SliderValue_2 = Mathf.RoundToInt(sliders[2].value);

        if (CurrentMode == Mode.Sprite || CurrentMode == Mode.Text || CurrentMode == Mode.Audio)
        {
            currentCipherEngine.Obfuscate();
        }
       
    }

    public void HandleModeToggled(bool isForInitialization)
    {
        
        currentCipherEngine = textCE;

        if (!isForInitialization)
        {
            int[] values = new int[3] { SliderValue_0, SliderValue_1, SliderValue_2 };
            fm.ReceiveUpdatedSliderValues(CurrentMode, values);
        }

        if (toggleMode.value == 0)
        {
            CurrentMode = Mode.Text;
            currentCipherEngine = textCE;
            currentCipherEngine.Obfuscate();
        }
        if (toggleMode.value == 1)
        {
            CurrentMode = Mode.Sprite;
            currentCipherEngine = imageCE;
        }
        if (toggleMode.value == 2)
        {
            CurrentMode = Mode.Audio;
            currentCipherEngine = audioCE;
        }
        //else
        //{
        //    currentCipherEngine = textCE;
        //    Debug.Log($"invoking fallback mode selection of {currentCipherEngine}");
        //}
        UpdateLabels();
        OnModeChanged?.Invoke(CurrentMode);


    }


    public void DriveSlidersToCurrentFilePreviousSetting(int[] settings)
    {
        for (int i = 0; i < settings.Length; i++)
        {
            sliders[i].value = settings[i];
        }
    }

    public void HandleNextPressed()
    {
        int[] values = new int[3] { SliderValue_0, SliderValue_1, SliderValue_2 };
        fm.ReceiveUpdatedSliderValues(CurrentMode, values);
        fm.StepToNextFile(CurrentMode);
    }

    public void HandleBackPressed()
    {
        int[] values = new int[3] { SliderValue_0, SliderValue_1, SliderValue_2 };
        fm.ReceiveUpdatedSliderValues(CurrentMode, values);
        fm.StepBackFile(CurrentMode);
    }

    public void HandlePlayPausePressed()
    {
        if (CurrentMode == Mode.Audio)
        {
            audioCE.HandleAutoPlayToggle();
        }
        else
        {
            Debug.Log($"Cannot play audio in {CurrentMode} mode.");
        }
    }

    #endregion

    #region Public UI Updaters
    public void UpdateFileIndexDisplay(string newFileIndex)
    {
        fileIndexTMP.text = newFileIndex;
    }

    public void UpdateDisplayMode(Mode newMode)
    {
        Debug.Log($"updating mode to {newMode}");
        switch (newMode)
        {
            case Mode.Sprite:
                displayTextTMP.gameObject.SetActive(false);
                ImageScreen.gameObject.SetActive(true);
                PaintingScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(false);
                SetupSliders();
                return;

            case Mode.Text:
                displayTextTMP.gameObject.SetActive(true);
                ImageScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(false);
                PaintingScreen.gameObject.SetActive(false);
                SetupSliders();
                return;

            case Mode.Audio:
                displayTextTMP.gameObject.SetActive(true);
                displayTextTMP.text = "Audio Clip Loaded";
                ImageScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(false);
                PaintingScreen.gameObject.SetActive(false);
                SetupSliders();
                return;

        }
    }

    public bool UpdateDisplay(string text)
    {
        if (CurrentMode == Mode.Text || CurrentMode == Mode.Audio)
        {
            displayTextTMP.text = text;
            return true;
        }
        else
        {
            Debug.Log($"Cannot send text while in {CurrentMode} mode");
            return false;
        }
    }

    public bool UpdateDisplay(Sprite sprite)
    {
        if (CurrentMode != Mode.Sprite)
        {
            Debug.Log($"Cannot send images while in {CurrentMode} mode");
            return false;
        }
        ImageScreen.sprite = sprite;
        return true;
    }
    #endregion

    #region Public Helpers
    public Material GetScreenMaterial()
    {
        return ImageScreen.material;
    }

    public Image GetScreenImage()
    {
        return ImageScreen;
    }
    #endregion

}
