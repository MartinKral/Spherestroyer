using System;
using Unity.Entities;
using Unity.Scenes;

[UpdateInGroup(typeof(SceneSystemGroup))]
[UpdateBefore(typeof(SceneSystem))]
public class SceneManagerSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
    {
        SceneName sceneToLoad = SceneName.None;

        Entities.ForEach((Entity e, in ChangeScene changeScene) =>
        {
            sceneToLoad = changeScene.Value;
            EntityManager.DestroyEntity(e);
        }).WithStructuralChanges().Run();

        var gameState = GetSingleton<GameData>();
        if (sceneToLoad == SceneName.Menu)
        {
            gameState.currentGameState = GameState.Menu;
            TryUnloadScene<GameSceneTag>();
            LoadScene<MainMenuSceneTag>();
        }
        else if (sceneToLoad == SceneName.Gameplay)
        {
            gameState.currentGameState = GameState.PreGame;
            TryUnloadScene<MainMenuSceneTag>();
            LoadScene<GameSceneTag>();
        }

        SetSingleton(gameState);
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
