using Unity.Entities;

[GenerateAuthoringComponent]
public struct SceneManager : IComponentData
{
    public SceneType CurrentScene;
}