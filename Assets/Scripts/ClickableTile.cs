using UnityEngine;
using System.Collections;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;

	void OnMouseUp() {
		Debug.Log ("Click!");

		map.MoveSelectedUnitTo(tileX, tileY);
	}

}
