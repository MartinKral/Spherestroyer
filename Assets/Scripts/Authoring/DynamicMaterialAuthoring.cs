using Unity.Entities;
using Unity.Rendering;
using Unity.Tiny.Rendering;
using UnityEngine;
using MeshRenderer = Unity.Tiny.Rendering.MeshRenderer;

namespace IDnet.Game
{
    public class DynamicMaterialAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            MeshRenderer meshRenderer = dstManager.GetComponentData<MeshRenderer>(entity);
            dstManager.AddComponent<DynamicMaterial>(meshRenderer.material);
        }
    }
}