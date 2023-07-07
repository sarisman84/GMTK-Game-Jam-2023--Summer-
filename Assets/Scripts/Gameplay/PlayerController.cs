using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour, IManager {
    public float movementSpeed;
    public float attackSpeed;
    public float maxHealth;
    public float invurnabilityDurationOnDamageTakenInSeconds;
    public float basicAttackRange;
    public float basicAttackDamage;


    private float currentHealth;
    private float currentInvurnabilityDuration;


    private CharacterController controller;
    private Vector3 currentMovement;


    public List<BaseUpgradeObject> activeUpgrades;

    private void Awake()
    {
        controller = GetComponent<CharacterController>() != null ? GetComponent<CharacterController>() : gameObject.AddComponent<CharacterController>();
        Spawn();
    }



    public void Spawn()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float someDamage)
    {
        if(currentInvurnabilityDuration <= 0)
        {
            currentInvurnabilityDuration = invurnabilityDurationOnDamageTakenInSeconds;
            currentHealth -= someDamage;

            if (currentHealth < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            currentInvurnabilityDuration -= Time.deltaTime;
        }

        
    }

    private void Movement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();

        currentMovement.x = input.x * movementSpeed * Time.deltaTime;
        currentMovement.z = input.y * movementSpeed * Time.deltaTime;
        controller.Move(currentMovement);

        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }


    private void Attack()
    {
       //var foundColliders =  Physics.OverlapSphere(transform.position, basicAttackRange);


       // foreach (var item in foundColliders)
       // {
       //     if (item.GetComponent<Damagable>())
       //     {
       //         var damageable = item.GetComponent<Damagable>();

       //         damageable.Hit(basicAttackDamage, gameObject);
       //     }
       // }
    }

    private void Update()
    {
        Movement();
        //Attack();
    }

    public void OnLoad()
    {
       
    }
}
