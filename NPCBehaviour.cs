using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class NPCBehaviour : MonoBehaviour
{
    [SerializeField, Range(0,7)] private int defaultDirection;
    [SerializeField] private bool faceOnEnter, alwaysFacePlayer, faceDefaultAuto;

    [SerializeField] private float resetDelay;

    [Header("Internal")]
    [SerializeField] private AnimationClip[] idlePoses= new AnimationClip[8];

    [SerializeField]
    private int[] hashList = new int[8];
    private void OnValidate()
    {
        for (int i = 0; i < 8; i++)
        {
            hashList[i] = Animator.StringToHash(idlePoses[i].name);
        }
    }

    [SerializeField] private Animator animator;


    private GameObject player;
    private readonly Vector2 defaultDir = new Vector2(0,1);

    private void Awake()
    {
        FaceDefault();
    }

    public void FaceDefault()
    {
        animator.Play(hashList[defaultDirection]);
    }

    public void FaceDirection(int dir)
    {
        animator.Play(hashList[dir]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        player = collision.gameObject;

        if (alwaysFacePlayer)
        {
            StartCoroutine(AlwaysFacePlayer());
            return;
        }

        if(faceOnEnter) FacePlayer();

    }

    private async void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = null;
        StopAllCoroutines();


        if (faceDefaultAuto)
        {
            await Task.Delay((int)resetDelay*1000);
            FaceDefault();
        }
    }

    public void FacePlayer()
    {
        var dir = CalcVector2Dir(player.transform.position- transform.position);

        animator.Play(hashList[dir]);
    }

    private int CalcVector2Dir(Vector2 vec)
    {
        vec.Normalize();

        var dir = Vector2.SignedAngle(vec, defaultDir);
        // ReSharper disable once PossibleLossOfFraction
        dir = (int)(dir / 43);
        return _ = dir < 0 ? (int)dir + 8 : (int)dir;
    }


    IEnumerator AlwaysFacePlayer()
    {
        while (alwaysFacePlayer)
        {
            FacePlayer();
            yield return null;
        }
    }

}
