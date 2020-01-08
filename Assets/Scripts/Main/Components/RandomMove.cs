using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct RandomMove : IComponentData
{
    public float2 MinMaxX;
    public float2 MinMaxY;
    public float2 MinMaxZ;
}