using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny;
using Unity.Transforms;

public class ParticleShardDestroySystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new ParticleShardDestroySystemJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);
        ecbs.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }

    [BurstCompile]
    [RequireComponentTag(typeof(ParticleShardTag))]
    private struct ParticleShardDestroySystemJob : IJobForEachWithEntity<Translation>
    {
        public EntityCommandBuffer.Concurrent ecb;

        public void Execute(Entity entity, int index, ref Translation translation)
        {
            if (-10 < translation.Value.y) return;
            ecb.DestroyEntity(index, entity);
        }
    }
}