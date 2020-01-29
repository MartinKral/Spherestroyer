using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using System;
using Unity.Collections;

[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class ButtonInputSystem : JobComponentSystem
{
    public static Y8.APIController y8Api = new Y8.APIController("5e22f7f4e694aabc7e4d0ee2");
    private InputWrapperSystem inputSystem;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputWrapperSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!inputSystem.IsTouchOrButtonDown()) return default;

        Logger.Log($"Is logged in: {y8Api.IsLoggedIn()}");

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
        y8Api.ShowHighscore("Leaderboard");
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