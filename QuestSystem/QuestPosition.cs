using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QuestPosition : QuestHandle
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!this.enabled 
            && !Retroactive) 
            || !collision.CompareTag("Player")) 
            return;
        

        SendCompletion();
        
    }


}
