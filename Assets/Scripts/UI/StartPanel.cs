using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StartPanel : UI_Panel
{
    CaseManager cm;
    [SerializeField] TextMeshProUGUI[] caseTMPs = null;
    [SerializeField] TextMeshProUGUI startTMP = null;

    //state
    int selectedCase = -1;

    protected override void Start()
    {
        base.Start();
        cm = FindObjectOfType<CaseManager>();
        PrepCaseNames();
    }

    private void PrepCaseNames()
    {
        string[] names = cm.GetCaseNames();
        for (int i = 0; i < names.Length; i++)
        {
            caseTMPs[i].text = names[i];
        }
    }

    public void HandleCaseSelect(int index)
    {
        selectedCase = index;
        UpdateStartButtonWithCaseName();
    }

    public void HandleStartButtonPress()
    {
        uic.SetContext(UI_Controller.Context.CoreGame);
        cm.BeginCaseAtIndex(selectedCase);
        //FindObjectOfType<FileManager>().InitializeGame();
        FindObjectOfType<InterfaceManager>().InitializeGame();
    }

    private void UpdateStartButtonWithCaseName()
    {
        if (selectedCase >= 0)
        {
            startTMP.text = "Solve " + caseTMPs[selectedCase].text;
        }
        else
        {
            startTMP.text = "Select a Case";
        }
    }


}
