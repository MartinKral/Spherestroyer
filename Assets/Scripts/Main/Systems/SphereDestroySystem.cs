using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class SphereDestroySystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        base.OnCreate();
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new DestroySphereSystemJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [RequireComponentTag(typeof(DestroyedIcosphereTag))]
    private struct DestroySphereSystemJob : IJobForEachWithEntity<Translation>
    {
        public EntityCommandBuffer.Concurrent ecb;

        public void Execute(Entity entity, int index, ref Translation translation)
        {
            ecb.DestroyEntity(index, entity);
        }
    }
}