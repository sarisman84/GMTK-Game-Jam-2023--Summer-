using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IManager {
    public float movementSpeed;
    public float attackSpeed;
    public float maxHealth;
    public float invurnabilityDurationOnDamageTakenInSeconds;

    private float currentHealth;
    private float currentInvurnabilityDuration;


    private CharacterController controller;
    private Vector3 currentMovement;

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



    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();

        currentMovement.x = input.x * movementSpeed * Time.deltaTime;
        currentMovement.z = input.y * movementSpeed * Time.deltaTime;

        controller.Move(currentMovement);
    }

    public void OnLoad() {}
}
