using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel : MonoBehaviour
{
    public List<GameObject> elements = new List<GameObject>();

    protected virtual void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            elements.Add(transform.GetChild(i).gameObject);
        }
        foreach (var elem in elements)
        {
            elem.SetActive(false);
        }
    }

    public virtual void ShowHideElements(bool shouldShow)
    {
        foreach (var elem in elements)
        {
            elem.SetActive(shouldShow);
        }
    }
}
