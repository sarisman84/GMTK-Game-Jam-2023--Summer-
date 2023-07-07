using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Damagable, IManager {
   
    public float baseMovementSpeed;
    [HideInInspector] public float movementSpeed;

    private UpgradeManager upgradeManager;
    private CharacterController controller;
    private Vector3 currentMovement;


    //public List<BaseWeaponUpgrade> activeWeapons;

    private void Awake()
    {
        movementSpeed = baseMovementSpeed;
        upgradeManager = GetComponent<UpgradeManager>();
        controller = GetComponent<CharacterController>() != null ? GetComponent<CharacterController>() : gameObject.AddComponent<CharacterController>();

        //Start with a basic weapon
        upgradeManager.GainWeapon(0);
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


    private void Update()
    {
        Movement();
        //Attack();
    }

    public void OnLoad()
    {
       
    }
}
