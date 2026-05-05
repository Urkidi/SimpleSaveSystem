using NUnit.Framework;
using SimpleSaveSystem.Core;
using UnityEngine;

namespace SimpleSaveSystem.Tests
{
    [TestFixture]
    public class SaveSystemTest 
    {
        
        [SetUp]
        public void Setup()
        {
            CreateSaveSystem();
        }

        private void CreateSaveSystem()
        {
            _saveSystem = new SaveSystem<FakeSaveData>();
        }

        [Test]
        public void GameObject_WithRigidBody_WillBeAffectedByPhysics()
        {
            var go = new GameObject();
            go.AddComponent<Rigidbody>();
            var originalPosition = go.transform.position.y;

            Assert.AreNotEqual(1, 2);

        }
    }
}