using Unity.Entities;

[GenerateAuthoringComponent]
public struct SphereSpawner : IComponentData
{
    public Entity Prefab;
    public float Delay;
    public float SecondsUntilSpawn;
}