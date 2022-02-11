using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : UI_Panel
{
    public void HandleStartButtonPress()
    {
        uic.SetContext(UI_Controller.Context.CoreGame);
        FindObjectOfType<FileManager>().InitializeGame();
        FindObjectOfType<InterfaceManager>().InitializeGame();
    }
}
