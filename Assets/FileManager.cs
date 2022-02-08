using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    InterfaceManager im;
    CipherEngine ce;
    [SerializeField] string[] fileSource = null;
    [SerializeField] Sprite[] spriteSource = null;

    //settings
    int maxStringLength = 46;
    char underscore = '_';

    //state
    int currentFile;
    int currentSprite;
    List<int[]> targetValues_files;
    List<int[]> targetValues_sprites;

    private void Start()
    {
        im = FindObjectOfType<InterfaceManager>();
        ce = FindObjectOfType<CipherEngine>();
        PrepareFiles();
        targetValues_files = GenerateTargetValues(fileSource);
        targetValues_sprites = GenerateTargetValues(spriteSource);

    }

    private void PrepareFiles()
    {
        for (int i = 0; i < fileSource.Length; i++)
        {
            if (fileSource[i].Length > maxStringLength)
            {
                fileSource[i].Remove(maxStringLength - 1);
            }

            while (fileSource[i].Length < maxStringLength)
            {
                fileSource[i] += underscore;
            }                
        }
    }

    private List<int[]> GenerateTargetValues(Array array)
    {
        targetValues_files = new List<int[]>();
        for (int i = 0; i < array.Length; i++)
        {
            int[] values = new int[3];
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = UnityEngine.Random.Range(0, im.MaxSettings);
            }
            targetValues_files.Add(values);
        }
        return targetValues_files;
    }

    public FilePack GetNextFile()
    {
        currentFile++;
        if (currentFile >= fileSource.Length)
        {
            currentFile = 0;
        }
        FilePack file = new FilePack(fileSource[currentFile], targetValues_files[currentFile]);
        return file;
    }
   
    public FilePack GetPreviousFile()
    {
        currentFile--;
        if (currentFile <0)
        {
            currentFile = fileSource.Length-1;
        }
        FilePack file = new FilePack(fileSource[currentFile], targetValues_files[currentFile]);
        return file;
    }

    public FilePack GetRandomFile()
    {
        int rand = UnityEngine.Random.Range(0, fileSource.Length);
        currentFile = rand;
        FilePack file = new FilePack(fileSource[currentFile], targetValues_files[currentFile]);
        return file;
    }

    public SpritePack GetRandomSprite()
    {
        int rand = UnityEngine.Random.Range(0, spriteSource.Length);
        currentSprite = rand;
        SpritePack pack = new SpritePack(spriteSource[currentSprite], targetValues_sprites[currentSprite]);
        return pack;
    }
}
