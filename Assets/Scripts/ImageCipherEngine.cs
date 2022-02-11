using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCipherEngine : CipherEngine
{
    Material screenMaterial;

    //state
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
    }

    
    private void HandleSlider0()
    {
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

    public override void Obfuscate()
    {
        if (!isReadyToObfuscate)
        {
            Debug.Log("Not ready to obfuscate");
            return;
        }
        HandleSlider0();
        HandleSlider1();
        HandleSlider2();
    }

    public override void InitializeNewFile(System.Object newObject)
    {
        SpritePack spritePack = (SpritePack)newObject;
        im.UpdateDisplay(spritePack.SpritePlain);
        targetVal_0 = spritePack.TargetValues[0];
        targetVal_1 = spritePack.TargetValues[1];;
        targetVal_2 = spritePack.TargetValues[2];

        isReadyToObfuscate = true;

        Obfuscate();
    }
}
