using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Text;
using Unity.Tiny.UI;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(HighScoreSystem))]
[UpdateAfter(typeof(SphereDestructionSystem))]
public class UpdateScorePartSystem : SystemBase
{
    private HighScoreSystem highScoreSystem;

    protected override void OnCreate()
    {
        highScoreSystem = World.GetOrCreateSystem<HighScoreSystem>();
        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
    {
        // TODO: Update only when sphere destroyed?

        var gameData = GetSingleton<GameData>();

        Entities
            .ForEach((Entity e, ref TextRenderer textRenderer, in UIName uiName) =>
                {
                    if (uiName.Name == "ScoreText")
                    {
                        TextLayout.SetEntityTextRendererString(EntityManager, e, $"{gameData.score}");
                    }

                    if (uiName.Name == "HighscoreText")
                    {
                        TextLayout.SetEntityTextRendererString(EntityManager, e, $"{highScoreSystem.CurrentHighscore}");
                    }
                }).WithStructuralChanges().Run();
    }
}
