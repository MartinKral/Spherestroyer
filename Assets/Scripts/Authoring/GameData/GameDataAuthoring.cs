using Unity.Entities;
using UnityEngine;

public class GameDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public GameDataSO GameDataScriptableObject;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        GameSettings.Init(GameDataScriptableObject.GameData);

        dstManager.AddComponentData(entity, GameDataScriptableObject.GameData);
        Debug.Log("CONVERTED");
    }
}