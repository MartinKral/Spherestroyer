using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class SphereCollisionSystem : JobComponentSystem
{
    private DestructionBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<DestructionBufferSystem>();
        RequireSingletonForUpdate<GameData>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var gameData = GetSingleton<GameData>();
        if (!gameData.IsGameActive) return inputDeps;

        var jobHandle = new SphereCollisionSystemJob()
        {
            ecb = ecbs.CreateCommandBuffer().ToConcurrent(),
            MaterialIdData = GetComponentDataFromEntity<MaterialId>(true),
            TranslationData = GetComponentDataFromEntity<Translation>(true),
            DisabledData = GetComponentDataFromEntity<Disabled>(true)
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    [RequireComponentTag(typeof(SphereTag))]
    [ExcludeComponent(typeof(DestroyedTag))]
    private struct SphereCollisionSystemJob : IJobForEachWithEntity<Translation, MaterialId, SpikeReference>
    {
        public EntityCommandBuffer.Concurrent ecb;
        [ReadOnly] public ComponentDataFromEntity<MaterialId> MaterialIdData;
        [ReadOnly] public ComponentDataFromEntity<Translation> TranslationData;
        [ReadOnly] public ComponentDataFromEntity<Disabled> DisabledData;

        public void Execute(
            Entity entity,
            int index,
            [ReadOnly] ref Translation translation,
            [ReadOnly] ref MaterialId materialId,
            [ReadOnly] ref SpikeReference spikeReference)
        {
            if (DisabledData.Exists(spikeReference.Entity)) return; // The entity is disabled
            if (!MaterialIdData.Exists(spikeReference.Entity)) return;
            if (!TranslationData.Exists(spikeReference.Entity)) return;

            MaterialId spikeMaterial = MaterialIdData[spikeReference.Entity];
            Translation spikeTranslation = TranslationData[spikeReference.Entity];

            if (translation.Value.y <= spikeTranslation.Value.y + 0.5f)
            {
                if (materialId.currentMaterialId == spikeMaterial.currentMaterialId)
                {
                    ecb.AddComponent<DestroyedTag>(index, entity);
                }
                else
                {
                    ecb.AddComponent<DestroyedTag>(index, spikeReference.Entity);
                }
            }
        }
    }
}