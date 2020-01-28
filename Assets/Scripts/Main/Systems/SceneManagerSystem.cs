using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class SceneManagerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);

        Entities
          .WithoutBurst()
          .WithStructuralChanges()
          .ForEach((Entity entity, ref SceneManager sceneManager, in ChangeScene changeScene) =>
          {
              DestroyAllSceneEntities(ecb);
              sceneManager.CurrentSceneType = changeScene.Value;

              if (sceneManager.CurrentSceneType == SceneName.Gameplay)
              {
                  EntityManager.Instantiate(sceneManager.GameplayScene);
              }
              else if (sceneManager.CurrentSceneType == SceneName.Menu)
              {
                  EntityManager.Instantiate(sceneManager.MenuScene);
              }

              ecb.RemoveComponent<ChangeScene>(entity);
          }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }

    private void DestroyAllSceneEntities(EntityCommandBuffer ecb)
    {
        EntityQuery eq = GetEntityQuery(ComponentType.ReadOnly<ScenePrefabTag>());

        var scenePrefabs = eq.ToEntityArray(Allocator.TempJob);

        for (int i = 0; i < scenePrefabs.Length; i++)
        {
            DestroyAllLinkedEntities(scenePrefabs[i], ecb);
        }

        scenePrefabs.Dispose();
    }

    private void DestroyAllLinkedEntities(Entity sceneEntity, EntityCommandBuffer ecb)
    {
        if (EntityManager.HasComponent<LinkedEntityGroup>(sceneEntity))
        {
            var buffer = EntityManager.GetBuffer<LinkedEntityGroup>(sceneEntity);
            var linkedEntities = buffer.ToNativeArray(Allocator.TempJob);

            for (int i = 0; i < linkedEntities.Length; i++)
            {
                ecb.DestroyEntity(linkedEntities[i].Value);
            }

            linkedEntities.Dispose();
        }
    }
}