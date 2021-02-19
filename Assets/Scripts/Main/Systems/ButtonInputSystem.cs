using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class ButtonInputSystem : SystemBase
{
    private InputWrapperSystem inputSystem;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputWrapperSystem>();
    }

    protected override void OnStartRunning()
    {
    }

    protected override void OnUpdate()
    {
        if (!inputSystem.IsTouchOrButtonDown()) return;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithoutBurst()
            .ForEach((Entity entity, in Button button) =>
            {
                if (inputSystem.IsInRect(button.MinMaxRect))
                {
                    ClickedOnButton(button.Type, ecb);
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return;
    }

    private void ClickedOnButton(ButtonType type, EntityCommandBuffer ecb)
    {
        switch (type)
        {
            case ButtonType.GoToPlay:
                ClickOnPlayBtn(ecb);
                break;

            case ButtonType.GoToMenu:
                ClickOnMenuBtn(ecb);
                break;

            case ButtonType.Branding:
                ClickOnBrandingBtn();
                break;

            case ButtonType.Music:
                ClickOnMusicBtn(ecb);
                break;

            case ButtonType.Sound:
                ClickOnSoundBtn(ecb);
                break;

            case ButtonType.Highscore:
                ClickOnHighscore();
                break;

            default:
                break;
        }
    }

    private void ClickOnHighscore()
    {
        Logger.Log("Click on highscore btn");
        Y8.Api.ShowHighscore("Leaderboard");
    }

    private void ClickOnSoundBtn(EntityCommandBuffer ecb)
    {
        Logger.Log("Click on sound btn");

        ecb.AddComponent<ToggleSoundsTag>(ecb.CreateEntity());
        ecb.AddComponent<UpdateMenuUITag>(ecb.CreateEntity());
    }

    private void ClickOnMusicBtn(EntityCommandBuffer ecb)
    {
        Logger.Log("Click on music btn");
        ecb.AddComponent<ToggleMusicTag>(ecb.CreateEntity());
        ecb.AddComponent<UpdateMenuUITag>(ecb.CreateEntity());
    }

    private void ClickOnBrandingBtn()
    {
        URLOpener.OpenURL("https://www.y8.com/");
    }

    private void ClickOnPlayBtn(EntityCommandBuffer ecb)
    {
        Logger.Log("Click on play btn");
        Entity sceneManagerEntity = GetSingletonEntity<SceneManager>();

        ecb.AddComponent(sceneManagerEntity, new ChangeScene()
        {
            Value = SceneName.Gameplay
        });
    }

    private void ClickOnMenuBtn(EntityCommandBuffer ecb)
    {
        Logger.Log("Click on menu btn");
        Entity sceneManagerEntity = GetSingletonEntity<SceneManager>();

        ecb.AddComponent(sceneManagerEntity, new ChangeScene()
        {
            Value = SceneName.Menu
        });
    }
}
