using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{

    [SerializeField]
    UnityEvent Trigger;
    private bool activated;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (activated || !collision.CompareTag("Player")) return;

        activated = true;
        Trigger.Invoke();

    }
}
