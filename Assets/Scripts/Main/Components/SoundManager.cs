using Unity.Entities;

public struct SoundManager : IComponentData
{
    public Entity MusicAS;
    public Entity SuccessAS;
    public Entity InputAS;
    public Entity EndAS;
    public Entity HighscoreAS;
}