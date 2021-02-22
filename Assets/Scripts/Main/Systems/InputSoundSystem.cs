using Unity.Entities;
using Unity.Tiny.Input;

[AlwaysUpdateSystem]
[AlwaysSynchronizeSystem]
public class InputSoundSystem : SystemBase
{
    private InputSystem inputSystem;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputSystem>();
    }

    protected override void OnUpdate()
    {
        if (!inputSystem.GetMouseButtonDown(0)) return;

        EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.Input });
    }
}
