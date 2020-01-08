using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace IDnet.Game
{
    public class ParticlesSpawnerAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
    {
        public GameObject particlePrefab;

        public int particlesToSpawn;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Entity prefabEntity = conversionSystem.GetPrimaryEntity(particlePrefab);
            dstManager.AddComponentData(entity, new ParticlesSpawner()
            {
                Prefab = prefabEntity,
                ParticlesToSpawn = particlesToSpawn
            });
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(particlePrefab);
        }
    }
}