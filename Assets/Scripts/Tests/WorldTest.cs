using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Entities;
using Unity.Tiny;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class WorldTest
    {
        public World w { get; set; }
        public EntityManager em { get; set; }

        [SetUp]
        public void SetUp()
        {
            w = new World("Test World");
            em = w.EntityManager;
            em.AddComponent<DisplayInfo>(em.CreateEntity());
        }

        [Test]
        public void score_should_increase()
        {
            var gameStateEntity = em.CreateEntity();
            em.AddComponent<GameState>(gameStateEntity);

            var destroyedSphere = em.CreateEntity();
            em.AddComponent<DestroyedTag>(destroyedSphere);
            em.AddComponent<SphereTag>(destroyedSphere);

            w.Update();

            var GameState = em.GetComponentData<GameState>(gameStateEntity);
            Assert.AreEqual(1, GameState.score);
        }
    }
}