using UnityEngine;
using Axis.Abstractions;
using System.Collections.Generic;

public class ThrowingKnifeProjectile : SpellBase
{
    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    float speed;


    float timer;

    [SerializeField]
    private List<string> ignoreTags = new List<string>();

    private void Update()
    {
        body.velocity = transform.TransformDirection(Vector2.up * speed);
        timer += Time.deltaTime;
        if (timer > 3f) Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var tag in ignoreTags)
        { if (collision.gameObject.CompareTag(tag)) { return; } }

        collision
            .gameObject
            .GetComponent<IDamagable>()
            ?.TakeDamage(stats.GetAttackFromType(damageType) * multiplier, damageType);

        Destroy(this.gameObject);
    }

}