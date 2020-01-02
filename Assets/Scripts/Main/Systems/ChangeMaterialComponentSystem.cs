using Unity.Entities;
using Unity.Tiny.Rendering;

namespace IDnet.Game
{
    public class ChangeMaterialComponentSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<SpikeMaterial>();
            Logger.Log("CHANGE MATERIAL COMPONENT SYSTEM START");
        }

        protected override void OnUpdate()
        {
            var materialReferencesEntity = GetSingletonEntity<SpikeMaterial>();
            var materialReferences = EntityManager.GetComponentData<SpikeMaterial>(materialReferencesEntity);

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
            Entities.ForEach((Entity entity, ref MeshRenderer meshRenderer, ref ChangableMaterialTag changableMaterialTag) =>
            {
                // Logger.Log($" Entity {entity}, material references {materialReferences}, material {materialReferences.material}");
                //meshRenderer.material = materialReferences.material;
                if (EntityManager.HasComponent<LitMaterial>(materialReferences.material) &&
                   EntityManager.HasComponent<LitMaterial>(meshRenderer.material))
                {
                    Logger.Log($"HAS MATERIAL {meshRenderer.material}");
                    LitMaterial currentMaterial = EntityManager.GetComponentData<LitMaterial>(meshRenderer.material);

                    LitMaterial newMaterial = EntityManager.GetComponentData<LitMaterial>(materialReferences.material);

                    Logger.Log($"Has DYNAMIC ref {EntityManager.HasComponent<DynamicMaterial>(materialReferences.material)}, " +
                        $"Has DYNAMIC current {EntityManager.HasComponent<DynamicMaterial>(meshRenderer.material)}");

                    Logger.Log($"IS EQUAL: {currentMaterial.Equals(newMaterial)}");
                    ecb.SetComponent(meshRenderer.material, newMaterial);
                }
            });
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}