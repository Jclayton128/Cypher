using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseManager : MonoBehaviour
{
    [SerializeField] CaseFile[] caseFiles = null;
    FileManager fm;
    UI_Controller uic;

    //state
    int activeCaseIndex = 0;
    bool wasLastCaseSuccess = false;

    private void Start()
    {
        fm = FindObjectOfType<FileManager>();
        uic = FindObjectOfType<UI_Controller>();
    }

    public void BeginCaseAtIndex(int indexOfCase)
    {
        activeCaseIndex = indexOfCase;
        fm.SetNewCaseFile(caseFiles[indexOfCase]);
    }

    public void HandleGuessOnForgery(Sprite paintingBeingShown)
    {
        if (paintingBeingShown == caseFiles[activeCaseIndex].GetForgery())
        {
            Debug.Log($"pbs: {paintingBeingShown}, forgery: {caseFiles[activeCaseIndex].GetForgery()}");
            Debug.Log("you are right!");
            wasLastCaseSuccess = true;
        }
        else
        {
            Debug.Log("You are wrong :(");
            wasLastCaseSuccess = false;
        }
        uic.SetContext(UI_Controller.Context.Finish);
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
