using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BoardTests
    {
      
        private BoardController board;
        
        [SetUp]
        public void Setup()
        {
            GameObject Board = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Board"));
            board = Board.GetComponent<BoardController>();
            int size = 3;
            board.ButtonCreate(3);
        }
        
        [UnityTest]
        public IEnumerator DeleteOneTypeGemTest()
        {
            Sprite emerald = board.GetTile(0, 0).GetComponent<SpriteRenderer>().sprite;
            board.DeleteOneTypeGem(emerald);
            bool findEmerald = false;
        
            for (int x = 0; x < board.GetSize(); x++)
            {
                for (int y = 0; y < board.GetSize(); y++)
                {
                    var tempSprite = board.GetTile(x,y).GetComponent<SpriteRenderer>().sprite;
                    if (tempSprite == emerald)
                    {
                        findEmerald = true;
                    }
                }
            }

            Assert.IsFalse(findEmerald);
            
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ResetBoardTest()
        {
            board.ResetByButton();
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(board.transform.childCount, 0);
            yield return null;
        }
        
        [TearDown]
        public void Teardown()
        {
            Object.Destroy(board.gameObject);
        }
    }
}
