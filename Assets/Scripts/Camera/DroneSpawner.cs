using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DroneSpawner : MonoBehaviour {

	public GameObject Drone;

	int maxDrones;
	
	ArrayList drones;

	static Tilemap map;
	static BoundsInt bounds;

	static GameObject player;

	// Start is called before the first frame update
	void Start() {
		drones = new ArrayList();
		map = GameObject.Find("Collidable").GetComponent<Tilemap>();
		map.CompressBounds();
		bounds = map.cellBounds;
		player = GameObject.Find("Player");
	}

	public static Vector2 RandomLocation() {
		Vector2Int pos = Vector2Int.zero;
		do {
			pos.x = Random.Range(bounds.xMin + 1, bounds.xMax);
			pos.y = Random.Range(bounds.yMin + 1, bounds.yMax);
		} while(((Vector2) pos - (Vector2) player.transform.position).magnitude < 10
			|| map.HasTile((Vector3Int) pos) || Vision.VisionCheck(Pathfinding.WorldSpace(pos), player.transform.position, 0.2f));
		return Pathfinding.WorldSpace(pos);
	}

	// Update is called once per frame
	void Update() {
		maxDrones = (int) (Time.time / 20 + 10);
		if(drones.Count < maxDrones) {
			Vector2 pos = RandomLocation();
			drones.Add(Instantiate(Drone, pos, Quaternion.identity));
		}
		if(Random.value * 10 < Time.deltaTime) {
			int index = Random.Range(0, drones.Count);
			GameObject drone = (GameObject) drones[index];
			if(!Vision.VisionCheck(drone.transform.position, player.transform.position, 0.2f)) {
				Destroy(drone);
				drones.RemoveAt(index);
			}
		}
	}
}
