using Unity.Entities;

[GenerateAuthoringComponent]
public struct SphereSpawner : IComponentData
{
    public Entity prefab;
}