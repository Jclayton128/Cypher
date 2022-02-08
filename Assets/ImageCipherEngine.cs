using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCipherEngine : CipherEngine
{
    [SerializeField] Sprite spriteSource = null;

    //state
    Color[] arrayPlain;
    Texture2D textureDisplay;
    int height;
    int width;
    Rect screenRect;

    protected override void Start()
    {
        base.Start();
        InitializeDisplayTexture();
        PreparePlainTexture();
        ConvertArrayPlainToDisplaySprite();
    }

    private void InitializeDisplayTexture()
    {

        width = Mathf.RoundToInt(displayImage.GetComponent<RectTransform>().rect.width);
        height = Mathf.RoundToInt(displayImage.GetComponent<RectTransform>().rect.height);
        screenRect = displayImage.GetComponent<RectTransform>().rect;
        textureDisplay = new Texture2D(width, height);
        Debug.Log($"rect: {screenRect}");
        Debug.Log($"height: {height}, width: {width}");
        //textureDisplay.height = height;
    }

    private void PreparePlainTexture()
    {
        arrayPlain = spriteSource.texture.GetPixels(0, 0, width, height);
        Debug.Log($"{arrayPlain[0]}, {arrayPlain[10]}, {arrayPlain[60]}. length: {arrayPlain.Length}");
    }

    public void HandleSlider0()
    {
        if (slider0.value > 5)
        {
            for (int i = 0; i < arrayPlain.Length; i++)
            {
                arrayPlain[i] = Color.black;
            }
        }

        ConvertArrayPlainToDisplaySprite();
    }

    private void ConvertArrayPlainToDisplaySprite()
    {
        textureDisplay.SetPixels(0, 0, width, height, arrayPlain);
        textureDisplay.Apply();
        //Rect newRect = new Rect(new Vector2(-50, -50), new Vector2(240, 240));
        //Sprite display = Sprite.Create(textureDisplay, newRect, Vector2.zero);
;        displayImage.texture = textureDisplay;
    }
}
