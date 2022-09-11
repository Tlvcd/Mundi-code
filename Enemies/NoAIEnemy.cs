using Axis.Abstractions;
using UnityEngine;
using Pathfinding;


public class NoAIEnemy : BaseEnemy
{
    


    [SerializeField]
    DamageType damageType;

    //na przyszlosc
    //[SerializeField]
    //AIPath ai;

    float timer;

    private void Update()
    {
        if (timer > 5f)
        {
            AttackNearby(damageType);
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    
}
