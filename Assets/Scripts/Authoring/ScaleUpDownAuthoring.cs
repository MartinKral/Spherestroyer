using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ScaleUpDownAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [Range(0, 2)] public float MinScale;
    [Range(0, 3)] public float MaxScale;
    [Range(0.1f, 2)] public float TransitionDuration;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new NonUniformScale() { Value = MinScale });
        dstManager.AddComponentData(entity, new ScaleAnimation()
        {
            Duration = TransitionDuration,
            MinScale = MinScale,
            MaxScale = MaxScale,
            IsIncreasing = true
        });
    }
}