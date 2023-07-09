using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Enemy : Damagable {
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public float gravSpeed = 0.0f;
    public float gravityScale = 1.0f;

    [HideInInspector] public Transform target;
    public CharacterController controller { get; private set; }

    public float amountOfDroppedExperience = 0;
    public float closeAttackDamage = 10.0f;
    public float closeAttackRad = 2.0f;
    public float closeAttackCooldown = 0.5f;
    private float closeAttackTime = 0.0f;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        target = PollingStation.Get<PlayerController>().transform;

        PollingStation.Get<TeamsManager>().AddToTeam<Enemy>(gameObject);
    }

    public void FixedUpdate()
    {
        if (!BackendManager.Get.runtimeActive) return;
        if (!controller.isGrounded)
        {
            gravSpeed += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;
        }
        else
        {
            gravSpeed = 0;
        }

        controller.Move(velocity * Time.fixedDeltaTime + Vector3.up * gravSpeed);

        transform.rotation = Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z), Vector3.up);
    }

    private void Update()
    {
        if (!BackendManager.Get.runtimeActive) return;
        if ((target.position - transform.position).sqrMagnitude < closeAttackRad * closeAttackRad)
        {//if the target is close enough
            Damagable other = target.GetComponent<Damagable>();
            if (other != null)
                CloseAttack(other);
        }

        closeAttackTime += Time.deltaTime;

        OnUpdate();
    }

    public virtual void OnUpdate() {}

    public virtual bool CloseAttack(Damagable other)
    {
        if (closeAttackTime < closeAttackCooldown) return false;

        other.Hit(closeAttackDamage, this);
        closeAttackTime = 0.0f;
        return true;
    }

    public override void OnDeath(MonoBehaviour attacker)
    {
        Destroy(gameObject);

        PollingStation.Get<UpgradeManager>().AddExperience(amountOfDroppedExperience);
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeAttackRad);
    }
}
