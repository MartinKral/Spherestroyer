using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(HighScoreSystem))]
[UpdateAfter(typeof(SphereDestructionSystem))]
[UpdateBefore(typeof(UpdateScoreUISystem))]
public class UpdateScorePartSystem : JobComponentSystem
{
    private HighScoreSystem highScoreSystem;

    protected override void OnCreate()
    {
        highScoreSystem = World.GetOrCreateSystem<HighScoreSystem>();
        RequireSingletonForUpdate<GameState>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var gameData = GetSingleton<GameState>();

        Entities
            .WithAll<ScoreTag, ActivatedTag>()
            .ForEach((Entity entity, ref ScorePart scorePart) =>
            {
                scorePart.TargetScore = gameData.score;
            }).Run();

        Entities
            .WithoutBurst()
            .WithAll<HighscoreTag, ActivatedTag>()
            .ForEach((Entity entity, ref ScorePart scorePart) =>
            {
                scorePart.TargetScore = highScoreSystem.CurrentHighscore;
            }).Run();

        return default;
    }
}