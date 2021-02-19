using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny;
using Unity.Tiny.Input;

[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class InputWrapperSystem : SystemBase
{
    private InputSystem Input;

    public bool IsTouchOrButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool IsInRect(MinMaxRect minMaxRect)
    {
        float posX = Input.GetInputPosition().x;
        float posY = Input.GetInputPosition().y;

        var displayInfo = GetSingleton<DisplayInfo>();

        float targetWidth = displayInfo.width;
        float targetHeight = displayInfo.height;

        float playableCanvasWidth = displayInfo.width;
        float playableCanvasHeight = displayInfo.height;

        float targetRatio = 1920.0f / 1080.0f; // internal aspect ratio
        float currentRatio = (float)displayInfo.width / displayInfo.height;

        float ratioDifference = targetRatio / currentRatio;

        if (targetRatio < currentRatio)
        {
            playableCanvasWidth = targetWidth * ratioDifference;
            float boundingBoxWidth = (targetWidth - playableCanvasWidth) / 2;

            posX -= boundingBoxWidth;
        }
        else
        {
            playableCanvasHeight = targetHeight / ratioDifference;
            float boundingBoxHeight = (targetHeight - playableCanvasHeight) / 2;

            posY -= boundingBoxHeight;
        }

        float percentualPosX = posX / playableCanvasWidth;
        float percentualPosY = posY / playableCanvasHeight;

#if !UNITY_DOTSPLAYER
        percentualPosX = UnityEngine.Input.mousePosition.x / UnityEngine.Screen.width;
        percentualPosY = UnityEngine.Input.mousePosition.y / UnityEngine.Screen.height;
#endif

        return
            (minMaxRect.MinX <= percentualPosX) &&
            (percentualPosX <= minMaxRect.MaxX) &&
            (minMaxRect.MinY <= percentualPosY) &&
            (percentualPosY <= minMaxRect.MaxY);
    }

    protected override void OnCreate()
    {
        Input = World.GetExistingSystem<InputSystem>();
    }

    protected override void OnUpdate()
    {
    }
}
