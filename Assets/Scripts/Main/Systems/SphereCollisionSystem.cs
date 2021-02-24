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
        RequireSingletonForUpdate<GameData>();
        RequireSingletonForUpdate<SpikeTag>();
    }

    protected override void OnUpdate()
    {
        var gameData = GetSingleton<GameData>();
        if (gameData.currentGameState != GameState.Game) return;

        var spikeEntity = GetSingletonEntity<SpikeTag>();
        var MaterialIdData = GetComponentDataFromEntity<MaterialId>(true);
        var TranslationData = GetComponentDataFromEntity<Translation>(true);

        MaterialId spikeMaterial = MaterialIdData[spikeEntity];
        Translation spikeTranslation = TranslationData[spikeEntity];

        var collisionOffsetY = this.collisionOffsetY;

        Entities
            .WithAll<SphereTag>()
            .WithNone<DestroyedTag>()
            .ForEach((ref Entity entity, ref Translation translation, ref MaterialId materialId) =>
            {
                if (translation.Value.y <= spikeTranslation.Value.y + collisionOffsetY)

                {
                    {
                        if (materialId.currentMaterialId == spikeMaterial.currentMaterialId)
                        {
                            EntityManager.AddComponent<DestroyedTag>(entity);
                        }
                        else
                        {
                            EntityManager.AddComponent<DestroyedTag>(spikeEntity);
                        }
                    }
                }
            }).WithStructuralChanges().Run();
    }
}
