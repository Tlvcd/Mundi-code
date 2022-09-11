using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class TabButton : MonoBehaviour, IPointerUpHandler ,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private TabGroup group;
    private bool selected, pointerInside;

    private Animator buttonAnimator;

    [SerializeField] private string Normal = "Normal", Highlighted = "Highlighted", Pressed= "Pressed", Selected= "Selected";
    [SerializeField]
    private int NormalHash, HighlightedHash, PressedHash, SelectedHash;

    [SerializeField] private UnityEvent OnSelect, OnDeselect;

    private void OnValidate()
    {
        NormalHash = Animator.StringToHash(Normal);
        HighlightedHash = Animator.StringToHash(Highlighted);
        PressedHash = Animator.StringToHash(Pressed);
        SelectedHash = Animator.StringToHash(Selected);
    }

    private void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInside = true;
        if (selected) return;
        buttonAnimator.Play(HighlightedHash);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInside = false;
        if (selected) return;
        buttonAnimator.Play(NormalHash);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (selected) return;
        buttonAnimator.Play(PressedHash);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (selected || !pointerInside) return;
        @group.ButtonSelected(this);
    }

    public void OnSelected()
    {
        selected = true;
        buttonAnimator.Play(SelectedHash);
        OnSelect.Invoke();
    }

    public void OnDeselected()
    {
        selected = false;
        buttonAnimator.Play(NormalHash);
        OnDeselect.Invoke();
    }

    public void SelectButton()
    {
        group.ButtonSelected(this);
    }
}
