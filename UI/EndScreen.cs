using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onEnd;

    CanvasGroup group;

    public async void EndGame()
    {
        gameObject.SetActive(true);
        group = GetComponent<CanvasGroup>();
        PlayerInputManagerClass.DisablePlayerInput();
        MusicManager.instance.StopPlaying();

        LeanTween.alphaCanvas(group, 1, 0.5f).setEaseInOutCubic();

        await Task.Delay(2500);

        onEnd.Invoke();
    }
}
