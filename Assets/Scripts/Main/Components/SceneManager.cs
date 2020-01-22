using Unity.Entities;

[GenerateAuthoringComponent]
public struct SceneManager : IComponentData
{
    public SceneType CurrentSceneType;
    public Entity GameplayScene;
    public Entity MenuScene;
}