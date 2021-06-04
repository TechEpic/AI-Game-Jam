using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Pathfinding {

	static Vector2Int[] dirsX = {Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right};
	static Vector2Int[] dirsY = {Vector2Int.left, Vector2Int.right, Vector2Int.down, Vector2Int.up};
	static Tilemap map;

	public static void InitVars() {
		map = GameObject.Find("Collidable").GetComponent<Tilemap>();
	}

	public static Vector2Int TileSpace(Vector2 v) {
		return new Vector2Int(Mathf.FloorToInt(v.x * 2), Mathf.FloorToInt(v.y * 2));
	}

	public static Vector2 WorldSpace(Vector2Int v) {
		return (Vector2) v / 2 + new Vector2(0.25f, 0.25f);
	}

	public static Dictionary<Vector2Int, Vector2> GetPath(Vector2Int start, Vector2Int end) {
		Vector2Int[] dirs;
		Vector2Int rel = end - start;
		rel *= rel;
		dirs = rel.x > rel.y ? dirsX : dirsY;
		Dictionary<Vector2Int, Vector2> path = new Dictionary<Vector2Int, Vector2>();
		path.Add(end, Vector2.zero);
		Queue<Vector2Int> nexts = new Queue<Vector2Int>();
		nexts.Enqueue(end);
		while(nexts.Count > 0) {
			Vector2Int pos = nexts.Dequeue();
			foreach(Vector2Int dir in dirs) {
				Vector2Int newPos = pos + dir;
				if(!path.ContainsKey(newPos) && !map.HasTile((Vector3Int) newPos)) {
					path.Add(newPos, -dir);
					nexts.Enqueue(newPos);
				}
			}
			if(pos.Equals(start)) {
				return path;
			}
		}
		return null;
	}
}
