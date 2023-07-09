using System;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    float speed = 3.0f;

    public override void OnUpdate() {
        base.OnUpdate();

        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        velocity = dir * speed;
    }

}
