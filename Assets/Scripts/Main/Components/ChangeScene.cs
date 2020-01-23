using Unity.Entities;

[GenerateAuthoringComponent]
public struct ChangeScene : IComponentData
{
    public SceneName Value;
}