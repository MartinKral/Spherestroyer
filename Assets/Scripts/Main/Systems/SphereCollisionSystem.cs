using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class SphereCollisionSystem : SystemBase
{
    private readonly float collisionOffsetY = 0.5f;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameState>();
        RequireSingletonForUpdate<SpikeTag>();
    }

    protected override void OnUpdate()
    {
        var gameData = GetSingleton<GameState>();
        if (!gameData.IsGameActive) return;

        var spikeEntity = GetSingletonEntity<SpikeTag>();
        var MaterialIdData = GetComponentDataFromEntity<MaterialId>(true);
        var TranslationData = GetComponentDataFromEntity<Translation>(true);
        var collisionOffsetY = this.collisionOffsetY;

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Entities
            .WithAll<SphereTag>()
            .WithNone<DestroyedTag>()
            .ForEach((ref Entity entity, ref Translation translation, ref MaterialId materialId) =>
            {
                if (!MaterialIdData.HasComponent(spikeEntity)) return;
                if (!TranslationData.HasComponent(spikeEntity)) return;

                MaterialId spikeMaterial = MaterialIdData[spikeEntity];
                Translation spikeTranslation = TranslationData[spikeEntity];

                if (translation.Value.y <= spikeTranslation.Value.y + collisionOffsetY)

                {
                    {
                        if (materialId.currentMaterialId == spikeMaterial.currentMaterialId)
                        {
                            ecb.AddComponent<DestroyedTag>(entity);
                        }
                        else
                        {
                            ecb.AddComponent<DestroyedTag>(spikeEntity);
                        }
                    }
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
