using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TileTests
    {
      
        private Tile tile;
        
        [SetUp]
        public void Setup()
        {
            GameObject example = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Tile"));
            tile = example.GetComponent<Tile>();
        }
        
        [UnityTest]
        public IEnumerator TestIfNoMatches()
        {
            tile.FindAndDeleteMatch();
            Assert.NotNull(tile.GetRend().sprite);
            yield return null;
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(tile.gameObject);
        }
    }
}