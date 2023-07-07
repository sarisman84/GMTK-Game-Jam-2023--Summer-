using UnityEngine;

public class Enemy : Damagable
{
    public Transform target;

    private void Awake() {
        target = PollingStation.Get<TestManager>().transform;
    }
}
