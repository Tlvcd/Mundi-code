using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour
{
    private Vector3 target = new Vector3(0, 0, 0); //target position

    [SerializeField] private CanvasGroup groupAlpha;
    [SerializeField] private GameObject border, pointerArrow;
    [SerializeField] private TMP_Text distanceText;

    private RectTransform parentRect;
    private CanvasGroup borderGroup;

    private bool leaning;


    public void SetTarget(Vector3 pos) => target = pos;

    private Camera cam;

    private float maxY;

    private void Awake()
    {
        
        cam = Camera.main; //cache camera

        borderGroup = border.GetComponent<CanvasGroup>(); //get canvasgroup for border anim

        LeanTween.scale(border, Vector3.one * 4, 1).setEaseOutQuint().setLoopClamp();
        LeanTween.alphaCanvas(borderGroup, 0, 1).setEaseOutQuint().setLoopClamp();//set animation loops for border

        parentRect = transform.parent.GetComponent<RectTransform>();
        //maxY = ;//cache parent rect values

    }

    private void Start()
    {
        leaning = true;
        groupAlpha.alpha = 0;
        LeanTween.alphaCanvas(groupAlpha, 1, 0.35f).setEaseOutQuint().setOnComplete(() => { leaning = false; }); //fade anim on awake
    }

    private void LateUpdate()
    {

        var point = cam.WorldToScreenPoint(target);
        var camPos = cam.transform.position;


        RectTransformUtility
            .ScreenPointToLocalPointInRectangle
                (parentRect, point, null, out var canvasPos); //target screen to parent rect

        canvasPos = Vector3.ClampMagnitude(canvasPos, parentRect.rect.max.y);
        transform.localPosition = canvasPos; //set position inside parent

        var dir = (target - camPos).normalized;
        pointerArrow.transform.localEulerAngles = new Vector3(0,0,(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360); //set arrow rotation



        if (leaning) return;
        var distance = Vector2.Distance(target, camPos);
        groupAlpha.alpha = distance/7; //alpha when player close

        distanceText.text = (distance/1.3f).ToString("0") + "m";


    }
}
