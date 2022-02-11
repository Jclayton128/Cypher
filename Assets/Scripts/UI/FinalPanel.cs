using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPanel : UI_Panel
{

    public void HandleRestartPress()
    {
        uic.SetContext(UI_Controller.Context.Start);
    }
}
