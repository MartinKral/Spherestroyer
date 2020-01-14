using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class SphereSpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject IcospherePrefab;
    public GameObject spike;
    public float delay;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Entity prefabEntity = conversionSystem.GetPrimaryEntity(IcospherePrefab);

        dstManager.AddComponent(entity, typeof(Disabled));
        dstManager.AddComponent(entity, typeof(EnableOnStartTag));
        dstManager.AddComponentData(prefabEntity, new SpikeReference { Entity = conversionSystem.GetPrimaryEntity(spike) });
        dstManager.AddComponentData(entity, new SphereSpawner()
        {
            Prefab = prefabEntity,
            Delay = delay,
            SecondsUntilSpawn = 0
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(IcospherePrefab);
    }
}