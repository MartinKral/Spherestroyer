using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SphereTests : WorldTest
    {
        [Test]
        public void DestroyedSphere_IncreasesScoreByOne()
        {
            var gameStateEntity = EM.CreateEntity();
            EM.AddComponent<GameState>(gameStateEntity);

            var destroyedSphere = EM.CreateEntity();
            EM.AddComponent<DestroyedTag>(destroyedSphere);
            EM.AddComponent<SphereTag>(destroyedSphere);

            W.Update();

            var GameState = EM.GetComponentData<GameState>(gameStateEntity);
            Assert.AreEqual(1, GameState.score);
        }
    }
}