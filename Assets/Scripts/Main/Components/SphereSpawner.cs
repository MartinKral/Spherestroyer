using Unity.Entities;

[GenerateAuthoringComponent]
public struct SphereSpawner : IComponentData
{
    public Entity Prefab;
    public float SpheresPerSecond;
    public float ChanceToUpgrade;
    public float SpawnRatePerUpgrade;
    public float ChanceToBurst;

    public int TimesUpgraded;
    public float SecondsUntilSpawn;
}