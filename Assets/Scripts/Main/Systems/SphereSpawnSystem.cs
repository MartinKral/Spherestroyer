using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[AlwaysSynchronizeSystem]
public class SphereSpawnSystem : JobComponentSystem
{
    private Random randomGenerator;

    protected override void OnStartRunning()
    {
        randomGenerator = new Random();
        randomGenerator.InitState();
        RequireSingletonForUpdate<GameState>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!GetSingleton<GameState>().IsGameActive) return default;

        randomGenerator.NextFloat();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithoutBurst()
            .ForEach((ref SphereSpawner spawner) =>
            {
                spawner.SecondsUntilSpawn -= Time.DeltaTime;
                if (0 < spawner.SecondsUntilSpawn) return;

                spawner.TimesUpgraded += randomGenerator.NextFloat() < spawner.ChanceToUpgrade ? 1 : 0;

                var nextDelay = 1 / (spawner.SpheresPerSecond + spawner.TimesUpgraded * spawner.SpawnRatePerUpgrade);
                spawner.SecondsUntilSpawn = nextDelay;

                var position = new float3(0, spawner.SpawnPositionY, 0);
                var materialId = randomGenerator.NextInt(3);

                if (!CanSpawnSphereBurst(spawner))
                {
                    SpawnRandomSphereAtPosition(position, materialId, ecb, spawner);
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
                        SpawnRandomSphereAtPosition(burstSpherePosition, burstSphereMaterial, ecb, spawner);
                    }
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;
    }

    private bool CanSpawnSphereBurst(SphereSpawner spawner)
    {
        if (spawner.TimesUpgraded < spawner.MinUpgradesToBurst) return false;
        return randomGenerator.NextFloat() < spawner.ChanceToBurst;
    }

    private void SpawnRandomSphereAtPosition(float3 position, int materialId, EntityCommandBuffer ecb, in SphereSpawner spawner)
    {
        Entity icosphereEntity = ecb.Instantiate(spawner.Prefab);
        ecb.SetComponent(icosphereEntity, new Translation { Value = position });
        ecb.SetComponent(icosphereEntity, new MaterialId { currentMaterialId = materialId });
        ecb.SetComponent(icosphereEntity, new Move
        {
            speedY = -spawner.InitialSpeed - spawner.SpeedPerUpgrade * spawner.TimesUpgraded
        });

        ecb.AddComponent<UpdateMaterialTag>(icosphereEntity);
    }
}