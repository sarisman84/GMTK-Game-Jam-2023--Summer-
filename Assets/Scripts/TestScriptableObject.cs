using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Object", menuName = "Testing/Scriptable Objects", order = 0)]
public class TestScriptableObject : ScriptableObject
{
    public float myTestFloat;
    public string myTestString;

    [Inspect]
    public TestScriptableObject recursiveScriptableObject;
}
