
using UnityEngine;


public class PlayerAnimationInterpreter : MonoBehaviour
{
    [SerializeField]
    Animator playerAnimator;

    [SerializeField]
    PlayerAnimationAssets animas;

    private void OnEnable()
    {
        animas.OnAnimChange += PlayAnim;
        animas.OnAnimSpeedChange += ChangeSpeed;
    }

    private void OnDisable()
    {
        animas.OnAnimChange -= PlayAnim;
        animas.OnAnimSpeedChange -= ChangeSpeed;
    }

    private void PlayAnim()
    {
        playerAnimator.Play(animas.CurrentAnimHash, 0);
    }

    private void ChangeSpeed(float speed)
    {
        playerAnimator.speed = speed;
    }
}

