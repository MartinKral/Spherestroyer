using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SoundManagerSystem))]
public class UpdateMenuUISystem : JobComponentSystem
{
    private EntityQuery soundCrossIconEQ;
    private EntityQuery musicCrossIconEQ;

    protected override void OnCreate()
    {
        EntityQueryDesc soundBtnDesc = new EntityQueryDesc()
        {
            All = new ComponentType[] { ComponentType.ReadOnly<SoundBtnCrossIconTag>() },
            Options = EntityQueryOptions.IncludeDisabled
        };

        soundCrossIconEQ = GetEntityQuery(soundBtnDesc);

        EntityQueryDesc musicBtnDesc = new EntityQueryDesc()
        {
            All = new ComponentType[] { ComponentType.ReadOnly<MusicBtnCrossIconTag>() },
            Options = EntityQueryOptions.IncludeDisabled
        };

        musicCrossIconEQ = GetEntityQuery(musicBtnDesc);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var soundManager = GetSingleton<SoundManager>();

        Entities
            .WithAll<UpdateMenuUITag>() // UpdateMenuUITag
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity) =>
            {
                ToggleSoundCrossIcon(in soundManager);
                ToggleMusicCrossIcon(in soundManager);
                EntityManager.DestroyEntity(entity);
            }).Run();

        return default;
    }

    private void ToggleSoundCrossIcon(in SoundManager soundManager)
    {
        var soundCrossE = soundCrossIconEQ.GetSingletonEntity();
        EntityManager.SetEnabled(soundCrossE, !soundManager.IsSoundEnabled);
    }

    private void ToggleMusicCrossIcon(in SoundManager soundManager)
    {
        var musicCrossE = musicCrossIconEQ.GetSingletonEntity();
        EntityManager.SetEnabled(musicCrossE, !soundManager.IsMusicEnabled);
    }
}