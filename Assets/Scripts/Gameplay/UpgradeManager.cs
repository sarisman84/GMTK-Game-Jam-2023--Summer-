using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IManager { 
    public List<BaseUpgradeObject> upgrades;

    public float experienceRequiredToLevelUp;

    private int currentLevel;
    private float currentExperience;

    private List<BaseUpgradeObject> currentUpgrades;

    public void AddExperience(float someExperience)
    {
        currentExperience += someExperience;

        if (currentExperience >= experienceRequiredToLevelUp)
        {
            currentLevel++;
            currentExperience = 0;

            GainUpgrade();
        }
    }


    public void GainUpgrade()
    {
        var randomChoice = upgrades[Random.Range(0, upgrades.Count)];
        randomChoice.ExecuteUpgrade(GetComponent<PlayerController>());
        Debug.Log("Gained Upgrade!");
    }

    public void OnLoad()
    {
       
    }
}
