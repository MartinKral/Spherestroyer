using Unity.Entities;

[GenerateAuthoringComponent]
public struct IcosphereSpawner : IComponentData
{
    public Entity prefab;
}