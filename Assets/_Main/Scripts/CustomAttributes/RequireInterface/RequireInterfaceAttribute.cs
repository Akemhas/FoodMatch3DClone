using UnityEngine;
using System;

public class RequireInterfaceAttribute : PropertyAttribute
{
    public Type requiredType { get; private set; }

    public RequireInterfaceAttribute(Type type)
    {
        this.requiredType = type;
    }
}