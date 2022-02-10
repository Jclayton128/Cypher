using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuspectFile")]
public class SuspectFile : ScriptableObject
{
    [SerializeField] string suspectName = "default name";
    [SerializeField] Sprite suspectMugshot = null;
    [SerializeField] string[] suspectTraits = { "default 0", "default 1", "default 2" };

    public int CurrentSuspicion = 0;

    public string GetSuspectName()
    {
        return suspectName;
    }

    public Sprite GetSuspectMugshot()
    {
        return suspectMugshot;
    }

    public string[] GetSuspectTraits()
    {
        return suspectTraits;
    }

    public void ResetSuspicion()
    {
        CurrentSuspicion = 1;
    }
}
