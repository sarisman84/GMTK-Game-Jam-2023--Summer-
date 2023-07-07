using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IManager {

    public List<BaseUpgradeObject> upgrades;
    public List<BaseWeaponObject> weapons;


    public float experienceRequiredToLevelUp;

    private int currentLevel;
    private float currentExperience;

    private bool hasAlreadyLoaded = false;

    [HideInInspector] public List<int> currentUpgrades;

    private PlayerController player;


    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    public void AddExperience(float someExperience)
    {
        currentExperience += someExperience;

        if (currentExperience >= experienceRequiredToLevelUp)
        {
            Debug.Log("Leveled up!");

            currentLevel++;
            currentExperience = 0;

            GainUpgrade();
        }
    }

    public void GainUpgrade(int anUpgrade)
    {
        int selectedUpgrade = anUpgrade;


        if (currentUpgrades.Count == 0)
            OnLoad();

        currentUpgrades[selectedUpgrade]++;

        if (selectedUpgrade < upgrades.Count)
        {
            upgrades[selectedUpgrade].upgradeCount = currentUpgrades[selectedUpgrade];
            upgrades[selectedUpgrade].OnUpdate(player);

            Debug.Log($"Gained Upgrade: {upgrades[selectedUpgrade].GetType().Name}");
        }
        else
        {
            weapons[selectedUpgrade - upgrades.Count].upgradeCount = currentUpgrades[selectedUpgrade];

            Debug.Log($"{(currentUpgrades[selectedUpgrade] > 1 ? "Upgraded" : "Gained")} Weapon: {weapons[selectedUpgrade - upgrades.Count].GetType().Name}");
        }


    }

    public void GainWeapon(int aWeapon)
    {
        GainUpgrade(aWeapon + upgrades.Count);
    }

    public void GainUpgrade()
    {
        GainUpgrade(Random.Range(0, upgrades.Count + weapons.Count));
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //DEBUG
            AddExperience(experienceRequiredToLevelUp);
        }

        for (int i = upgrades.Count; i < currentUpgrades.Count; i++)
        {
            if (currentUpgrades[i] >= 1)
                weapons[i - upgrades.Count].OnUpdate(player);
        }
    }

    public void OnLoad()
    {
        if (hasAlreadyLoaded) return;

        for (int i = 0; i < upgrades.Count; i++)
        {
            currentUpgrades.Add(0);
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            currentUpgrades.Add(0);
        }

        hasAlreadyLoaded = true;
    }
}
