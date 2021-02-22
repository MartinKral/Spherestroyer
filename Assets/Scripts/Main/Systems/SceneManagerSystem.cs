using System;
using Unity.Entities;
using Unity.Scenes;

[UpdateInGroup(typeof(SceneSystemGroup))]
[UpdateBefore(typeof(SceneSystem))]
public class SceneManagerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        SceneName sceneToLoad = SceneName.None;

        Entities.ForEach((Entity e, in ChangeScene changeScene) =>
        {
            sceneToLoad = changeScene.Value;
            EntityManager.DestroyEntity(e);
        }).WithStructuralChanges().Run();

        if (sceneToLoad == SceneName.Menu)
        {
            TryUnloadScene<GameSceneTag>();
            LoadScene<MainMenuSceneTag>();
        }
        else if (sceneToLoad == SceneName.Gameplay)
        {
            TryUnloadScene<MainMenuSceneTag>();
            LoadScene<GameSceneTag>();
        }
    }

    private void TryUnloadScene<T>() where T : struct
    {
        var sceneSystem = World.GetExistingSystem<SceneSystem>();
        var sceneEntity = GetSingletonEntity<T>();

        if (sceneSystem.IsSceneLoaded(sceneEntity))
        {
            sceneSystem.UnloadScene(sceneEntity);
        }
    }

    private void LoadScene<T>() where T : struct
    {
        var sceneSystem = World.GetExistingSystem<SceneSystem>();
        var sceneEntity = GetSingletonEntity<T>();
        var scene = EntityManager.GetComponentData<SceneReference>(sceneEntity);
        sceneSystem.LoadSceneAsync(scene.SceneGUID, new SceneSystem.LoadParameters { AutoLoad = true });
    }
}
