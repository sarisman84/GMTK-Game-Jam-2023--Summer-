using UnityEngine;

public class SimpleEnemy : Enemy
{
    float speed = 3.0f;

    void Update() {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        controller.Move(dir * speed * Time.deltaTime);
    }    
}
