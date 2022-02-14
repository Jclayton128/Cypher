using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseManager : MonoBehaviour
{
    [SerializeField] CaseFile[] caseFiles = null;
    FileManager fm;


    private void Start()
    {
        fm = FindObjectOfType<FileManager>();
    }

    public void BeginCaseAtIndex(int indexOfCase)
    {
        fm.SetNewCaseFile(caseFiles[indexOfCase]);
    }

    public string[] GetCaseNames()
    {
        string[] names = new string[caseFiles.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = caseFiles[i].GetCaseName();
        }
        return names;
    }
}
