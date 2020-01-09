using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Rotate : IComponentData
{
    public float3 radiansPerSecond;
}