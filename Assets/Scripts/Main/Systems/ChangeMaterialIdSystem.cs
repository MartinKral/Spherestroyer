using Unity.Entities;
using Unity.Jobs;

public class ChangeMaterialIdSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new ChangeMaterialIdSystemJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);
        ecbs.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }

    [RequireComponentTag(typeof(OnInputTag))]
    private struct ChangeMaterialIdSystemJob : IJobForEachWithEntity<MaterialId>
    {
        public EntityCommandBuffer.Concurrent ecb;

        public void Execute(Entity entity, int index, ref MaterialId materialId)
        {
            materialId.currentMaterialId = ++materialId.currentMaterialId % 3;
            ecb.AddComponent<UpdateMaterialTag>(index, entity);
        }
    }
}