using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class SphereSpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject IcospherePrefab;
    public GameObject spike;
    public float SpawnPositionY;
    public float InitialSpeed;
    public float SpheresPerSecond;
    public float ChanceToUpgrade;
    public float SpeedPerUpgrade;
    public float SpawnRatePerUpgrade;
    public float MinUpgradesToBurst;
    public float ChanceToBurst;
    public float ChanceToSkipSpherePerUpgrade;
    public float MaxChanceToSkipSpawn;
    public int UpgradesToSkipSphere;
    public float SkipSpawnDuration;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Entity prefabEntity = conversionSystem.GetPrimaryEntity(IcospherePrefab);

        dstManager.AddComponentData(prefabEntity, new SpikeReference { Entity = conversionSystem.GetPrimaryEntity(spike) });
        dstManager.AddComponentData(entity, new SphereSpawner()
        {
            Prefab = prefabEntity,
            SpawnPositionY = SpawnPositionY,
            InitialSpeed = InitialSpeed,
            SpheresPerSecond = SpheresPerSecond,
            ChanceToUpgrade = ChanceToUpgrade,
            SpeedPerUpgrade = SpeedPerUpgrade,
            SpawnRatePerUpgrade = SpawnRatePerUpgrade,
            MinUpgradesToBurst = MinUpgradesToBurst,
            ChanceToBurst = ChanceToBurst,
            ChanceToSkipSpawnPerUpgrade = ChanceToSkipSpherePerUpgrade,
            MaxChanceToSkipSpawn = MaxChanceToSkipSpawn,
            UpgradesToSkipSpawn = UpgradesToSkipSphere,
            SkipSpawnDuration = SkipSpawnDuration,
            SecondsUntilSpawn = 0,
            TimesUpgraded = 0
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(IcospherePrefab);
    }
}