using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextClue")]
public class TextClue : ScriptableObject
{

    [SerializeField] string clue = "default clue";

    public int SliderValue_0 = 0;
    public int SliderValue_1 = 0;
    public int SliderValue_2 = 0;

    public void ResetSliderValues()
    {
        SliderValue_0 = 0;
        SliderValue_1 = 0;
        SliderValue_2 = 0;
    }


}
