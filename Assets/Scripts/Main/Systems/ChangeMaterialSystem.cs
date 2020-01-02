using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Rendering;
using Unity.Transforms;

public class ChangeMaterialSystem : JobComponentSystem
{
    private BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<SpikeMaterial>();
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        Logger.Log("CHANGE MATERIAL SYSTEM START");
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        // skip
        return inputDependencies;

        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        Logger.Log("CHANGE MATERIAL SYSTEM");
        var materialReferencesEntity = GetSingletonEntity<SpikeMaterial>();
        var materialReferences = EntityManager.GetComponentData<SpikeMaterial>(materialReferencesEntity);

        var job = new ChangeMaterialSystemJob { materialReferences = materialReferences, commandBuffer = commandBuffer };

        return job.Schedule(this, inputDependencies);
    }

    //[BurstCompile]
    private struct ChangeMaterialSystemJob : IJobForEach<MeshRenderer>
    {
        public SpikeMaterial materialReferences;
        public EntityCommandBuffer.Concurrent commandBuffer;

        // public Entity
        public void Execute(ref MeshRenderer meshRenderer)
        {
            Logger.Log($"Before: {meshRenderer.material}");
            meshRenderer.material = materialReferences.material;

            //commandBuffer.SetComponent<MeshRenderer>(0, )
            Logger.Log($"After: {meshRenderer.material}");
        }
    }
}