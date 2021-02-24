using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.UI;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateBefore(typeof(GameInputSystem))]
public class ButtonInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entity clickedEntity = Entity.Null;
        Entity pressedEntity = Entity.Null;
        Entities.ForEach((Entity e, in UIState uiState) =>
        {
            if (uiState.IsClicked)
            {
                clickedEntity = e;
            }
            else if (uiState.IsPressed)
            {
                pressedEntity = e;
            }
        }).Run();

        var uiSystem = World.GetExistingSystem<ProcessUIEvents>();
        if (clickedEntity != Entity.Null)
        {
            if (IsUiEntityMatch(clickedEntity, uiSystem, "PlayBtn"))
            {
                ClickOnPlayBtn();
            }

            if (IsUiEntityMatch(clickedEntity, uiSystem, "SoundBtn"))
            {
                ClickOnSoundBtn();
            }

            if (IsUiEntityMatch(clickedEntity, uiSystem, "MusicBtn"))
            {
                ClickOnMusicBtn();
            }

            if (IsUiEntityMatch(clickedEntity, uiSystem, "HighscoreBtn"))
            {
                ClickOnHighscore();
            }

            if (IsUiEntityMatch(clickedEntity, uiSystem, "BrandingBtn"))
            {
                ClickOnBrandingBtn();
            }
        }

        if (pressedEntity != Entity.Null)
        {
            if (IsUiEntityMatch(pressedEntity, uiSystem, "MenuBtn"))
            {
                ClickOnMenuBtn();
            }
        }
    }

    private bool IsUiEntityMatch(Entity entity, ProcessUIEvents uiSystem, string uiEntityName)
    {
        return uiSystem.GetEntityByUIName(uiEntityName) == entity;
    }

    private void ClickOnHighscore()
    {
        Logger.Log("Click on highscore btn");
        Y8.Api.ShowHighscore("Leaderboard");
    }

    private void ClickOnSoundBtn()
    {
        Logger.Log("Click on sound btn");

        EntityManager.AddComponent<ToggleSoundsTag>(EntityManager.CreateEntity());
        EntityManager.AddComponent<UpdateMenuUITag>(EntityManager.CreateEntity());
    }

    private void ClickOnMusicBtn()
    {
        Logger.Log("Click on music btn");
        EntityManager.AddComponent<ToggleMusicTag>(EntityManager.CreateEntity());
        EntityManager.AddComponent<UpdateMenuUITag>(EntityManager.CreateEntity());
    }

    private void ClickOnBrandingBtn()
    {
        URLOpener.OpenURL("https://www.y8.com/");
    }

    private void ClickOnPlayBtn()
    {
        EntityManager.AddComponentData(EntityManager.CreateEntity(), new ChangeScene() { Value = SceneName.Gameplay });
        Logger.Log("Click on play btn");
    }

    private void ClickOnMenuBtn()
    {
        Logger.Log("Click on menu btn");
        EntityManager.AddComponentData(EntityManager.CreateEntity(), new ChangeScene() { Value = SceneName.Menu });
    }
}
