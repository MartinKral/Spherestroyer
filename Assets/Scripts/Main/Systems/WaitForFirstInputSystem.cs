using Unity.Entities;
using Unity.Tiny.Input;

[AlwaysSynchronizeSystem]
public class WaitForFirstInputSystem : SystemBase
{
    private EntityQuery hideableEQ;
    private InputSystem inputSystem;

    protected override void OnCreate()
    {
        hideableEQ = GetEntityQuery(ComponentType.ReadOnly<HideableSymbolTag>());
        inputSystem = World.GetOrCreateSystem<InputSystem>();

        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
    {
        var gameState = GetSingleton<GameData>();

        if (gameState.currentGameState != GameState.PreGame) return;
        if (!inputSystem.GetMouseButtonUp(0)) return;

        EntityManager.AddComponent<StartGameTag>(EntityManager.CreateEntity());
        EntityManager.AddComponent(hideableEQ, typeof(Disabled));
    }
}
