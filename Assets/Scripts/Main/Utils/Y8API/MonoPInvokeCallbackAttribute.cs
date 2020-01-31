using System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MonoPInvokeCallbackAttribute : Attribute
{
    public Type DelegateType { get; set; }

    public MonoPInvokeCallbackAttribute(Type type)
    {
        DelegateType = type;
    }
}