using Unity.Entities;

[GenerateAuthoringComponent]
public struct SceneManager : IComponentData
{
    public SceneName CurrentSceneType;
    public Entity GameplayScene;
    public Entity MenuScene;
}