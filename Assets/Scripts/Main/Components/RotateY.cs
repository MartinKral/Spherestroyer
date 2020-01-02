using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct RotateY : IComponentData
{
    public float RadiansPerSecond;
}