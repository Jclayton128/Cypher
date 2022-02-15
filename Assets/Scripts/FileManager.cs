using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    InterfaceManager im;
    UI_Controller uic;
    TextCipherEngine textCE;
    ImageCipherEngine imageCE;
    AudioCipherEngine audioCE;


    string[] textSupply = null;
    Sprite[] spriteSupply = null;
    AudioClue[] audioSupply = null;
    List<Sprite> paintings = new List<Sprite>();
   

    //settings
    int maxStringLength = 46;
    char underscore = ' ';

    //state
    CaseFile currentCaseFile = null;
    int currentText;
    int currentSprite;
    int currentAudio;
    int currentPainting;
    List<int[]> targetValues_text;
    List<int[]> targetValues_sprites;
    List<int[]> targetValues_audio;

    List<int[]> sliderState_text = new List<int[]>();
    List<int[]> sliderState_sprites = new List<int[]>();
    List<int[]> sliderState_audio = new List<int[]>();
    private void Start()
    {
        im = FindObjectOfType<InterfaceManager>();
        im.OnModeChanged += HandleModeChange;
        textCE = FindObjectOfType<TextCipherEngine>();
        imageCE = FindObjectOfType<ImageCipherEngine>();
        audioCE = FindObjectOfType<AudioCipherEngine>();
        uic = FindObjectOfType<UI_Controller>();
        //InitializeGame();
    }

    public void InitializeGame()
    {
        LoadCaseFile();

        PrepareTextFiles();
        targetValues_text = GenerateTargetValues(textSupply);
        targetValues_sprites = GenerateTargetValues(spriteSupply);
        targetValues_audio = GenerateTargetValues(audioSupply);

        sliderState_text = PrepareSliderState(textSupply);
        sliderState_sprites = PrepareSliderState(spriteSupply);
        sliderState_audio = PrepareSliderState(audioSupply);

        SetInitialText();
        SetInitialSprite();
        SetInitialAudio();

        currentPainting = 0;
        im.UpdatePainting(paintings[currentPainting]);
    }

    private void LoadCaseFile()
    {
        currentCaseFile.PrepareCase();
        audioSupply = currentCaseFile.AudioClues_Shuffled;
        textSupply = currentCaseFile.TextClues_Shuffled;
        spriteSupply = currentCaseFile.SpriteClues_Shuffled;
        paintings.Clear();
        foreach (var painting in currentCaseFile.Paintings_Shuffled)
        {
            paintings.Add(painting);
        }
        PushPaintingIndexToDisplay();
    }


    private void PrepareTextFiles()
    {
        for (int i = 0; i < textSupply.Length; i++)
        {
            if (textSupply[i].Length > maxStringLength)
            {
                textSupply[i].Remove(maxStringLength - 1);
            }

            while (textSupply[i].Length < maxStringLength)
            {
                textSupply[i] += underscore;
            }                
        }
    }

    private List<int[]> GenerateTargetValues(Array array)
    {
        List<int[]> targetValues_this = new List<int[]>();
        for (int i = 0; i < array.Length; i++)
        {
            int[] values = new int[3];
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = UnityEngine.Random.Range(0, im.MaxSettings);
            }
            targetValues_this.Add(values);
        }
        return targetValues_this;
    }

    private List<int[]> PrepareSliderState(Array array)
    {
        List<int[]> list = new List<int[]>();
        for (int i = 0; i < array.Length; i++)
        {
            int[] slidervalues = new int[3] { 0, 0, 0 };
            list.Add(slidervalues);
        }
        return list;
    } 

    private void HandleModeChange(InterfaceManager.Mode newMode)
    {
        im.UpdateDisplayMode(newMode);
        PushFileIndexToDisplay(newMode);
        switch (newMode)
        {
            case InterfaceManager.Mode.Text:
                im.DriveSlidersToCurrentFilePreviousSetting(sliderState_text[currentText]);
                return;
            case InterfaceManager.Mode.Audio:
                im.DriveSlidersToCurrentFilePreviousSetting(sliderState_audio[currentAudio]);
                return;
            case InterfaceManager.Mode.Sprite:
                im.DriveSlidersToCurrentFilePreviousSetting(sliderState_sprites[currentSprite]);
                return;

        }

    }

    private void SetInitialText()
    {
        int rand = UnityEngine.Random.Range(0, textSupply.Length);
        currentText = rand;
        PushCurrentText();
    }

    private void SetInitialSprite()
    {
        int rand = UnityEngine.Random.Range(0, spriteSupply.Length);
        currentSprite = rand;
        PushCurrentSprite();
    }
    private void SetInitialAudio()
    {
        int rand = UnityEngine.Random.Range(0, audioSupply.Length);
        currentAudio = rand;
        PushCurrentAudio();
    }

    private void PushFileIndexToDisplay(InterfaceManager.Mode currentMode)
    {
        switch (currentMode)
        {
            case InterfaceManager.Mode.Sprite:
                im.UpdateFileIndexDisplay($"{currentSprite+1}/{spriteSupply.Length}");

                return;

            case InterfaceManager.Mode.Text:
                im.UpdateFileIndexDisplay($"{currentText+1}/{textSupply.Length}");
                return;

            case InterfaceManager.Mode.Audio:
                im.UpdateFileIndexDisplay($"{currentAudio + 1}/{audioSupply.Length}");
                return;
        }
    }

    private void PushPaintingIndexToDisplay()
    {
        im.UpdatePaintingIndexDisplay($"{currentPainting+1}/{paintings.Count}");
    }

    private void PushCurrentText()
    {
        TextPack file = new TextPack(textSupply[currentText], targetValues_text[currentText]);
        textCE.InitializeNewFile(file);
    }
    private void PushCurrentSprite()
    {
        SpritePack spritePack = new SpritePack(spriteSupply[currentSprite], targetValues_sprites[currentSprite]);
        imageCE.InitializeNewFile(spritePack);
    }

    private void PushCurrentAudio()
    {
        AudioPack audioPack = new AudioPack(audioSupply[currentAudio].GetShuffledClips(), targetValues_audio[currentAudio]);
        audioCE.InitializeNewFile(audioPack);
    }


    #region Public Methods
    
    public void SetNewCaseFile(CaseFile newCase)
    {
        currentCaseFile = newCase;
        InitializeGame();
    }


    public void HandleGotoNextPainting()
    {
        currentPainting++;
        if (currentPainting >= paintings.Count)
        {
            currentPainting = 0;
        }

        im.UpdatePainting(paintings[currentPainting]);
        PushPaintingIndexToDisplay();
    }

    public void HandleGotoPreviousPainting()
    {
        currentPainting--;
        if (currentPainting < 0)
        {
            currentPainting = paintings.Count - 1;
        }
        im.UpdatePainting(paintings[currentPainting]);
        PushPaintingIndexToDisplay();
    }


    public void ReceiveUpdatedSliderValues(InterfaceManager.Mode currentMode, int[] values)
    {
        switch (currentMode)
        {
            case InterfaceManager.Mode.Text:
                for (int i = 0; i < sliderState_text[currentText].Length; i++)
                {
                    sliderState_text[currentText][i] = values[i];
                }
                return;

            case InterfaceManager.Mode.Sprite:
                for (int i = 0; i < sliderState_sprites[currentSprite].Length; i++)
                {
                    sliderState_sprites[currentSprite][i] = values[i];
                }
                return;

            case InterfaceManager.Mode.Audio:
                for (int i = 0; i < sliderState_audio[currentAudio].Length; i++)
                {
                    sliderState_audio[currentAudio][i] = values[i];
                }
                return;


        }


    }


    public void StepToNextFile(InterfaceManager.Mode currentMode)
    {
        if (currentMode == InterfaceManager.Mode.Text)
        {
            currentText++;
            if (currentText >= textSupply.Length)
            {
                currentText = 0;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentText();
            im.DriveSlidersToCurrentFilePreviousSetting(sliderState_text[currentText]);
            return;
        }

        if (currentMode == InterfaceManager.Mode.Sprite)
        {
            currentSprite++;
            if (currentSprite >= spriteSupply.Length)
            {
                currentSprite = 0;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentSprite();
            im.DriveSlidersToCurrentFilePreviousSetting(sliderState_sprites[currentSprite]);
            return;
        }

        if (currentMode == InterfaceManager.Mode.Audio)
        {
            currentAudio++;
            if (currentAudio >= audioSupply.Length)
            {
                currentAudio = 0;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentAudio();
            im.DriveSlidersToCurrentFilePreviousSetting(sliderState_audio[currentAudio]);
            return;
        }
    }  
    public void StepBackFile(InterfaceManager.Mode currentMode)
    {
        if (currentMode == InterfaceManager.Mode.Text)
        {
            currentText--;
            if (currentText < 0)
            {
                currentText = textSupply.Length - 1;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentText();
            im.DriveSlidersToCurrentFilePreviousSetting(sliderState_text[currentText]);
            return;
        }

        if (currentMode == InterfaceManager.Mode.Sprite)
        {
            currentSprite--;
            if (currentSprite < 0)
            {
                currentSprite = spriteSupply.Length - 1;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentSprite();
            im.DriveSlidersToCurrentFilePreviousSetting(sliderState_sprites[currentSprite]);
            return;
        }

        if (currentMode == InterfaceManager.Mode.Audio)
        {
            currentAudio--;
            if (currentAudio < 0)
            {
                currentAudio = audioSupply.Length - 1;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentAudio();
            im.DriveSlidersToCurrentFilePreviousSetting(sliderState_audio[currentAudio]);
            return;
        }

    }

    public void RemoveCurrentPainting()
    {
        paintings.RemoveAt(currentPainting);
        currentPainting--;
        if (currentPainting < 0)
        {
            currentPainting = paintings.Count - 1;
        }
        im.UpdatePainting(paintings[currentPainting]);
        PushPaintingIndexToDisplay();
        
    }

    #endregion
}
