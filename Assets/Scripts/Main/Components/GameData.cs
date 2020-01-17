using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameData : IComponentData
{
    public bool IsGameActive;
    public int score;
}