using Unity.Entities;
using UnityEngine;
using Unity.Tiny.Audio;

namespace IDnet.Game
{
    public class SoundManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent(entity, typeof(AudioSourceStart));
        }
    }
}