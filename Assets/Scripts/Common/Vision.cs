using UnityEngine;

public static class Vision {
	
	static LayerMask mask;
	static GameObject player;

	static Vision() {
		player = GameObject.Find("Player");
		mask = LayerMask.GetMask("Default");
	}

	// Determines whether or not the position pos has line of sight to
	// a circular entity at the position target with the radius radius
	public static bool VisionCheck(Vector2 target, Vector2 pos, float radius) {
		// Relative vector from pos to target
		Vector2 rel = target - pos;
		// Distance between the two positions
		float dist = rel.magnitude;
		// Angle of a ray pointing from pos to target
		float angle = Mathf.Atan2(rel.y, rel.x);
		// Angle of a ray intersecting the edge of the target circle, relative to angle
		float theta = Mathf.Asin(radius / dist);
		// Directional vectors of the two rays that intersect the edge of the target circle
		Vector2 dir1 = new Vector2(Mathf.Cos(angle + theta), Mathf.Sin(angle + theta));
		Vector2 dir2 = new Vector2(Mathf.Cos(angle - theta), Mathf.Sin(angle - theta));
		// Distance from pos to the ray circle intersection
		float maxDist = Mathf.Sqrt(dist * dist - radius * radius);
		// Perform the raycasts and store their results
		RaycastHit2D hit1 = Physics2D.Raycast(pos, dir1, maxDist, mask);
		RaycastHit2D hit2 = Physics2D.Raycast(pos, dir2, maxDist, mask);
		// Return true if either ray went past the distance to the circle intersection
		return hit1.collider == null || hit2.collider == null;
	}

	// Returns a bunch of normalized direction vectors in a circle at the desired resolution
	public static Vector2[] GetDirections(int resolution) {
		Vector2[] dirs = new Vector2[resolution];
		for(int i = 0; i < resolution; i++) {
			// Get the angle
			float angle = (float) i / resolution * Mathf.PI * 2;
			// Get the directional vector
			dirs[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
		return dirs;
	}

	// Casts a circle simulating the way an entity can move in the given direction
	public static RaycastHit2D MoveCast(Vector2 pos, Vector2 dir, float radius) {
		return Physics2D.CircleCast(pos, radius, dir, 1e6f, mask);
	}

	// Returns a float array with the resolution of the vec2 array with distance values of rays cast outwards
	// in a circle around the player
	// The array dirs must be have normalized direction vectors going in a circle
	// This is so that you don't have to compute them multiple times, just get them using GetDirections()
	public static float[] GetPlayerDepths(Vector2[] dirs) {
		float[] dists = new float[dirs.Length];
		for(int i = 0; i < dists.Length; i++) {
			// Do the raycast
			RaycastHit2D hit = Physics2D.Raycast(player.transform.position, dirs[i], 1e6f, mask);
			// Check if there was an actual collision
			if(hit.collider == null) {
				// If not, set distance to a million because the unity devs were smoothbrain and made the default 0
				dists[i] = 1e6f;
			} else {
				// Otherwise just set the distance
				dists[i] = hit.distance;
			}
		}
		if(PlayerController.isJumping) {
			for(int i = 0; i < dists.Length; i++) {
				dists[i] = Mathf.Max(PlayerController.JumpVision, dists[i]);
			}
		}
		return dists;
	}
}
