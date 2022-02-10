using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] UI_Panel[] allPanels = null;
    [SerializeField] UI_Panel startPanel = null;
    [SerializeField] UI_Panel coreGamePanel = null;
    [SerializeField] UI_Panel finishPanel = null;

    public enum Context { Start, CoreGame, Finish};

    //state
    Context currentContext = Context.Start;


    public void SetContext(Context newContext)
    {
        foreach (var panel in allPanels)
        {
            panel.ShowHideElements(false);
        }
        switch (newContext)
        {
            case Context.Start:
                startPanel.ShowHideElements(true);
                return;

            case Context.CoreGame:
                coreGamePanel.ShowHideElements(true);
                return;

            case Context.Finish:
                finishPanel.ShowHideElements(true);
                return;


        }

    }
}
