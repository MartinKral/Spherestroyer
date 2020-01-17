using Unity.Entities;
using UnityEngine;
using AudioClip = UnityEngine.AudioClip;

namespace IDnet.Game
{
    public class SoundManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject MusicAS;
        public GameObject SuccessAS;
        public GameObject InputAS;
        public GameObject EndAS;
        public GameObject HighscoreAS;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new SoundManager()
            {
                MusicAS = conversionSystem.GetPrimaryEntity(MusicAS),
                SuccessAS = conversionSystem.GetPrimaryEntity(SuccessAS),
                InputAS = conversionSystem.GetPrimaryEntity(InputAS),
                EndAS = conversionSystem.GetPrimaryEntity(EndAS),
                HighscoreAS = conversionSystem.GetPrimaryEntity(HighscoreAS)
            });
            dstManager.AddComponent<InputTag>(entity);
        }
    }
}