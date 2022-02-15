using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] UI_Panel[] allPanels = null;
    [SerializeField] UI_Panel startPanel = null;
    [SerializeField] UI_Panel coreGamePanel = null;
    [SerializeField] UI_Panel finalPanel = null;

    public enum Context { Start, CoreGame, Finish};

    //state
    Context currentContext = Context.Start;

    private void Awake()
    {
        foreach(var panel in allPanels)
        {
            panel.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        SetContext(Context.Start);
    }

    public void SetContext(Context newContext)
    {
        foreach (var panel in allPanels)
        {
            panel.ShowHideElements(false);
        }

        switch (newContext)
        {
            case Context.Start:
                startPanel.Activate();
                return;

            case Context.CoreGame:
                coreGamePanel.Activate();
                return;

            case Context.Finish:
                finalPanel.Activate();
                return;
        }

    }
}
