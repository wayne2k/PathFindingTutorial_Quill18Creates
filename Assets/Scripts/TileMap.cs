using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
	
	public class Node {
		public List<Node> neighbours;
		public int x;
		public int y;

		public Node() {
			neighbours = new List<Node>();
		}

		public float DistanceToNode (Node n)
		{
			return Vector2.Distance ( new Vector2 (x, y), new Vector2 (n.x, n.y));
		}
	}
	
	
	void GeneratePathfindingGraph() 
	{
		//initilize the graph
		graph = new Node[mapSizeX,mapSizeY];

		//Initilize the node for each sopt in the array/
		for (int x=0; x < mapSizeX; x++) {
			for (int y=0; y < mapSizeY; y++) {
				graph [x, y] = new Node ();
				graph [x, y].x = x;
				graph [x, y].y = y;
			}
		}

		//Now that all the nodes exist, calculate their neighbours.
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
	
	public void GeneratePathTo(int x, int y) 
	{
		selectedUnit.GetComponent <Unit> ().currentPath = null;
			
		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		// Setup the "Q" List of nodes we havent checked yet.
		List <Node> unvisited = new List<Node> (); 

		Node source = graph[
		                    selectedUnit.GetComponent<Unit>().tileX = x, 
		                    selectedUnit.GetComponent<Unit>().tileY = y
		                    ];

		Node target = graph[x, y];

		dist [source] = 0;
		prev [source] = null;


		//Initilize everyting to have infinity disatance.
		//since we dont know any better right now. its possible some nodes cant be reached from the source
		// which should make INFINITY a reasonable value.
		foreach (Node v in graph) 
		{
			if (v != source) {
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add (v);
		}

		while (unvisited.Count > 0)
		{
//			Node u = unvisited.OrderBy (n => dist[n]).First ();
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach (Node possibleU in unvisited)
			{
				if (u == null || dist[possibleU] < dist[u])
				{
					u = possibleU;
				}
			}

			if (u == target) {
				break;
			}

			unvisited.Remove (u);

			foreach (Node v in u.neighbours)
			{
				float alt = dist[u] + u.DistanceToNode (v);
				if (alt < dist[v])
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		if (prev[target] == null)
		{
			// Not route between target and source;
			return;
		}


		List<Node> currentPath = new List<Node> ();
		Node curr = target;


		//Step through prev chain and add to our path.
		while (curr != null) {
			currentPath.Add (curr);
			curr = prev[curr];
		}

		//Right now currentPath describes a path from our target to out source
		//so we need to invert it.

		currentPath.Reverse ();

		selectedUnit.GetComponent <Unit> ().currentPath = currentPath;
	}
}
