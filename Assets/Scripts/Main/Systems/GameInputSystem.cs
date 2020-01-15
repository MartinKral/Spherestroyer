using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny;
using Unity.Tiny.Audio;
using Unity.Tiny.Input;

[AlwaysUpdateSystem]
[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameInputSystem : JobComponentSystem
{
    private InputSystem Input;

    private EntityQuery inputEntityQuery;

    private BeginSimulationEntityCommandBufferSystem beginInitECBS;
    private EndSimulationEntityCommandBufferSystem endInitECBS;

    protected override void OnCreate()
    {
        Input = World.GetExistingSystem<InputSystem>();

        inputEntityQuery = GetEntityQuery(typeof(InputTag));

        beginInitECBS = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        endInitECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!Input.GetMouseButtonDown(0)) return default;

        EntityCommandBuffer beginBuffer = beginInitECBS.CreateCommandBuffer();
        EntityCommandBuffer endBuffer = endInitECBS.CreateCommandBuffer();

        beginBuffer.AddComponent(inputEntityQuery, typeof(OnInputTag));
        endBuffer.RemoveComponent(inputEntityQuery, typeof(OnInputTag));

        if (IsInputInRect(0.1f, 0.21f, 0.78f, 0.88f)) // Box with Y8 branding
        {
            URLOpener.OpenURL("https://www.y8.com/");
        };

        return default;
    }

    private bool IsInputInRect(float minX, float maxX, float minY, float maxY)
    {
        var posX = Input.GetInputPosition().x;
        var posY = Input.GetInputPosition().y;

        var displayInfo = GetSingleton<DisplayInfo>();

        float targetRatio = 1920.0f / 1080.0f; // internal aspect ratio
        float currentRatio = (float)displayInfo.width / displayInfo.height;

        float ratioDifference = targetRatio / currentRatio;
        float playableCanvasWidth = displayInfo.width;
        float playableCanvasHeight = displayInfo.height;

        if (targetRatio < currentRatio)
        {
            playableCanvasWidth = displayInfo.width * ratioDifference;
            float boundingBoxWidth = (displayInfo.width - playableCanvasWidth) / 2;

            posX -= boundingBoxWidth;
        }
        else
        {
            playableCanvasHeight = displayInfo.height / ratioDifference;
            float boundingBoxHeight = (displayInfo.height - playableCanvasHeight) / 2;

            posY -= boundingBoxHeight;
        }

        float percentualPosX = posX / playableCanvasWidth;
        float percentualPosY = posY / playableCanvasHeight;

        return
            (minX <= percentualPosX) &&
            (percentualPosX <= maxX) &&
            (minY <= percentualPosY) &&
            (percentualPosY <= maxY);
    }
}