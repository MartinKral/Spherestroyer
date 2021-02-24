using Unity.Entities;

public enum GameState
{
    Menu,
    PreGame,
    Game
}

[GenerateAuthoringComponent]
public struct GameData : IComponentData
{
    public GameState currentGameState;
    public int score;
}
