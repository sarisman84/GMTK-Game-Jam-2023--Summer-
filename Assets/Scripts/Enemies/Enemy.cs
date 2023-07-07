using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : Damagable
{
    public Vector3 velocity;

    public Transform target;
    public CharacterController controller { get; private set; }


    public float closeAttackDamage = 10.0f;
    public float closeAttackRad = 2.0f;
    public float closeAttackCooldown = 0.5f;
    private float closeAttackTime = 0.0f;

    private void Awake() {
        controller = GetComponent<CharacterController>();
        target = PollingStation.Get<PlayerController>().transform;
    }

    public void FixedUpdate() {
        if (!controller.isGrounded) {
            velocity += Physics.gravity * Time.fixedDeltaTime;
            controller.Move(velocity * Time.fixedDeltaTime);
        }
        else {
            velocity.y = 0;
        }
    }

    public virtual void Update() {
        if ((target.position - transform.position).sqrMagnitude < closeAttackRad*closeAttackRad) {//if the target is close enough
            Damagable other = target.GetComponent<Damagable>();
            if(other != null)
                CloseAttack(other);
        }

        closeAttackTime += Time.deltaTime;
    }

    public virtual bool CloseAttack(Damagable other) {
        if (closeAttackTime < closeAttackCooldown) return false;

        other.Hit(closeAttackDamage, this);
        closeAttackTime = 0.0f;
        return true;
    }


    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeAttackRad);
    }
}
