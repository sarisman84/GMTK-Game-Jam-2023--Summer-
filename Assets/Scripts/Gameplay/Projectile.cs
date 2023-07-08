using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 rotVelocity;

    [Space]

    public bool destroyOnHit = true;
    public bool friendlyFire = false;

    [Space]

    public float damage;

    private MonoBehaviour shooter;

    public void FixedUpdate() {
        if (!GameplayManager.Get.runtimeActive) return;
        transform.position += velocity * Time.fixedDeltaTime;
        transform.Rotate(rotVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        bool hit;
        Damagable damagable = other.GetComponent<Damagable>();
        if(damagable)
            hit = Hit(damagable);
        else {
            HitLifeless(other);
            hit = true;
        }

        if (destroyOnHit && hit) {
            Destroy(gameObject);
        }
    }

    public virtual bool Hit(Damagable other) {
        other.Hit(damage, shooter);
        return true;
    }

    public virtual void HitLifeless(Collider other) {}
}
