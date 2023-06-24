using System;
using System.Collections;
using UnityEngine;


[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public class InspectAttribute : PropertyAttribute
{
    public string displayName;
    public bool displayBackground;

    public InspectAttribute(string aDisplayName = "", bool aDisplayBackground = true)
    {
        displayName = aDisplayName;
        displayBackground = aDisplayBackground;
    }
}
