using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public static BoardController Board;
    
    [SerializeField] private GameObject tile;
    [SerializeField] private int size;
    private Tile[,] tiles;
    public List<Sprite> gems = new List<Sprite>();
    private bool isFulling;

    
    
    private void CreateBoard (float offsetX, float offsetY) 
    {
        tiles = new Tile[size, size];    
        var starterX = transform.position.x;     
        var starterY = transform.position.y;

        for (int y = 0; y < size; y++) {  
            for (int x = 0; x < size; x++) {
                List<Sprite> availableGems = new List<Sprite>(); 
                availableGems.AddRange(MakeAvailableGemsList(starterX + (offsetX * x), starterY + (offsetY * y)));
                Sprite newSprite = availableGems[Random.Range(0, availableGems.Count)];
                
                GameObject newTile = Instantiate(tile, new Vector2(starterX + (offsetX * x), starterY + (offsetY * y)), tile.transform.rotation);
                tiles[x, y] = newTile.GetComponent<Tile>();
                newTile.transform.parent = transform;
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
            }
        }
    }

    private List<Sprite> MakeAvailableGemsList(float cordX, float cordY)
    {
        List<Sprite> tempGems = new List<Sprite>();
        tempGems.AddRange(gems);
        RayCastLeft(tempGems, cordX, cordY);
        RayCastDown(tempGems, cordX, cordY);
        return tempGems;
    }

    private void RayCastLeft(List<Sprite> temp, float cordX, float cordY)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(cordX, cordY), Vector2.left);
        if (hits.Length > 1)
        {
            ChangeListByRaycast(temp, hits);
        }
    }
    
    private void RayCastDown(List<Sprite> temp, float cordX, float cordY)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(cordX, cordY), Vector2.down);
        if (hits.Length > 1)
        {
            ChangeListByRaycast(temp, hits);
        }
    }

    private void ChangeListByRaycast(List<Sprite> sprites, RaycastHit2D[] hits)
    {
        var first = hits[0].transform.GetComponent<SpriteRenderer>();
        var second = hits[1].transform.GetComponent<SpriteRenderer>();
        if (first.sprite == second.sprite)
        {
            sprites.Remove(first.sprite);
        }
    }
    
    public IEnumerator FindEmptyTiles() {
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                if (tiles[x, y].GetRend().sprite == null) {
                    
                    yield return StartCoroutine(MoveTilesDown(x, y));
                    break;
                }
            }
        }
        
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                tiles[x, y].FindAndDeleteMatch();
            }
        }
    }
    
    private IEnumerator MoveTilesDown(int x, int y, float delay = .1f) {
        isFulling = true;
        var rends = new List<SpriteRenderer>();
        var emptyCounts = 0;

        for (int i = y; i < size; i++) { 
            SpriteRenderer render = tiles[x, i].GetRend();
            if (render.sprite == null) { 
                emptyCounts++;
            }
            rends.Add(render);
        }

        for (int i = 0; i < emptyCounts; i++)
        {
            
            yield return new WaitForSeconds(delay);
            for (int j = 0; j < rends.Count - 1; j++) { 
                rends[j].sprite = rends[j + 1].sprite;
                rends[j + 1].sprite = null;
            }
            rends[rends.Count-1].sprite = GetNewSprite(x, size - 1);
        }
        
      
        isFulling = false;
    }
    
    private Sprite GetNewSprite(int x, int y) {
        var possibleSprites = new List<Sprite>();
        possibleSprites.AddRange(gems);

        if (x > 0) {
            possibleSprites.Remove(tiles[x - 1, y].GetRend().sprite);
        }
        if (x < size - 1) {
            possibleSprites.Remove(tiles[x + 1, y].GetRend().sprite);
        }
        if (y > 0) {
            possibleSprites.Remove(tiles[x, y - 1].GetRend().sprite);
        }

        return possibleSprites[Random.Range(0, possibleSprites.Count)];
    }

    public void DeleteOneTypeGem(Sprite gem)
    {
      
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++)
            {
                if (tiles[x, y].GetRend().sprite == gem)
                {
                    tiles[x, y].GetRend().sprite = null;
                 
                }

                
            }
        }


    }

    public GameObject GetTile(int x, int y)
    {
        return tiles[x, y].gameObject;
    }

    public int GetSize()
    {
        return size;
    }

    public void ButtonCreate(int xByY)
    {
        size = xByY;
        Board = GetComponent<BoardController>();
        CreateBoard(1, 1);
    }

    public void ResetByButton()
    {
        foreach (Transform child in Board.transform) {
            Destroy(child.gameObject);
        }
    }
}
