using Unity.Entities;

[GenerateAuthoringComponent]
public struct ScaleAnimation : IComponentData
{
    public bool IsIncreasing;
    public float MinScale;
    public float MaxScale;
    public float Duration;
}