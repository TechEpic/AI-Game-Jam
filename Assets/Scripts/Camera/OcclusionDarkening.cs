using UnityEngine;

public class OcclusionDarkening : MonoBehaviour {

	// Resolution of the distance map for occlusion rendering
	public int OcclusionResolution;

	// Material to use for drawing
	Material mat;
	
	// Dimensions of the camera
	// May not actually be the dimensions of the camera
	// Don't try to use this elsewhere
	Vector2 camDims;

	// Direction vectors
	Vector2[] dirs;

	GameObject player;

	void Start() {
		// Calculate all the direction vectors only once for giga speed
		dirs = Vision.GetDirections(OcclusionResolution);

		// Get player
		player = GameObject.Find("Player");

		// I stole this code from Unity's docs lol
		// Get the material
		mat = new Material(Shader.Find("Hidden/Internal-Colored"));
		// Idk
		mat.hideFlags = HideFlags.HideAndDontSave;
		// Turn on alpha blending
		mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		// Turn backface culling off
		mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		// Turn off depth writes
		mat.SetInt("_ZWrite", 0);
	}

	// Map a world position to a position on the screen for use in GL calls
	// Function name is short because I have to call it a lot and I value my fingers
	Vector2 GLS(Vector2 pos) {
		return (pos - (Vector2) Camera.main.transform.position + camDims / 2) / camDims;
	}

	void OnPostRender() {
		// Update camera dimensions
		// Is this necessary? I guess it will be if the camera ever zooms in or out
		camDims = new Vector2(2 * Camera.main.orthographicSize * Camera.main.aspect, 2 * Camera.main.orthographicSize);
		// This does... uh, something matrix-related
		GL.PushMatrix();
		// I think
		GL.LoadOrtho();
		// Use the material we set earlier
		mat.SetPass(0);
		GL.Begin(GL.QUADS);
		// Set the shadow color
		GL.Color(new Color(0, 0, 0, 0.7f));
		float[] dists = Vision.GetPlayerDepths(dirs);
		for(int i = 0; i < dists.Length; i++) {
			// Draw a quad as part of the shadow
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[i] * dists[i]));
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[(i + 1) % dirs.Length] * dists[(i + 1) % dists.Length]));
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[(i + 1) % dirs.Length] * 50));
			GL.Vertex(GLS((Vector2) player.transform.position + dirs[i] * 50));
		}
		GL.End();
		GL.PopMatrix();
	}
}
