﻿using Unity.Collections;
using Unity.Entities;
using Unity.Tiny.Rendering;

namespace IDnet.Game
{
    public class ChangeMaterialComponentSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<RuntimeMaterialReferencesTag>();
            Logger.Log("CHANGE MATERIAL COMPONENT SYSTEM START");
        }

        protected override void OnUpdate()
        {
            var materialReferencesEntity = GetSingletonEntity<RuntimeMaterialReferencesTag>();

            var nBuffer = EntityManager.GetBuffer<RuntimeMaterialReference>(materialReferencesEntity);
            var materials = nBuffer.ToNativeArray(Allocator.Persistent);
            var targetMaterial = materials[0].materialEntity;

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            Entities.ForEach((Entity entity, ref MeshRenderer meshRenderer, ref ChangableMaterialTag changableMaterialTag) =>
            {
                if (EntityManager.HasComponent<LitMaterial>(targetMaterial) &&
                   EntityManager.HasComponent<LitMaterial>(meshRenderer.material))
                {
                    LitMaterial currentMaterial = EntityManager.GetComponentData<LitMaterial>(meshRenderer.material);
                    LitMaterial newMaterial = EntityManager.GetComponentData<LitMaterial>(targetMaterial);

                    ecb.SetComponent(meshRenderer.material, newMaterial);
                }
            });

            ecb.Playback(EntityManager);
            ecb.Dispose();

            materials.Dispose();
        }
    }
}