using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Shake : IComponentData
{
    public float3 Intensity;
    public float Duration;
    public float DurationLeft;
    public float3 DefaultTranslation;
}