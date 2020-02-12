using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameState : IComponentData
{
    public bool IsGameActive;
    public int score;
}