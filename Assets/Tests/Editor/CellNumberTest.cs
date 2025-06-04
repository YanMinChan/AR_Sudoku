using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Cattoku;

public class CellNumberTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CacheColliders_ShouldReturnMoreThanOneColliders()
    {
        // Use the Assert class to test conditions
        var go = new GameObject("TestNum");

        var cnc = go.AddComponent<CellNumberController>();

        go.AddComponent<BoxCollider>();
        go.AddComponent<MeshCollider>();

        cnc.Start();
        Assert.IsNotNull(cnc.colliders);
        Assert.AreEqual(2, cnc.colliders.Count);
        Assert.IsTrue(cnc.colliders[0] is Collider);
    }
}
