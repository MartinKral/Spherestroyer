using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class SphereCollisionSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new SphereCollisionSystemJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent(),
            MaterialIdData = GetComponentDataFromEntity<MaterialId>(true),
            TranslationData = GetComponentDataFromEntity<Translation>(true)
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    [ExcludeComponent(typeof(DestroyedIcosphereTag))]
    private struct SphereCollisionSystemJob : IJobForEachWithEntity<Translation, MaterialId, SpikeReference>
    {
        public EntityCommandBuffer.Concurrent ecb;
        [ReadOnly] public ComponentDataFromEntity<MaterialId> MaterialIdData;
        [ReadOnly] public ComponentDataFromEntity<Translation> TranslationData;

        public void Execute(
            Entity entity,
            int index,
            [ReadOnly] ref Translation translation,
            [ReadOnly] ref MaterialId materialId,
            [ReadOnly] ref SpikeReference spikeReference)
        {
            if (!MaterialIdData.Exists(spikeReference.entity)) throw new System.Exception($"Spike should have {typeof(MaterialId)}");
            if (!TranslationData.Exists(spikeReference.entity)) throw new System.Exception($"Spike should have {typeof(Translation)}");

            MaterialId spikeMaterial = MaterialIdData[spikeReference.entity];
            Translation spikeTranslation = TranslationData[spikeReference.entity];

            if (translation.Value.y <= spikeTranslation.Value.y)
            {
                if (materialId.currentMaterialId == spikeMaterial.currentMaterialId)
                {
                    ecb.AddComponent<DestroyedIcosphereTag>(index, entity);
                }
                else
                {
                }
            }
        }
    }
}