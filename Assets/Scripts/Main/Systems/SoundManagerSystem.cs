using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Audio;

[AlwaysSynchronizeSystem]
public class SoundManagerSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<SoundManager>();
    }

    protected override void OnUpdate()
    {
        var soundManager = GetSingleton<SoundManager>();

        TryPlaySounds(soundManager);
        TryStartMusic(soundManager);
        TryStopMusic(soundManager);
        TryToggleMusic(soundManager);
        TryToggleSound(soundManager);
    }

    private void TryStopMusic(SoundManager soundManager)
    {
        Entities
          .WithoutBurst()
          .WithAll<StopMusicTag>()
          .ForEach((Entity entity) =>
          {
              if (soundManager.IsMusicEnabled) EntityManager.AddComponent<AudioSourceStop>(soundManager.MusicAS);
              EntityManager.DestroyEntity(entity);
          }).WithStructuralChanges().Run();
    }

    private void TryStartMusic(SoundManager soundManager)
    {
        Entities
            .WithoutBurst()
            .WithAll<StartMusicTag>()
            .ForEach((Entity entity) =>
            {
                if (soundManager.IsMusicEnabled) EntityManager.AddComponent<AudioSourceStart>(soundManager.MusicAS);
                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }

    private void TryPlaySounds(SoundManager soundManager)
    {
        Entities
            .WithoutBurst()
            .ForEach((Entity entity, in SoundRequest soundRequest) =>
            {
                if (soundManager.IsSoundEnabled) PlaySound(soundManager, soundRequest.Value);
                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }

    private void PlaySound(SoundManager soundManager, SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Input:
                EntityManager.AddComponent<AudioSourceStart>(soundManager.InputAS);
                break;

            case SoundType.Success:
                EntityManager.AddComponent<AudioSourceStart>(soundManager.SuccessAS);
                break;

            case SoundType.Highscore:
                EntityManager.AddComponent<AudioSourceStart>(soundManager.HighscoreAS);
                break;

            case SoundType.End:
                EntityManager.AddComponent<AudioSourceStart>(soundManager.EndAS);
                break;

            default:
                break;
        }
    }

    private void TryToggleMusic(SoundManager soundManager)
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
                    EntityManager.AddComponent<AudioSourceStart>(soundManager.MusicAS);
                }
                else
                {
                    EntityManager.AddComponent<AudioSourceStop>(soundManager.MusicAS);
                }

                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }

    private void TryToggleSound(SoundManager soundManager)
    {
        Entities
            .WithAll<ToggleSoundsTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                soundManager.IsSoundEnabled = !soundManager.IsSoundEnabled;
                SetSingleton(soundManager);

                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }
}
