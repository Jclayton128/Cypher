using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RapidSelectController : MonoBehaviour
{
    InterfaceManager im;
    
    [SerializeField] Image[] sliderRapidSelects = null;

    [SerializeField] Sprite blankDisplay = null;
    [SerializeField] Sprite litDisplay = null;
    [SerializeField] Button clueUp = null;
    [SerializeField] Button clueDown = null;
    [SerializeField] Button paintingLeft = null;
    [SerializeField] Button paintingRight = null;
    [SerializeField] Button autoplayToggle = null;
    [SerializeField] Button playButton = null;

    //state
    int currentSlider = 0;

    void Start()
    {
        im = FindObjectOfType<InterfaceManager>();
        UpdateSliderRapidSelectDisplay();
    }


    // Update is called once per frame
    void Update()
    {
        ListenForClueModeToggle();
        ListenForArrowKeys();
        ListenForClueScroll();
        ListenForPaintingScroll();
        ListenForAudioStuff();
    }

    private void ListenForClueModeToggle()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            im.CycleThroughClueModes();
        }
    }

    private void ListenForClueScroll()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            clueUp.onClick?.Invoke();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            clueDown.onClick?.Invoke();
            return;
        }
    }

    private void ListenForPaintingScroll()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            paintingLeft.onClick?.Invoke();
            return;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            paintingRight.onClick?.Invoke();
            return;
        }
    }



    private void UpdateSliderRapidSelectDisplay()
    {
        foreach (var display in sliderRapidSelects)
        {
            display.sprite = blankDisplay;
        }
        sliderRapidSelects[currentSlider].sprite = litDisplay;
    }
    private void ListenForAudioStuff()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playButton.onClick?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            im.IncrementVolumeSliderViaRapidSelect(-1);
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            im.IncrementVolumeSliderViaRapidSelect(1);
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            autoplayToggle.onClick?.Invoke();
        }
    }



    private void ListenForArrowKeys()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentSlider--;
            currentSlider = Mathf.Clamp(currentSlider, 0, Mathf.RoundToInt(sliderRapidSelects.Length - 1));
            UpdateSliderRapidSelectDisplay();
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentSlider++;
            currentSlider = Mathf.Clamp(currentSlider, 0, Mathf.RoundToInt(sliderRapidSelects.Length - 1));
            UpdateSliderRapidSelectDisplay();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            im.IncrementSliderViaRapidSelect(currentSlider, -1);
            UpdateSliderRapidSelectDisplay();
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            im.IncrementSliderViaRapidSelect(currentSlider, 1);
            UpdateSliderRapidSelectDisplay();
            return;
        }

    }
}
