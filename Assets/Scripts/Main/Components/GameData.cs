using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameData : IComponentData
{
    public bool IsGameFinished;
    public int score;
}