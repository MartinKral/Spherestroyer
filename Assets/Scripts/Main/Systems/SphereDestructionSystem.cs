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
    private EntityQuery uiUpdateTarget;

    protected override void OnCreate()
    {
        shakeTarget = GetEntityQuery(ComponentType.ReadOnly(typeof(Shake)));
        uiUpdateTarget = GetEntityQuery(ComponentType.ReadOnly(typeof(ScoreTag)));

        RequireSingletonForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var gameData = GetSingleton<GameState>();

        Logger.Log("In Sphere Destruction System");
        Entities
            .WithAll<DestroyedTag, SphereTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                Logger.Log("Increasing score");
                gameData.score++;
                SetSingleton(gameData);

                EntityManager.AddComponent(shakeTarget, typeof(ActivatedTag));
                EntityManager.AddComponent(uiUpdateTarget, typeof(ActivatedTag));

                EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.Success });

                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }
}
