using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[AlwaysSynchronizeSystem]
public class SphereSpawnSystem : SystemBase
{
    private Random randomGenerator;

    protected override void OnStartRunning()
    {
        randomGenerator = new Random();
        randomGenerator.InitState();
        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
    {
        var gameData = GetSingleton<GameData>();
        if (gameData.currentGameState != GameState.Game) return;

        randomGenerator.NextFloat();

        Entities
            .WithoutBurst()
            .ForEach((ref SphereSpawner spawner) =>
            {
                spawner.SecondsUntilSpawn -= Time.DeltaTime;
                if (0 < spawner.SecondsUntilSpawn) return;
                if (TrySkipSpawn(spawner))
                {
                    spawner.SecondsUntilSpawn = spawner.SkipSpawnDuration;
                    return;
                }

                spawner.TimesUpgraded += randomGenerator.NextFloat() < spawner.ChanceToUpgrade ? 1 : 0;

                var nextDelay = 1 / (spawner.SpheresPerSecond + spawner.TimesUpgraded * spawner.SpawnRatePerUpgrade);
                spawner.SecondsUntilSpawn = nextDelay;

                var position = new float3(0, spawner.SpawnPositionY, 0);
                var materialId = randomGenerator.NextInt(3);

                if (!CanSpawnSphereBurst(spawner))
                {
                    SpawnRandomSphereAtPosition(position, materialId, spawner);
                }
                else
                {
                    spawner.SecondsUntilSpawn = nextDelay * 3;
                    position.y += 2;

                    for (int i = 0; i < 3; i++)
                    {
                        var burstSpherePosition = position;
                        burstSpherePosition.y += 1.2f * i;

                        var burstSphereMaterial = (materialId + 1 * i) % 3;
                        SpawnRandomSphereAtPosition(burstSpherePosition, burstSphereMaterial, spawner);
                    }
                }
            }).WithStructuralChanges().Run();
    }

    private bool TrySkipSpawn(SphereSpawner spawner)
    {
        if (spawner.TimesUpgraded < spawner.UpgradesToSkipSpawn) return false;
        float chanceToSkipSpawn = spawner.ChanceToSkipSpawnPerUpgrade * spawner.TimesUpgraded;
        chanceToSkipSpawn = math.min(spawner.MaxChanceToSkipSpawn, chanceToSkipSpawn);

        if (randomGenerator.NextFloat() < chanceToSkipSpawn)
        {
            Logger.Log("Skipping spawn");
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanSpawnSphereBurst(SphereSpawner spawner)
    {
        if (spawner.TimesUpgraded < spawner.MinUpgradesToBurst) return false;
        return randomGenerator.NextFloat() < spawner.ChanceToBurst;
    }

    private void SpawnRandomSphereAtPosition(float3 position, int materialId, in SphereSpawner spawner)
    {
        Entity icosphereEntity = EntityManager.Instantiate(spawner.Prefab);
        EntityManager.SetComponentData(icosphereEntity, new Translation { Value = position });
        EntityManager.SetComponentData(icosphereEntity, new MaterialId { currentMaterialId = materialId });
        EntityManager.SetComponentData(icosphereEntity, new Move
        {
            speedY = -spawner.InitialSpeed - spawner.SpeedPerUpgrade * spawner.TimesUpgraded
        });

        EntityManager.AddComponent<UpdateMaterialTag>(icosphereEntity);
    }
}
