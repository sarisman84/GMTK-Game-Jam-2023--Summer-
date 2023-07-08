using System.Collections;
using UnityEngine;


public class GameplayManager {

    static GameplayManager instance;

    public static GameplayManager Get
    {
        get
        {
            if (instance == null)
                instance = new GameplayManager();

            return instance;
        }


    }

    public bool runtimeActive = true;
}
