using UnityEngine;

public interface IManager
{
    //OnLoad is called when the pollingstation loads this manager
    public abstract void OnLoad();//this method may be called before the awake function
}
