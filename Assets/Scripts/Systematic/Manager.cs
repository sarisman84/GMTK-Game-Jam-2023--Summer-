using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    //OnLoad is called when the pollingstation loads this manager
    public abstract void OnLoad();//this method may be called before the awake function
}
