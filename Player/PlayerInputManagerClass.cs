using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerClass : MonoBehaviour
{
    private static PlayerInputs inputs;

    private void Awake()
    {
        disableStack.Clear();
        inputs = new PlayerInputs();
        inputs.Enable();
    }


    private static Queue<int> disableStack = new Queue<int>();
    public static void DisablePlayerInput()
    {
        disableStack.Enqueue(0);

        inputs.BasePlayer.Disable();
    }

    public static void EnablePlayerInput()
    {
        if (disableStack.Count > 0)
        {
            disableStack.Dequeue();

        }

        if (disableStack.Count == 0)
        {
            inputs.BasePlayer.Enable();
        }
    }

    /// <summary>
    /// Obecny input gracza
    /// </summary>
    /// <returns>PlayerInput z bazowymi inputami gracza</returns>
    public static PlayerInputs GetInputClass() => inputs;
}
