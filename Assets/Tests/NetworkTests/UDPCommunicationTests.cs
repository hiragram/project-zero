using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class UDPCommunicationTests {
        [Test]
        public void CanMakeReceiverConnection() {
            var o = new UDPCommunication("other.local", 35001, 35001);
            var client = o.MakeReceiverClient();

            Assert.IsNotNull(client);
        }
    }
}
