using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField]
    TabButton defaultSelection;


    private TabButton selected;

    private void Start()
    {
        if (defaultSelection)
        {
            ButtonSelected(defaultSelection);
            defaultSelection = null;
        }
    }
    public void ButtonSelected(TabButton button)
    {
        selected?.OnDeselected();

        selected = button;
        selected.OnSelected();
    }
}
