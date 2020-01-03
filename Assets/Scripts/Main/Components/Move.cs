using Unity.Entities;

[GenerateAuthoringComponent]
public struct Move : IComponentData
{
    public float speedX;
    public float speedY;
    public float speedZ;
}