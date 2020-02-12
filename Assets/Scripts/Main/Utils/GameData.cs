using System;

[Serializable]
public struct GameData
{
    public AnimationSettings Animation;
    public SphereSettings Sphere;

    [Serializable]
    public struct AnimationSettings
    {
        public float speed;
    }

    [Serializable]
    public struct SphereSettings
    {
        public float collisionOffsetY; // 0.5f
    }
}