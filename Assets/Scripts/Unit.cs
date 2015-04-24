using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;

	public List<Node> currentPath = null;

	void Update ()
	{
		if (currentPath != null) {
			int currentNode = 0;

			while (currentNode < currentPath.Count -1)
			{
				Vector3 start = map.TileCoordToWorldCoord (currentPath[currentNode].x, currentPath[currentNode].y);
				Vector3 end   = map.TileCoordToWorldCoord (currentPath[currentNode + 1].x, currentPath[currentNode + 1].y);;

				start += Vector3.forward * -1f;
				end += Vector3.forward * -1f;

				Debug.DrawLine (start, end, Color.red);
				currentNode ++;
			}
		}
	}
}
