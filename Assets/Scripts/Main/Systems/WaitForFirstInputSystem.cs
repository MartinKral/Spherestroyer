using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Audio;

[AlwaysSynchronizeSystem]
public class WaitForFirstInputSystem : SystemBase
{
    private EntityQuery hideableEQ;

    protected override void OnCreate()
    {
        hideableEQ = GetEntityQuery(ComponentType.ReadOnly<HideableSymbolTag>());
    }

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Entities
            .WithAll<HideableSymbolTag, OnInputTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                ecb.AddComponent<StartGameTag>(ecb.CreateEntity());

                // OnInputTag needs to be removed manually, since this will get disabled immediately
                ecb.RemoveComponent<OnInputTag>(entity);
                ecb.AddComponent(hideableEQ, typeof(Disabled));
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
