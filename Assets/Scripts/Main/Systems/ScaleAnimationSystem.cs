using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class ScaleAnimationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref NonUniformScale scale, ref ScaleAnimation scaleAnimation) =>
        {
            if (scaleAnimation.MaxScale <= scale.Value.x) scaleAnimation.IsIncreasing = false;
            if (scale.Value.x <= scaleAnimation.MinScale) scaleAnimation.IsIncreasing = true;

            float scalePerFrame = (scaleAnimation.MaxScale - scaleAnimation.MinScale) * deltaTime / scaleAnimation.Duration;
            if (scaleAnimation.IsIncreasing)
            {
                scale.Value += scalePerFrame;
            }
            else
            {
                scale.Value -= scalePerFrame;
            }
        }).Run();
    }
}
