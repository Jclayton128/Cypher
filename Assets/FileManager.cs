using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    InterfaceManager im;
    TextCipherEngine textCE;
    ImageCipherEngine imageCE;
    [SerializeField] string[] textSource = null;
    [SerializeField] Sprite[] spriteSource = null;

    //settings
    int maxStringLength = 46;
    char underscore = '_';

    //state
    public int currentText;
    public int currentSprite;
    List<int[]> targetValues_text;
    List<int[]> targetValues_sprites;

    private void Start()
    {
        im = FindObjectOfType<InterfaceManager>();
        im.OnModeChanged += HandleModeChange;
        textCE = FindObjectOfType<TextCipherEngine>();
        imageCE = FindObjectOfType<ImageCipherEngine>();
        PrepareTextFiles();
        targetValues_text = GenerateTargetValues(textSource);
        targetValues_sprites = GenerateTargetValues(spriteSource);

        SetInitialText();
        SetInitialSprite();
    }

    private void PrepareTextFiles()
    {
        for (int i = 0; i < textSource.Length; i++)
        {
            if (textSource[i].Length > maxStringLength)
            {
                textSource[i].Remove(maxStringLength - 1);
            }

            while (textSource[i].Length < maxStringLength)
            {
                textSource[i] += underscore;
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

    private void HandleModeChange(InterfaceManager.Mode newMode)
    {
        im.UpdateDisplayMode(newMode);
        PushFileIndexToDisplay(newMode);
    }

    private void SetInitialText()
    {
        int rand = UnityEngine.Random.Range(0, textSource.Length);
        currentText = rand;
        PushCurrentText();
    }

    private void SetInitialSprite()
    {
        int rand = UnityEngine.Random.Range(0, spriteSource.Length);
        currentSprite = rand;
        PushCurrentSprite();
    }
    private void PushFileIndexToDisplay(InterfaceManager.Mode currentMode)
    {
        switch (currentMode)
        {
            case InterfaceManager.Mode.Image:
                im.UpdateFileIndexDisplay($"{currentSprite+1}/{spriteSource.Length}");

                return;

            case InterfaceManager.Mode.Text:
                im.UpdateFileIndexDisplay($"{currentText+1}/{textSource.Length}");
                return;

            case InterfaceManager.Mode.Suspect:

                return;
        }
    }

    #region Public Methods

    public void PushCurrentText()
    {
        TextPack file = new TextPack(textSource[currentText], targetValues_text[currentText]);
        textCE.InitializeNewFile(file);
    }

    public void PushCurrentSprite()
    {
        SpritePack spritePack = new SpritePack(spriteSource[currentSprite], targetValues_sprites[currentSprite]);
        imageCE.InitializeNewFile(spritePack);
    }

   

    public void StepToNextFile(InterfaceManager.Mode currentMode)
    {
        if (currentMode == InterfaceManager.Mode.Text)
        {
            currentText++;
            if (currentText >= textSource.Length)
            {
                currentText = 0;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentText();
            return;
        }

        if (currentMode == InterfaceManager.Mode.Image)
        {
            currentSprite++;
            if (currentSprite >= spriteSource.Length)
            {
                currentSprite = 0;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentSprite();
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
                currentText = textSource.Length - 1;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentText();
            return;
        }

        if (currentMode == InterfaceManager.Mode.Image)
        {
            currentSprite--;
            if (currentSprite < 0)
            {
                currentSprite = spriteSource.Length - 1;
            }
            PushFileIndexToDisplay(currentMode);
            PushCurrentSprite();
            return;
        }

    }


    #endregion
}
