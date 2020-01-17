using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SphereSpawnSystem : JobComponentSystem
{
    private Random randomGenerator;
    private BeginSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnStartRunning()
    {
        randomGenerator = new Random();
        randomGenerator.InitState();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        randomGenerator.NextFloat();

        var jobHandle = new SpawningJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent(),
            positionY = 5,
            randomGenerator = randomGenerator,
            deltaTime = Time.DeltaTime
        }.Schedule(this, inputDeps);
        ecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }

    [BurstCompile]
    private struct SpawningJob : IJobForEachWithEntity<SphereSpawner>
    {
        public EntityCommandBuffer.Concurrent ecb;

        public Random randomGenerator;
        public int positionY;
        public float deltaTime;

        public void Execute(Entity entity, int index, ref SphereSpawner spawner)
        {
            spawner.SecondsUntilSpawn -= deltaTime;
            if (0 < spawner.SecondsUntilSpawn) return;

            spawner.TimesUpgraded += randomGenerator.NextFloat() < spawner.ChanceToUpgrade ? 1 : 0;

            var nextDelay = 1 / (spawner.SpheresPerSecond + spawner.TimesUpgraded * spawner.SpawnRatePerUpgrade);
            spawner.SecondsUntilSpawn = nextDelay;

            var position = new float3(0, positionY, 0);
            var materialId = randomGenerator.NextInt(3);

            if (!TrySpawnBurst(spawner))
            {
                SpawnRandomSphereAtPosition(position, materialId, index, spawner);
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
                    SpawnRandomSphereAtPosition(burstSpherePosition, burstSphereMaterial, index, spawner);
                }
            }
        }

        private bool TrySpawnBurst(SphereSpawner spawner)
        {
            if (spawner.TimesUpgraded < 3) return false;
            return randomGenerator.NextFloat() < spawner.ChanceToBurst;
        }

        private void SpawnRandomSphereAtPosition(float3 position, int materialId, int index, in SphereSpawner spawner)
        {
            Entity icosphereEntity = ecb.Instantiate(index, spawner.Prefab);
            ecb.SetComponent(index, icosphereEntity, new Translation { Value = position });
            ecb.SetComponent(index, icosphereEntity, new MaterialId { currentMaterialId = materialId });
            ecb.SetComponent(index, icosphereEntity, new Move { speedY = -1 - 0.1f * spawner.TimesUpgraded });
            ecb.AddComponent<UpdateMaterialTag>(index, icosphereEntity);
        }
    }
}