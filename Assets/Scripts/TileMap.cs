using UnityEngine;
using System.Collections.Generic;

public class TileMap : MonoBehaviour {
	
	public GameObject selectedUnit;
	
	public TileType[] tileTypes;
	
	int[,] tiles;
	Node[,] graph;
	
	int mapSizeX = 10;
	int mapSizeY = 10;
	
	void Start() {
		GenerateMapData();
		GeneratePathfindingGraph();
		GenerateMapVisual();
	}
	
	void GenerateMapData() {
		// Allocate our map tiles
		tiles = new int[mapSizeX,mapSizeY];
		
		int x,y;
		
		// Initialize our map tiles to be grass
		for(x=0; x < mapSizeX; x++) {
			for(y=0; y < mapSizeY; y++) {
				tiles[x,y] = 0;
			}
		}
		
		// Make a big swamp area
		for(x=3; x <= 5; x++) {
			for(y=0; y < 4; y++) {
				tiles[x,y] = 1;
			}
		}
		
		// Let's make a u-shaped mountain range
		tiles[4, 4] = 2;
		tiles[5, 4] = 2;
		tiles[6, 4] = 2;
		tiles[7, 4] = 2;
		tiles[8, 4] = 2;
		
		tiles[4, 5] = 2;
		tiles[4, 6] = 2;
		tiles[8, 5] = 2;
		tiles[8, 6] = 2;
		
	}
	
	class Node {
		public List<Node> neighbours;
		
		public Node() {
			neighbours = new List<Node>();
		}
	}
	
	
	void GeneratePathfindingGraph() {
		graph = new Node[mapSizeX,mapSizeY];
		
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {

				// We have a 4-way connected map
				// This also works with 6-way hexes and 8-way tiles and n-way variable areas (like EU4)
				
				if(x > 0)
					graph[x,y].neighbours.Add( graph[x-1, y] );
				if(x < mapSizeX-1)
					graph[x,y].neighbours.Add( graph[x+1, y] );
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < mapSizeY-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );
			}
		}
	}
	
	void GenerateMapVisual() {
		for(int x=0; x < mapSizeX; x++) {
			for(int y=0; y < mapSizeY; y++) {
				TileType tt = tileTypes[ tiles[x,y] ];
				GameObject go = (GameObject)Instantiate( tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity );
				
				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
			}
		}
	}
	
	public Vector3 TileCoordToWorldCoord(int x, int y) {
		return new Vector3(x, y, 0);
	}
	
	public void MoveSelectedUnitTo(int x, int y) {
		/*		selectedUnit.GetComponent<Unit>().tileX = x;
		selectedUnit.GetComponent<Unit>().tileY = y;
		selectedUnit.transform.position = TileCoordToWorldCoord(x,y);
*/	
		
		
		
	}
	
}
