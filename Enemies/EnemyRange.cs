using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class EnemyRange : MonoBehaviour
{
    public event Action OnPlayerInRange, OnPlayerLeftRange;
    public GameObject PlayerObj { get; private set; }

    public Transform PlayerTransform { get; private set; }

    public bool PlayerInRange { get; private set; }

    private CircleCollider2D collider;

    [SerializeField] private float fightRange;
    private float initialRange;

    private void Awake()
    {
        collider = this.GetComponent<CircleCollider2D>();
        initialRange = collider.radius;
    }

    private void Start()
    {
        collider.radius = initialRange;
    }

    private void OnDisable()
    {
        MusicManager.instance.StopPlaying();
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        OnDetect(col);
    }

    public void LostPlayer()
    {
        PlayerObj = null;
        PlayerTransform = null;
        PlayerInRange = false;
        collider.radius = initialRange;
          
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || PlayerObj !=null) return;

        OnDetect(collision);
    }

    private void OnDetect(Collider2D col)
    {
        PlayerObj = col.gameObject;
        PlayerTransform = PlayerObj.transform;
        PlayerInRange = true;

        OnPlayerInRange?.Invoke();

        collider.radius = fightRange;
        MusicManager.instance.SwitchRegion("Battle");

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerObj = null;
        PlayerTransform = null;
        PlayerInRange = false;
        OnPlayerLeftRange?.Invoke();
        collider.radius = initialRange;
        MusicManager.instance.StopPlaying();
    }

}
