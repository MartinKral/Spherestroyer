using Unity.Entities;

public struct ParticlesSpawner : IComponentData
{
    public Entity Prefab;
    public int ParticlesToSpawn;
}