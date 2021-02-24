using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Tiny.Audio;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SphereCollisionSystem))]
public class SphereDestructionSystem : SystemBase
{
    private EntityQuery shakeTarget;

    protected override void OnCreate()
    {
        shakeTarget = GetEntityQuery(ComponentType.ReadOnly(typeof(Shake)));

        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
    {
        var gameData = GetSingleton<GameData>();

        Entities
            .WithAll<DestroyedTag, SphereTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                gameData.score++;
                SetSingleton(gameData);

                EntityManager.AddComponent(shakeTarget, typeof(ActivatedTag));

                EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.Success });

                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }
}
