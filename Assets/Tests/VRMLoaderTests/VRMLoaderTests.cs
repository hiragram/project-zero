using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VRM;

namespace Tests
{
    public class VRMLoaderTests
    {
        [Test]
        public async void LoadVRMTest() {
            var empty = new GameObject();
            var vrmLoader = empty.AddComponent<VRMLoader>();
            vrmLoader.vrmPath = "Assets/Tests/VRMLoaderTests/zero_lightweight.vrm";
            var vrm = await vrmLoader.LoadVRM();

            Assert.IsNotNull(vrm);
            Assert.IsNotNull(vrm.GetComponent<VRMBlendShapeProxy>());
            Assert.IsNotNull(vrm.GetComponent<VRMMeta>());
            Assert.IsNotNull(vrm.GetComponent<VRMHumanoidDescription>());
        }
    }
}
