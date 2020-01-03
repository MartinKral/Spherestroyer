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
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new SpawningJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent(),
            icosphereSpawner = icosphereSpawner,
            positionY = randomGenerator.NextFloat() * 2,
            materialType = randomGenerator.NextInt(3)
        }.Schedule(this, inputDeps);

        // This jobs needs to be finished before hitting the buffer system
        ecbs.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }

    private struct SpawningJob : IJobForEachWithEntity<OnClickTag>
    {
        public EntityCommandBuffer.Concurrent ecb;
        public IcosphereSpawner icosphereSpawner;
        public float positionY;
        public int materialType;

        public void Execute(Entity entity, int index, [ReadOnly] ref OnClickTag onClickTag)
        {
            Entity icosphereEntity = ecb.Instantiate(index, icosphereSpawner.prefab);
            var position = new float3(0, positionY, 0);
            ecb.SetComponent(index, icosphereEntity, new Translation { Value = position });
            ecb.SetComponent(index, icosphereEntity, new MaterialId { currentMaterialId = materialType });
            ecb.AddComponent<UpdateMaterialTag>(index, icosphereEntity);
        }
    }
}