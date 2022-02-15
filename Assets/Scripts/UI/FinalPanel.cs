using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinalPanel : UI_Panel
{
    CaseManager cm;
    [SerializeField] TextMeshProUGUI title = null;
    [SerializeField] TextMeshProUGUI body = null;

    [SerializeField] string successTitle = null;
    [SerializeField] string successBody = null;
    [SerializeField] string loseTitle = null;
    [SerializeField] string loseBody = null;

    [SerializeField] TextMeshProUGUI[] textClues = null;
    [SerializeField] TextMeshProUGUI[] audioClues = null;
    [SerializeField] Image[] spriteClues = null;
    [SerializeField] Image painting = null;

    protected override void Start()
    {
        base.Start();
        cm = FindObjectOfType<CaseManager>();
    }

    public void HandleRestartPress()
    {
        uic.SetContext(UI_Controller.Context.Start);
    }

    public override void Activate()
    {
        base.Activate();
        if (cm.WasLastCaseSuccess)
        {
            title.text = successTitle;
            body.text = successBody;
        }
        else
        {
            title.text = loseTitle;
            body.text = loseBody;
        }
        CaseFile previousCaseFile = cm.GetCaseFile();

        painting.sprite = previousCaseFile.GetForgery();

        for (int i = 0; i< previousCaseFile.SpriteClues_Shuffled.Length; i++)
        {
            spriteClues[i].sprite = previousCaseFile.SpriteClues_Shuffled[i];
        }

        for (int i = 0; i < previousCaseFile.TextClues_Shuffled.Length; i++)
        {
            textClues[i].text = previousCaseFile.TextClues_Shuffled[i];
        }

        for (int i = 0; i < previousCaseFile.AudioClueTranscripts.Length; i++)
        {
            audioClues[i].text = previousCaseFile.AudioClueTranscripts[i];
        }

    }
}
