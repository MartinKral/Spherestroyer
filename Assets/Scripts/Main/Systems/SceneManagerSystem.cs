using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Tiny.Rendering;
using Unity.Collections;
using System;

[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class SceneManagerSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
          .WithoutBurst()
          .WithStructuralChanges()
          .ForEach((Entity entity, ref SceneManager sceneManager, in ChangeScene changeScene) =>
          {
              DestroySceneEntities();
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

    private void DestroySceneEntities()
    {
        EntityQuery eq = GetEntityQuery(ComponentType.ReadOnly<ScenePrefabTag>());
        var scenePrefabs = eq.ToEntityArray(Allocator.TempJob);

        for (int i = 0; i < scenePrefabs.Length; i++)
        {
            DestroyChildRecursively(scenePrefabs[i]);
            EntityManager.DestroyEntity(scenePrefabs[i]);
        }

        scenePrefabs.Dispose();
    }

    private void DestroyChildRecursively(Entity entityParent)
    {
        if (!EntityManager.HasComponent<Child>(entityParent))
        {
            EntityManager.DestroyEntity(entityParent);
            return;
        }

        var buffer = EntityManager.GetBuffer<Child>(entityParent);
        var childArray = buffer.ToNativeArray(Allocator.Temp);

        for (int i = 0; i < childArray.Length; i++)
        {
            DestroyChildRecursively(childArray[i].Value);
        }
        childArray.Dispose();
    }
}