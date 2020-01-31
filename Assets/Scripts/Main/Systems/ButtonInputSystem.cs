using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using System;
using Unity.Collections;

[AlwaysSynchronizeSystem]
[AlwaysUpdateSystem]
public class ButtonInputSystem : JobComponentSystem
{
    private InputWrapperSystem inputSystem;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputWrapperSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!inputSystem.IsTouchOrButtonDown()) return default;

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
        return default;
    }

    private void ClickedOnButton(ButtonType type, EntityCommandBuffer ecb)
    {
        switch (type)
        {
            case ButtonType.Play:
                ClickOnPlayBtn(ecb);
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
        //y8Api.ShowHighscore("Leaderboard");
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
}