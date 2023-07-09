using System;
using System.Collections;
using UnityEngine;


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute {
    public ReadOnlyAttribute()
    {
    }
}