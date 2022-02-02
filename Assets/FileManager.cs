using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    Encrypter2 ce;
    [SerializeField] string[] fileSource = null;

    //settings
    int maxStringLength = 46;
    char underscore = '_';

    //state
    int currentFile;
    List<int[]> targetValues;

    private void Start()
    {
        ce = FindObjectOfType<Encrypter2>();
        PrepareFiles();
        GenerateTargetValues();

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

    private void GenerateTargetValues()
    {
        targetValues = new List<int[]>();
        for (int i = 0; i < fileSource.Length; i++)
        {
            int[] values = new int[3];
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = UnityEngine.Random.Range(0, ce.MaxSettings);
            }
            targetValues.Add(values);
        }
    }

    public FilePack GetNextFile()
    {
        currentFile++;
        if (currentFile >= fileSource.Length)
        {
            currentFile = 0;
        }
        FilePack file = new FilePack(fileSource[currentFile], targetValues[currentFile]);
        return file;
    }
   
    public FilePack GetPreviousFile()
    {
        currentFile--;
        if (currentFile <0)
        {
            currentFile = fileSource.Length-1;
        }
        FilePack file = new FilePack(fileSource[currentFile], targetValues[currentFile]);
        return file;
    }

    public FilePack GetRandomFile()
    {
        int rand = UnityEngine.Random.Range(0, fileSource.Length);
        currentFile = rand;
        FilePack file = new FilePack(fileSource[currentFile], targetValues[currentFile]);
        return file;
    }
}
