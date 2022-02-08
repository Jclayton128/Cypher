using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCipherEngine : CipherEngine
{
    [SerializeField] Sprite spriteSource = null;

    //state
    Sprite spritePlain;

    void Start()
    {
        spritePlain = spriteSource;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
