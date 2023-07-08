using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { Player, Enemy }
public class TeamsManager : MonoBehaviour, IManager {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnGlobalStart()
    {
        GameObject manager = GameObject.Find("Systems") ?? new GameObject("Systems");
        TeamsManager isValid = manager.GetComponent<TeamsManager>() ?? manager.AddComponent<TeamsManager>();

        Debug.Log($"{isValid.name} loaded!");

        DontDestroyOnLoad(manager.gameObject);
    }
    public void OnLoad()
    {

    }



    List<int>[] registeredEntities;


    public void AddToTeam<T>(GameObject aGameObject) where T : MonoBehaviour
    {
        if (typeof(T) == typeof(PlayerController))
            AddToTeam(Team.Player, aGameObject);
        else
            AddToTeam(Team.Enemy, aGameObject);
    }

    public void AddToTeam(Team aTeamType, GameObject aGameObject)
    {
        if (registeredEntities == null)
        {
            registeredEntities = new List<int>[2];
            for (int i = 0; i < registeredEntities.Length; i++)
            {
                registeredEntities[i] = new List<int>();
            }
        }


        registeredEntities[(int)aTeamType].Add(aGameObject.GetInstanceID());
    }



    public bool AreInTheSameTeam(GameObject aLhs, GameObject aRhs)
    {
        int lhs_id = aLhs.GetInstanceID();
        int rhs_id = aRhs.GetInstanceID();


        bool lhsFound = false;
        bool rhsFound = false;

        for (int i = 0; i < registeredEntities.Length; i++)
        {
            lhsFound = registeredEntities[i].Find((x) => { return x == lhs_id; }) != 0;
            rhsFound = registeredEntities[i].Find((x) => { return x == rhs_id; }) != 0;

            if (lhsFound && rhsFound) return true;
        }

        return false;
    }




}
