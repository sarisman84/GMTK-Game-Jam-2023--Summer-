using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Damagable
{
    public float movementSpeed;
    public float attackSpeed;

    private Vector3 currentMovement;

    private CharacterController controller;

    private PlayerController player;
    private UpgradeManager upgradeManager;

    private void Awake()
    {
        controller = GetComponent<CharacterController>() != null ? GetComponent<CharacterController>() : gameObject.AddComponent<CharacterController>();
    }

    private void Start()
    {
        upgradeManager = PollingStation.Get<UpgradeManager>();
        player = PollingStation.Get<PlayerController>();
    }


    private void Update()
    {
        if (player == null) return;


        Vector3 dirToPlayer = player.transform.position - transform.position;
        dirToPlayer.Normalize();

        currentMovement = dirToPlayer * movementSpeed * Time.deltaTime;
        currentMovement.y = 0;

        controller.Move(currentMovement);

        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }
}
