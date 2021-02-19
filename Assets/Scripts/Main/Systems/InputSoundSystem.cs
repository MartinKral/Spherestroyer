using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

[AlwaysUpdateSystem]
[AlwaysSynchronizeSystem]
public class InputSoundSystem : SystemBase
{
    private InputWrapperSystem inputSystem;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputWrapperSystem>();
    }

    protected override void OnUpdate()
    {
        if (!inputSystem.IsTouchOrButtonDown()) return;

        EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.Input });
    }
}
