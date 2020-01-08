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
        var jobHandle = new SpawningJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent(),
            positionY = 5,
            materialType = randomGenerator.NextInt(3),
            deltaTime = Time.DeltaTime
        }.Schedule(this, inputDeps);
        ecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }

    [BurstCompile]
    private struct SpawningJob : IJobForEachWithEntity<SphereSpawner>
    {
        public EntityCommandBuffer.Concurrent ecb;

        public int positionY;
        public int materialType;
        public float deltaTime;

        public void Execute(Entity entity, int index, ref SphereSpawner sphereSpawner)
        {
            sphereSpawner.SecondsUntilSpawn -= deltaTime;
            if (0 < sphereSpawner.SecondsUntilSpawn) return;

            sphereSpawner.SecondsUntilSpawn = sphereSpawner.Delay;

            Entity icosphereEntity = ecb.Instantiate(index, sphereSpawner.Prefab);
            var position = new float3(0, positionY, 0);
            ecb.SetComponent(index, icosphereEntity, new Translation { Value = position });
            ecb.SetComponent(index, icosphereEntity, new MaterialId { currentMaterialId = materialType });
            ecb.AddComponent<UpdateMaterialTag>(index, icosphereEntity);
        }
    }
}