using Unity.VisualScripting;
using UnityEngine;

public struct ProjectileDesc<T> where T : MonoBehaviour {
    public GameObject projectilePrefab;
    public T shooter;
    public Vector3 spawnPoint;
    public Quaternion spawnRotation;
    public Vector3 velocity;
    public float damage;
}

public class Projectile : MonoBehaviour {
    public Vector3 velocity;
    public Vector3 rotVelocity;

    [Space]

    public bool destroyOnHit = true;
    public bool friendlyFire = false;

    [Space]

    public float damage;

    private MonoBehaviour shooter;
    private Collider col;


    public static Projectile Create<T>(ProjectileDesc<T> aDesc) where T : MonoBehaviour
    {
        if(!aDesc.projectilePrefab)
        {
            Debug.LogError($"Invalid prefab assigned!");
            return null;
        }

        GameObject newProjectile = Instantiate(aDesc.projectilePrefab);
        newProjectile.transform.position = aDesc.spawnPoint;
        newProjectile.transform.rotation = aDesc.spawnRotation;

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        if(!projectile)
        {
            Debug.LogError($"No projectile component exists on prefab {aDesc.projectilePrefab.name}", aDesc.projectilePrefab);
            return null;
        }
        PollingStation.Get<TeamsManager>().AddToTeam<T>(newProjectile.gameObject);

        projectile.damage = aDesc.damage;
        projectile.velocity = aDesc.velocity;
        projectile.shooter = aDesc.shooter;
        return projectile;

    }

    private void Awake()
    {
        col = GetComponent<Collider>() != null ? GetComponent<Collider>() : gameObject.AddComponent<SphereCollider>();
    }

    public void FixedUpdate()
    {
        if (!BackendManager.Get.runtimeActive) return;
        transform.position += velocity * Time.fixedDeltaTime;
        transform.Rotate(rotVelocity * Time.fixedDeltaTime);



        //var foundColliders = Physics.OverlapSphere(transform.position, col.bounds.extents.x);
        //for (int i = 0; i < foundColliders.Length; i++)
        //{
        //    OnTriggerEnter(foundColliders[i]);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hit;
        Damagable damagable = other.GetComponent<Damagable>();

        // AUDIO: Play Divine Thrusts SFX
        FindObjectOfType<AudioManager>().PlayRandomSound("divinethrust01", "divinethrust02", "divinethrust03", "divinethrust04");

        if (damagable)
            hit = Hit(damagable);
        else
        {
            HitLifeless(other);
            hit = true;
        }
        if (destroyOnHit && hit)
        {
            Destroy(gameObject);
        }
    }

    public virtual bool Hit(Damagable other)
    {
        other.Hit(damage, shooter);
        
        return true;
    }

    public virtual void HitLifeless(Collider other) { }
}
