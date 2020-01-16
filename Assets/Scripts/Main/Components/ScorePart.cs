using Unity.Entities;

[GenerateAuthoringComponent]
public struct ScorePart : IComponentData
{
    public int TargetScore;
    public int Divisor;
}