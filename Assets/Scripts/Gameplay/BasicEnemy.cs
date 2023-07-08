using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Damagable
{
    public float movementSpeed;
    public float attackSpeed;
    public float amountOfDroppedExperience = 0;
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
        if (!GameplayManager.Get.runtimeActive) return;

        Vector3 dirToPlayer = player.transform.position - transform.position;
        dirToPlayer.Normalize();

        currentMovement = dirToPlayer * movementSpeed * Time.deltaTime;
        currentMovement.y = 0;

        controller.Move(currentMovement);

        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    public override void OnDeath(MonoBehaviour attacker)
    {
        Destroy(gameObject);
        PollingStation.Get<UpgradeManager>().AddExperience(amountOfDroppedExperience);
    }
}
