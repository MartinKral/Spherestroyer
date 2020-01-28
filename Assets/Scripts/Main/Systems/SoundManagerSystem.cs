using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Audio;

[AlwaysSynchronizeSystem]
public class SoundManagerSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<SoundManager>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var soundManager = GetSingleton<SoundManager>();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        TryPlaySounds(ecb, soundManager);
        TryStartMusic(ecb, soundManager);
        TryStopMusic(ecb, soundManager);
        TryToggleMusic(ecb, soundManager);
        TryToggleSound(ecb, soundManager);

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }

    private void TryStopMusic(EntityCommandBuffer ecb, SoundManager soundManager)
    {
        Entities
          .WithoutBurst()
          .WithAll<StopMusicTag>()
          .ForEach((Entity entity) =>
          {
              if (soundManager.IsMusicEnabled) ecb.AddComponent<AudioSourceStop>(soundManager.MusicAS);
              ecb.DestroyEntity(entity);
          }).Run();
    }

    private void TryStartMusic(EntityCommandBuffer ecb, SoundManager soundManager)
    {
        Entities
            .WithoutBurst()
            .WithAll<StartMusicTag>()
            .ForEach((Entity entity) =>
            {
                if (soundManager.IsMusicEnabled) ecb.AddComponent<AudioSourceStart>(soundManager.MusicAS);
                ecb.DestroyEntity(entity);
            }).Run();
    }

    private void TryPlaySounds(EntityCommandBuffer ecb, SoundManager soundManager)
    {
        Entities
            .WithoutBurst()
            .ForEach((Entity entity, in SoundRequest soundRequest) =>
            {
                if (soundManager.IsSoundEnabled) PlaySound(ecb, soundManager, soundRequest.Value);
                ecb.DestroyEntity(entity);
            }).Run();
    }

    private void PlaySound(EntityCommandBuffer ecb, SoundManager soundManager, SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Input:
                ecb.AddComponent<AudioSourceStart>(soundManager.InputAS);
                break;

            case SoundType.Success:
                ecb.AddComponent<AudioSourceStart>(soundManager.SuccessAS);
                break;

            case SoundType.Highscore:
                ecb.AddComponent<AudioSourceStart>(soundManager.HighscoreAS);
                break;

            case SoundType.End:
                ecb.AddComponent<AudioSourceStart>(soundManager.EndAS);
                break;

            default:
                break;
        }
    }

    private void TryToggleMusic(EntityCommandBuffer ecb, SoundManager soundManager)
    {
        Entities
            .WithAll<ToggleMusicTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                soundManager.IsMusicEnabled = !soundManager.IsMusicEnabled;
                SetSingleton(soundManager);

                if (soundManager.IsMusicEnabled)
                {
                    ecb.AddComponent<AudioSourceStart>(soundManager.MusicAS);
                }
                else
                {
                    ecb.AddComponent<AudioSourceStop>(soundManager.MusicAS);
                }

                ecb.DestroyEntity(entity);
            }).Run();
    }

    private void TryToggleSound(EntityCommandBuffer ecb, SoundManager soundManager)
    {
        Entities
            .WithAll<ToggleSoundsTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                soundManager.IsSoundEnabled = !soundManager.IsSoundEnabled;
                SetSingleton(soundManager);

                ecb.DestroyEntity(entity);
            }).Run();
    }
}