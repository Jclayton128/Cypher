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
    [SerializeField] Image MugshotImage = null;
    [SerializeField] TextMeshProUGUI SuspectNameTMP = null;
    [SerializeField] TextMeshProUGUI[] SuspectTraitsTMP = null;
    [SerializeField] Slider SuspectGuiltinessSlider = null;


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
        toggleMode.value = 0;
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
        if (CurrentMode == Mode.Image || CurrentMode == Mode.Text)
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
        }
        if (CurrentMode == Mode.Suspect)
        {
            slider0.minValue = 0;
            slider0.maxValue = 3;
            //slider0.value = fm.GetCurrentSuspectSuspicion();
            slider1.minValue = 0;
            slider1.maxValue = 0;
            slider1.value = 0;
            slider2.minValue = 0;
            slider2.maxValue = 0;
            slider2.value = 0;
        }

        //SliderValue_0 = Mathf.RoundToInt(slider0.value);
        //SliderValue_1 = Mathf.RoundToInt(slider1.value);
        //SliderValue_2 = Mathf.RoundToInt(slider2.value);
    }


    private void UpdateLabels()
    {
        if (CurrentMode == Mode.Suspect)
        {
            sliderLabels[0].text = "< Suspicion >";

            for (int i = 1; i < sliderLabels.Length; i++)
            {
                sliderLabels[i].text = " ";
            }
            //if (SuspectGuiltinessSlider.value == 0)
            //{
            //    sliderLabels[1].text = "Yes << Not Guilty?";
            //}
            if (SuspectGuiltinessSlider.value == 3)
            {
                sliderLabels[2].text = "Guilty? >> Yes";
            }

            return;
        }

        string[] labels = currentCipherEngine.GetSliderLabels();
        for (int i = 0; i < sliderLabels.Length; i++)
        {
            sliderLabels[i].text = labels[i];
        }
    }

    #region Button Handlers
    public void HandleUpdatedSliders()
    {
        SliderValue_0 = Mathf.RoundToInt(slider0.value);
        SliderValue_1 = Mathf.RoundToInt(slider1.value);
        SliderValue_2 = Mathf.RoundToInt(slider2.value);

        if (CurrentMode == Mode.Image || CurrentMode == Mode.Text)
        {
            currentCipherEngine.Obfuscate();
        }
        if (CurrentMode == Mode.Suspect)
        {
            fm.UpdateSuspicion(SliderValue_0);

            if (SuspectGuiltinessSlider.value == 3)
            {
                slider2.maxValue = 3;
            }
            else
            {
                slider2.maxValue = 0;
                slider2.value = 0;
            }

            fm.CheckForGuilty(SliderValue_2);
            UpdateLabels();
        }
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
            case Mode.Image:
                displayTextTMP.gameObject.SetActive(false);
                ImageScreen.gameObject.SetActive(true);
                SuspectScreen.gameObject.SetActive(false);
                SetupSliders();
                return;

            case Mode.Text:
                displayTextTMP.gameObject.SetActive(true);
                ImageScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(false);
                SetupSliders();
                return;

            case Mode.Suspect:
                displayTextTMP.gameObject.SetActive(false);
                ImageScreen.gameObject.SetActive(false);
                SuspectScreen.gameObject.SetActive(true);
                SetupSliders();
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

    public void UpdateSuspectDisplay(SuspectFile newSuspectFile)
    {
        MugshotImage.sprite = newSuspectFile.GetSuspectMugshot();
        SuspectNameTMP.text = newSuspectFile.GetSuspectName();
        string[] traits = newSuspectFile.GetSuspectTraits();
        for (int i = 0; i < SuspectTraitsTMP.Length; i++)
        {
            SuspectTraitsTMP[i].text = traits[i];
        }
        SuspectGuiltinessSlider.value = newSuspectFile.CurrentSuspicion;
        slider0.value = newSuspectFile.CurrentSuspicion;
        SetupSliders();
        UpdateLabels();
    }

    public void UpdateSuspectDisplaySuspicionOnly(int suspicionAmount)
    {
        SuspectGuiltinessSlider.value = suspicionAmount;
        slider0.value = suspicionAmount;
        UpdateLabels();
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
