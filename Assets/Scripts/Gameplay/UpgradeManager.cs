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

    private PlayerController _player;
    private UpgradeSelectorHUD upgradeSelector;

    public PlayerController player
    {
        get { return _player; }
    }


    private void Awake()
    {
        _player = GetComponent<PlayerController>();
        upgradeSelector = PollingStation.Get<UpgradeSelectorHUD>();
    }
    public void AddExperience(float someExperience)
    {
        currentExperience += someExperience;

        if (currentExperience >= experienceRequiredToLevelUp)
        {
            Debug.Log("Leveled up!");

            currentLevel++;
            currentExperience = 0;

            UpgradeSelectorHUD.SelectionDesc[] selections = new UpgradeSelectorHUD.SelectionDesc[3];
            for (int i = 0; i < selections.Length; i++)
            {
                int potentialUpgrade = Random.Range(0, upgrades.Count + weapons.Count);

                UpgradeSelectorHUD.SelectionDesc desc = new UpgradeSelectorHUD.SelectionDesc();

               

                if(potentialUpgrade >= upgrades.Count)
                {
                    desc.description = weapons[potentialUpgrade - upgrades.Count].upgradeCount > 1 ?
                        weapons[potentialUpgrade - upgrades.Count].upgradeDescription :
                         weapons[potentialUpgrade - upgrades.Count].description;
                }
                else
                {
                    desc.description = upgrades[potentialUpgrade].description;
                }

                desc.icon = potentialUpgrade >= upgrades.Count ?
                    weapons[potentialUpgrade - upgrades.Count].icon :
                    upgrades[potentialUpgrade].icon;

                desc.selectionEvent = () => { GainUpgrade(potentialUpgrade); };
               


                selections[i] = desc;
            }

            upgradeSelector.SetSelections(selections);
        }

        PollingStation.Get<VialHUD>().SetVialFill(currentExperience / experienceRequiredToLevelUp);
    }

    public void GainUpgrade(int anUpgrade)
    {
        int selectedUpgrade = anUpgrade;

        if (selectedUpgrade < upgrades.Count)
        {
            upgrades[selectedUpgrade].upgradeCount++;
            upgrades[selectedUpgrade].OnUpdate(_player);

            Debug.Log($"Gained Upgrade: {upgrades[selectedUpgrade].GetType().Name}");
        }
        else {
            BaseWeaponObject weapon = weapons[selectedUpgrade - upgrades.Count];
            weapon.upgradeCount++;
            weapon.OnUpgrade(this);

            PollingStation.Get<GizmoDrawer>().gizmoDraw += () => { weapon.OnDrawGizmo(this); };

            Debug.Log($"{(weapon.upgradeCount > 1 ? "Upgraded" : "Gained")} Weapon: {weapon.GetType().Name}");

            if (weapon.upgradeCount == 1)
                PollingStation.Get<WeaponHUD>().AddWeaponImage(weapon);
        }


    }

    public void GainWeapon(int aWeapon)
    {
        GainUpgrade(aWeapon + upgrades.Count);
    }

    private void Update()
    {
        if (!BackendManager.Get.runtimeActive) return;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            //DEBUG
            AddExperience(experienceRequiredToLevelUp);
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].upgradeCount >= 1)
                weapons[i].OnUpdate(this);
        }
    }

    public void OnLoad()
    {
        if (hasAlreadyLoaded) return;

        ResetUpgrades();

        hasAlreadyLoaded = true;
    }

    public void ResetUpgrades()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            upgrades[i].upgradeCount = 0;
        }
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].upgradeCount = 0;
        }
    }
}
