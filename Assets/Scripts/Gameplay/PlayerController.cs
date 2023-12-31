using UnityEngine;

public class PlayerController : Damagable, IManager {

    [Header("Debug Info")]
    public bool showDebug = false;

    [Header("Gameplay")]
    public float baseMovementSpeed;
    public float movementSpeed { get; set; }

    private UpgradeManager upgradeManager;
    private CharacterController controller;
    private Vector3 currentMovement;
    private MeshFilter meshFilter;
    private Camera mainCamera;

    public float invurnabilityDurationInSeconds = 0.5f;
    private float currentInvurnabilityDuration = 0.0f;
    private bool hasBeenHit = false;

    public bool isActive
    {
        private set;
        get;
    }


    //public List<BaseWeaponUpgrade> activeWeapons;

    public void OnLoad()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        movementSpeed = baseMovementSpeed;
        upgradeManager = GetComponent<UpgradeManager>();
        controller = GetComponent<CharacterController>() != null ? GetComponent<CharacterController>() : gameObject.AddComponent<CharacterController>();

        mainCamera = Camera.main;
    }


    public override void Start()
    {
        healthUpdate += PollingStation.Get<HealthBar>().GetHealth;
        base.Start();
        PollingStation.Get<TeamsManager>().AddToTeam<PlayerController>(gameObject);
    }

    private void Movement()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();

        currentMovement = (mainCamera.transform.right * input.x + mainCamera.transform.forward * input.y) * movementSpeed * Time.deltaTime;
        currentMovement.y = 0;
        controller.Move(currentMovement);

        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    public override void Hit(float damage, MonoBehaviour attacker)
    {
        if (!hasBeenHit)
        {
            base.Hit(damage, attacker);
            hasBeenHit = true;
        }
    }

    public override void OnDeath(MonoBehaviour attacker)
    {
        PollingStation.Get<GameplayManager>().GameOver();


    }

    private void UpdateInvurnabilityTimer()
    {
        if (!hasBeenHit) return;



        currentInvurnabilityDuration += Time.deltaTime;

        if (currentInvurnabilityDuration > invurnabilityDurationInSeconds)
        {
            currentInvurnabilityDuration = 0;
            hasBeenHit = false;
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PollingStation.Get<UIManager>().PauseGame();
        }


        if (!BackendManager.Get.runtimeActive) return;
        UpdateInvurnabilityTimer();
        Movement();
        //Attack();
    }


    private void OnDrawGizmos()
    {
        if (!showDebug || !Application.isPlaying || !hasBeenHit) return;




        Gizmos.color = Color.red * new Color(1, 1, 1, Mathf.Abs(invurnabilityDurationInSeconds - currentInvurnabilityDuration));
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawMesh(meshFilter.sharedMesh, Vector3.zero);
    }


    public void SetActive(bool aNewState)
    {
        BackendManager.Get.runtimeActive = aNewState;
        meshFilter.gameObject.SetActive(aNewState);
        if (!aNewState)
        {
            upgradeManager.ResetUpgrades();
            maxHealth = baseMaxHealth;
        }
           
        else
        {
            upgradeManager.GainWeapon(0);
            FullyHeal();
        }


    }
}
