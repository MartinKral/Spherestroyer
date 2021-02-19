using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SphereCollisionSystem))]
[UpdateBefore(typeof(SphereDestructionSystem))]
public class ParticlesSpawnSystem : SystemBase
{
    private EntityQuery entityQuery;

    protected override void OnCreate()
    {
        entityQuery = GetEntityQuery(
            ComponentType.ReadOnly(typeof(DestroyedTag)),
            ComponentType.ReadOnly(typeof(Translation)),
            ComponentType.ReadOnly(typeof(MaterialId)));
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        var translations = entityQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        var materialIds = entityQuery.ToComponentDataArray<MaterialId>(Allocator.TempJob);

        Entities
            .ForEach((in ParticlesSpawner particlesSpawner) =>
        {
            for (int i = 0; i < translations.Length; i++)
            {
                for (int u = 0; u < particlesSpawner.ParticlesToSpawn; u++)
                {
                    Entity particleEntity = ecb.Instantiate(particlesSpawner.Prefab);
                    ecb.SetComponent(particleEntity, translations[i]);
                    ecb.SetComponent(particleEntity, materialIds[i]);
                    ecb.AddComponent<UpdateMaterialTag>(particleEntity);
                }
            }
        }).Run();
        ecb.Playback(EntityManager);

        materialIds.Dispose();
        translations.Dispose();

        ecb.Dispose();
    }
}
