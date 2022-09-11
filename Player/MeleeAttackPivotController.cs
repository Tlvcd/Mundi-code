using System;
using UnityEngine;

public class MeleeAttackPivotController : MonoBehaviour
{
    [SerializeField]
    PlayerState state;

    private void OnEnable()
    {
        state.OnPlayerChangeDirection += AdjustPivotRotation;
    }

    private void OnDisable()
    {
        state.OnPlayerChangeDirection -= AdjustPivotRotation;
    }

    private void AdjustPivotRotation()
    {
        int dir = state.GetPlayerDirection();
        transform.rotation = Quaternion.Euler(0,0, -45*dir);
    }
}
