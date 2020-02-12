using Unity.Entities;
using UnityEngine;

namespace IDnet.Game
{
    public class GameDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameDataSO GameDataScriptableObject;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            GameSettings.Init(GameDataScriptableObject.GameData);

            dstManager.DestroyEntity(entity);
        }
    }
}