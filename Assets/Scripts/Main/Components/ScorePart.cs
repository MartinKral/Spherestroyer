using Unity.Entities;

[GenerateAuthoringComponent]
public struct ScorePart : IComponentData
{
    public int Divisor;
}