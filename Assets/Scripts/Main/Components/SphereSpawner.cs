using Unity.Entities;

public struct SphereSpawner : IComponentData
{
    public Entity Prefab;
    public float SpawnPositionY;
    public float InitialSpeed;
    public float SpheresPerSecond;
    public float ChanceToUpgrade;
    public float SpawnRatePerUpgrade;
    public float SpeedPerUpgrade;
    public float MinUpgradesToBurst;
    public float ChanceToBurst;
    public float ChanceToSkipSpawnPerUpgrade;
    public float MaxChanceToSkipSpawn;
    public int UpgradesToSkipSpawn;
    public float SkipSpawnDuration;

    public int TimesUpgraded;
    public float SecondsUntilSpawn;
}