using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SoundManagerSystem))]
public class UpdateMenuUISystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<SoundManager>();
    }

    protected override void OnUpdate()
    {
        var soundManager = GetSingleton<SoundManager>();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithAll<UpdateMenuUITag>() // UpdateMenuUITag
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                ToggleSoundCrossIcon(ecb, soundManager.IsSoundEnabled);
                ToggleMusicCrossIcon(ecb, soundManager.IsMusicEnabled);
                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }

    private void ToggleSoundCrossIcon(EntityCommandBuffer ecb, bool isSoundEnabled)
    {
        if (!isSoundEnabled)
        {
            var eq = GetEntityQuery(
                 ComponentType.ReadOnly(typeof(Disabled)),
                 ComponentType.ReadOnly(typeof(SoundBtnCrossIconTag)));
            ecb.RemoveComponent(eq, typeof(Disabled));
        }
        else
        {
            var eq = GetEntityQuery(
                 ComponentType.ReadOnly(typeof(SoundBtnCrossIconTag)));
            ecb.AddComponent(eq, typeof(Disabled));
        }
    }

    private void ToggleMusicCrossIcon(EntityCommandBuffer ecb, bool isMusicEnabled)
    {
        if (!isMusicEnabled)
        {
            var eq = GetEntityQuery(
                 ComponentType.ReadOnly(typeof(Disabled)),
                 ComponentType.ReadOnly(typeof(MusicBtnCrossIconTag)));
            ecb.RemoveComponent(eq, typeof(Disabled));
        }
        else
        {
            var eq = GetEntityQuery(
                 ComponentType.ReadOnly(typeof(MusicBtnCrossIconTag)));
            ecb.AddComponent(eq, typeof(Disabled));
        }
    }
}
