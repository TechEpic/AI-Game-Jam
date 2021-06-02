using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Pathfinding {

	static Vector2Int[] dirs = {Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right};
	static Tilemap map;
	

	static Pathfinding() {
		map = GameObject.Find("Collidable").GetComponent<Tilemap>();
	}

	public static Dictionary<Vector2Int, Vector2> GetPath(Vector2Int start, Vector2Int end) {
		Dictionary<Vector2Int, Vector2> path = new Dictionary<Vector2Int, Vector2>();
		Queue<Vector2Int> nexts = new Queue<Vector2Int>();
		nexts.Enqueue(start);
		while(nexts.Count > 0) {
			Vector2Int pos = nexts.Dequeue();
			foreach(Vector2Int dir in dirs) {
				Vector2Int newPos = pos + dir;
				if(!path.ContainsKey(newPos) && !map.HasTile((Vector3Int) newPos)) {
					path.Add(newPos, -dir);
					nexts.Enqueue(newPos);
				}
			}
			if(pos.Equals(end)) {
				return path;
			}
		}
		return null;
	}
}
