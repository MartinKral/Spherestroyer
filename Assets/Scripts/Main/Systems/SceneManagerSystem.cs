using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Tiny.Rendering;
using Unity.Collections;
using System;

[AlwaysSynchronizeSystem]
public class SceneManagerSystem : JobComponentSystem
{
    private EntityQuery eq;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<Camera>();

        EntityQueryDesc eqDesc = new EntityQueryDesc()
        {
            None = new ComponentType[] {
                typeof(DontDestroyOnLoadTag)
            }
        };

        eq = GetEntityQuery(eqDesc);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var cameraEntity = GetSingletonEntity<Camera>();
        var cameraTranslation = EntityManager.GetComponentData<Translation>(cameraEntity);

        Entities
          .WithoutBurst()
          .ForEach((Entity entity, ref SceneManager sceneManager, in ChangeScene changeScene) =>
          {
              sceneManager.CurrentScene = changeScene.Value;

              if (sceneManager.CurrentScene == SceneType.Gameplay)
              {
                  cameraTranslation.Value.x = 0;
              }
              else if (sceneManager.CurrentScene == SceneType.Menu)
              {
                  cameraTranslation.Value.x = 10;
              }

              ecb.SetComponent(cameraEntity, cameraTranslation);
              ecb.RemoveComponent<ChangeScene>(entity);
              //ecb.DestroyEntity(eq);
          }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}