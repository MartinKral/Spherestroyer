using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SpawningJobSystem : JobComponentSystem
{
    private IcosphereSpawner icosphereSpawner;
    private EntityQuery entityQuery;
    private Random randomGenerator;

    private BeginSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<IcosphereSpawner>();
        ecbs = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        Entity planeEntity = GetSingletonEntity<IcosphereSpawner>();
        icosphereSpawner = EntityManager.GetComponentData<IcosphereSpawner>(planeEntity);
        randomGenerator = new Random((uint)(Time.DeltaTime * 1000));
        entityQuery = GetEntityQuery(ComponentType.ReadOnly(typeof(OnClickTag)));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new SpawningJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent(),
            icosphereSpawner = icosphereSpawner,
            positionY = randomGenerator.NextFloat() * 5,
            materialType = randomGenerator.NextInt(3)
        }.Schedule(entityQuery.CalculateEntityCount(), 1, inputDeps);
        ecbs.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }

    private struct SpawningJob : IJobParallelFor
    {
        public EntityCommandBuffer.Concurrent ecb;
        public IcosphereSpawner icosphereSpawner;
        public float positionY;
        public int materialType;

        public void Execute(int index)
        {
            Entity icosphereEntity = ecb.Instantiate(index, icosphereSpawner.prefab);
            var position = new float3(0, positionY, 0);
            ecb.SetComponent(index, icosphereEntity, new Translation { Value = position });
            ecb.SetComponent(index, icosphereEntity, new MaterialId { currentMaterialId = materialType });
            ecb.AddComponent<UpdateMaterialTag>(index, icosphereEntity);
        }
    }
}