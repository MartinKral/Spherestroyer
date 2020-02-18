using NUnit.Framework;
using System.Linq;
using Unity.Entities;
using Unity.Tiny;
using Unity.Tiny.GLFW;

namespace Tests
{
    public abstract class WorldTest
    {
        protected World W { get; set; }
        protected EntityManager EM { get; set; }

        [SetUp]
        public void SetUp()
        {
            W = new World("Test World");
            EM = W.EntityManager;

            var allSystems = DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default)
                .Where(s => s != typeof(GLFWWindowSystem));

            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(W, allSystems);

            EM.AddComponent<DisplayInfo>(EM.CreateEntity());
        }

        [TearDown]
        public void TearDown()
        {
            W.Dispose();
        }
    }
}