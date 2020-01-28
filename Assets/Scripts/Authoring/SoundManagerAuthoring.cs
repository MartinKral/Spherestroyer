using Unity.Entities;
using UnityEngine;

public class SoundManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public bool IsSoundEnabled = true;
    public bool IsMusicEnabled = true;
    public AudioSource MusicAS;
    public AudioSource SuccessAS;
    public AudioSource InputAS;
    public AudioSource EndAS;
    public AudioSource HighscoreAS;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new SoundManager()
        {
            IsSoundEnabled = IsSoundEnabled,
            IsMusicEnabled = IsMusicEnabled,
            MusicAS = conversionSystem.GetPrimaryEntity(MusicAS),
            SuccessAS = conversionSystem.GetPrimaryEntity(SuccessAS),
            InputAS = conversionSystem.GetPrimaryEntity(InputAS),
            EndAS = conversionSystem.GetPrimaryEntity(EndAS),
            HighscoreAS = conversionSystem.GetPrimaryEntity(HighscoreAS)
        });
    }
}