using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCipherEngine : CipherEngine
{
    [SerializeField] Sprite spriteSource = null;

    Material screenMaterial;

    //state
    int targetVal_0 = 10;
    int targetVal_1 = 10;
    int targetVal_2 = 10;
    Image screenImage;

    Color[] arrayPlain;
    Texture2D textureDisplay;
    int height;
    int width;
    Rect screenRect;

    protected override void Start()
    {
        base.Start();
        //InitializeDisplayTexture();
       // PreparePlainTexture();
        //ConvertArrayPlainToDisplaySprite();
        screenMaterial = im.GetScreenMaterial();
        screenImage = im.GetScreenImage();
        Obfuscate();
    }

    private void InitializeDisplayTexture()
    {

        width = Mathf.RoundToInt(im.imageScreen.GetComponent<RectTransform>().rect.width);
        height = Mathf.RoundToInt(im.imageScreen.GetComponent<RectTransform>().rect.height);
        screenRect = im.imageScreen.GetComponent<RectTransform>().rect;
        textureDisplay = new Texture2D(width, height);
        //Debug.Log($"rect: {screenRect}");
        //Debug.Log($"height: {height}, width: {width}");
        //textureDisplay.height = height;
    }

    private void PreparePlainTexture()
    {
        arrayPlain = spriteSource.texture.GetPixels(0, 0, width, height);
        //Debug.Log($"{arrayPlain[0]}, {arrayPlain[10]}, {arrayPlain[60]}. length: {arrayPlain.Length}");
    }

    private void HandleSlider0()
    {
        //if (im.SliderValue_0 > 5)
        //{
        //    for (int i = 0; i < arrayPlain.Length; i++)
        //    {
        //        arrayPlain[i] = Color.black;
        //    }
        //}
        //else
        //{
        //    PreparePlainTexture();
        //}

        //ConvertArrayPlainToDisplaySprite();
        float val = Mathf.Abs(Mathf.Clamp((im.SliderValue_0 - targetVal_0),-2,im.MaxSettings)/ (float)im.MaxSettings);
        float val2 = Mathf.Abs(Mathf.Clamp((im.SliderValue_0 - targetVal_0), -im.MaxSettings, 1) / (float)im.MaxSettings);
        screenImage.materialForRendering.SetFloat("_FadeAmount", val);
        screenImage.materialForRendering.SetFloat("_ColorSwapBlend", val2);
    }

    private void HandleSlider1()
    {
        float val1 = Mathf.Abs(Mathf.Clamp((im.SliderValue_1 - targetVal_1), -2, im.MaxSettings) / (float)im.MaxSettings);
        screenImage.materialForRendering.SetFloat("_ChromAberrAmount", val1);
        screenImage.materialForRendering.SetFloat("_GradBlend", val1);

        float val2 = Mathf.Abs(Mathf.Clamp((im.SliderValue_1 - targetVal_1), -im.MaxSettings, 1) / (float)im.MaxSettings);
        screenImage.materialForRendering.SetFloat("_GreyscaleBlend", val2);
        float colLerp = Mathf.Lerp(50, 1, val2);
        float gamLerp = Mathf.Lerp(2,10, val2);
        screenImage.materialForRendering.SetFloat("_PosterizeNumColors", colLerp);
        screenImage.materialForRendering.SetFloat("_PosterizeGamma", gamLerp);
        screenImage.materialForRendering.SetFloat("_PosterizeNumColors", colLerp);
        screenImage.materialForRendering.SetFloat("_PosterizeGamma", gamLerp);
    }

    private void HandleSlider2()
    {
        float val1 = Mathf.Abs(Mathf.Clamp((im.SliderValue_2 - targetVal_2), 0, im.MaxSettings) / (float)im.MaxSettings);
        screenImage.materialForRendering.SetFloat("_DistortAmount", val1);
        //float zoom = Mathf.Lerp(1f, .3f, val1);
        //screenMaterial.SetFloat("_ZoomUvAmount", zoom);

        float val2 = Mathf.Abs(Mathf.Clamp((im.SliderValue_2 - targetVal_2), -im.MaxSettings, 0) / (float)im.MaxSettings);
        float pix = Mathf.Lerp(60, 4, val2);
        screenImage.materialForRendering.SetFloat("_PixelateSize", pix);
        float wave = Mathf.Lerp(0, 25, val2);
        //screenMaterial.SetFloat("_WaveAmount", wave);
        //screenMaterial.SetFloat("_WaveX", val2);
        //screenMaterial.SetFloat("_WaveY", 1-val2);
        float fisheye = Mathf.Lerp(0, 0.3f, val2);
        screenImage.materialForRendering.SetFloat("_FishEyeUvAmount", fisheye);

    }
    private void ConvertArrayPlainToDisplaySprite()
    {
        textureDisplay.SetPixels(0, 0, width, height, arrayPlain);
        textureDisplay.Apply();
        //Rect newRect = new Rect(new Vector2(-50, -50), new Vector2(240, 240));
        //Sprite display = Sprite.Create(textureDisplay, newRect, Vector2.zero);
;       //im.UpdateDisplay(textureDisplay);
    }


    public override void Obfuscate()
    {
        HandleSlider0();
        HandleSlider1();
        HandleSlider2();
    }
}
