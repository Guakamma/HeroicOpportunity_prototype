#if UNITY_EDITOR
using System;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CustomSetupAttribute : Attribute
{
    public int Priority { get; set; }


    public CustomSetupAttribute(int priority = 0)
    {
        Priority = priority;
    }
}
#endif
