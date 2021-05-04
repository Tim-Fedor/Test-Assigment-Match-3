using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private SpriteRenderer rend;
	private bool isChoose;
	private static Tile _alreadyChoose;
	private bool isMatch;

	private readonly Vector2[] possibleDirs = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	private void Start()
    {
     	rend = GetComponent<SpriteRenderer>();
		isChoose = false;   
    }

	private void Choose() {
		isChoose = true;
		rend.color = Color.gray;
		_alreadyChoose = gameObject.GetComponent<Tile>();
		Debug.Log("Sel");
	}

	private void UnChoose(){
		isChoose = false;
		rend.color = Color.white;
		_alreadyChoose = null;
		Debug.Log("DeSel");
	}

	private void OnMouseDown(){
		if(_alreadyChoose == null){
			if(isChoose){
				UnChoose();
			} else{
				Choose();
			}
		} else{
			
			if(GetPossibleTiles().Contains(_alreadyChoose.gameObject)){
				SwapSprites(_alreadyChoose.rend);
				FindAndDeleteMatch();
				_alreadyChoose.FindAndDeleteMatch();
				
				if(_alreadyChoose.rend.sprite != null && rend.sprite != null){
					SwapSprites(_alreadyChoose.rend);
				}
			

				_alreadyChoose.UnChoose();
			} else{
				_alreadyChoose.UnChoose();
				Choose();
			}
	
		}
		
	}

	private void SwapSprites(SpriteRenderer secondRend){
		if(rend.sprite == secondRend.sprite){
			return;
		}
		
		Sprite temp = rend.sprite;
		rend.sprite = secondRend.sprite;
		secondRend.sprite = temp; 
	}

	private GameObject GetTile(Vector2 dir) {
    	RaycastHit2D hit = Physics2D.Raycast(transform.position, dir);
		
    	if (hit.collider != null) {
	        return hit.transform.gameObject;
	    }
	    return null;
	}

	private List<GameObject> GetPossibleTiles() {
    	var possibleTiles = new List<GameObject>();
	
    	foreach (var t in possibleDirs)
        {
	        possibleTiles.Add(GetTile(t));
        }
    	return possibleTiles;
	}

	private List<GameObject> FindMatch(Vector2 dir) {
    	List<GameObject> matchingTiles = new List<GameObject>();
    	RaycastHit2D hit = Physics2D.Raycast(transform.position, dir);
	    while (hit.transform != null && hit.transform.GetComponent<SpriteRenderer>().sprite == rend.sprite) { 
	        matchingTiles.Add(hit.transform.gameObject);
	        hit = Physics2D.Raycast(hit.transform.position, dir);
	    }
	    return matchingTiles;
	}
	
	private void DeleteMatch(Vector2[] dirs)
	{
    	List<GameObject> matchTiles = new List<GameObject>();
    	foreach (var t in dirs)
        {
	        matchTiles.AddRange(FindMatch(t));
        }

		if(matchTiles.Count >= 4){
			isMatch = true;
			BoardController.Board.DeleteOneTypeGem(rend.sprite);
			ScoreManager.SCORE.Score += 5;
		}
		else if (matchTiles.Count >= 3)
		{
			isMatch = true;

			foreach (var t in dirs)
			{
				RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, t);
				foreach (var t1 in hits)
				{
					t1.transform.GetComponent<SpriteRenderer>().sprite = null;
					ScoreManager.SCORE.Score += 1;
				}
			}
		}
	    else if (matchTiles.Count >= 2)
		{
			foreach (var t in matchTiles)
			{
				t.GetComponent<SpriteRenderer>().sprite = null;
				ScoreManager.SCORE.Score += 1;
			}

			isMatch = true;
		}
	}


	public void FindAndDeleteMatch(){
		DeleteMatch(new Vector2[] { Vector2.left, Vector2.right});
		DeleteMatch(new Vector2[] { Vector2.up, Vector2.down});
		if(isMatch){
			rend.sprite = null;
			isMatch = false;
			ScoreManager.SCORE.Score += 1;
			StartCoroutine(BoardController.Board.FindEmptyTiles());
		}
	}

	public SpriteRenderer GetRend()
	{
		return rend;
	}
}
