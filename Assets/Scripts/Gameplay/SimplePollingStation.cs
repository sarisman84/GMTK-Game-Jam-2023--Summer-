using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePollingStation
{
    private static SimplePollingStation instance;


    public static SimplePollingStation Get
    {
        get
        {
            if (instance == null)
                instance = new SimplePollingStation();
            return instance;
        }
    }

    public PlayerController player;
    public UpgradeManager upgradeManager;
}
