using Unity.Entities;

[GenerateAuthoringComponent]
public struct SphereSpawner : IComponentData
{
    public Entity prefab;
    public float delay;
    public float secondsUntilSpawn;
}