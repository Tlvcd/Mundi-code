using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthVignette : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private CanvasGroup vignette;
    [SerializeField] private AudioSource sfxSource;

    private LTDescr animLoop;
    private void OnEnable()
    {
        playerState.OnPlayerHit += HurtVignette;
        playerState.OnPlayerHeal += LowVignette;
    }

    private void OnDisable()
    {
        playerState.OnPlayerHit -= HurtVignette;
        playerState.OnPlayerHeal -= LowVignette;

    }

    private void HurtVignette()
    {
        LeanTween.alphaCanvas(vignette, vignette.alpha + 0.2f, 0.1f).setEaseInOutBounce().setLoopPingPong(1).setOnComplete(
            LowVignette);

    }

    private void LowVignette()
    {
        var health = playerState.HealthPercentage;
        if (animLoop != null) LeanTween.cancel(animLoop.uniqueId);

        if (health < 0.35f)
        {
            sfxSource.Play();
            sfxSource.pitch = 2f - health;
            LeanTween.alphaCanvas(vignette, 1 - health*2f, 0.2f).setEaseInCubic().setOnComplete(() =>
            {
                animLoop =LeanTween.alphaCanvas(vignette, vignette.alpha - 0.1f,Mathf.Clamp(health, 0.2f, 0.35f)).setEaseInOutCubic().setLoopPingPong();
            });
            return;
        }

        sfxSource.Stop();
        LeanTween.alphaCanvas(vignette, 0, 0.2f).setEaseInCubic();
    }
}
